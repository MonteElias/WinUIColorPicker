// Archivo: WinUIColorPicker.Controls/WinUIColorPicker.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Representa un control selector de color completo para aplicaciones de interfaz de usuario de Windows (WinUI).
/// Permite seleccionar colores a través de una rueda de color, una paleta predefinida y ajustes numéricos (RGBA/HSVA).
/// </summary>
/// <remarks>
/// Este control hereda de <see cref="Control"/> y utiliza una plantilla para definir su estructura visual.
/// Incluye funcionalidades como:
/// <list type="bullet">
/// <item>Selección de color interactiva (rueda de color).</item>
/// <item>Paleta de colores predefinida y configurable.</item>
/// <item>Entrada y visualización de valores de color en formatos RGBA, HSVA y HEX.</item>
/// <item>Botón para copiar el valor HEX al portapapeles con retroalimentación visual.</item>
/// <item>Navegación entre diferentes modos de vista (rueda, paleta, ajustes).</item>
/// </list>
/// Asume la existencia de propiedades de dependencia definidas en otro parcial de la clase (e.g., WinUIColorPicker.Properties.cs).
/// </remarks>
public sealed partial class WinUIColorPicker : Control
{
    #region Campos para Elementos de Plantilla y Estado

    /// <summary>
    /// Referencia al control <see cref="ComboBox"/> para seleccionar el modelo de color (RGBA/HSVA).
    /// </summary>
    private ComboBox? _colorModelComboBox;

    /// <summary>
    /// Panel que contiene los controles de entrada para los valores RGBA.
    /// </summary>
    private StackPanel? _rgbaPanel;

    /// <summary>
    /// Panel que contiene los controles de entrada para los valores HSVA.
    /// </summary>
    private StackPanel? _hsvaPanel;

    /// <summary>
    /// Cuadrícula que muestra los colores de la paleta.
    /// </summary>
    private GridView? _paletteGridView;

    /// <summary>
    /// Botón para copiar el valor de color HEX al portapapeles.
    /// </summary>
    private Button? _copyHexButton;

    /// <summary>
    /// Campo de texto para mostrar y editar la cadena de color (formato HEX).
    /// </summary>
    private TextBox? _colorStringTextBox;

    /// <summary>
    /// Indica si el control está en proceso de actualización interna para evitar bucles de eventos.
    /// </summary>
    private bool _isUpdating;

    /// <summary>
    /// Barra de acento visual que refleja el color seleccionado.
    /// </summary>
    private ColorPickerAccent? _accentBar;

    /// <summary>
    /// Barra de navegación segmentada para cambiar entre las diferentes vistas del selector de color.
    /// </summary>
    private SegmentedNavigationView? _navigationBar;

    /// <summary>
    /// Contenedor visual para la vista de la rueda de color.
    /// </summary>
    private FrameworkElement? _wheelViewContainer;

    /// <summary>
    /// Contenedor visual para la vista de la paleta de colores.
    /// </summary>
    private FrameworkElement? _paletteViewContainer;

    /// <summary>
    /// Contenedor visual para la vista de ajustes de color.
    /// </summary>
    private FrameworkElement? _settingsViewContainer;

    #endregion

    #region Campos para la Mejora del Botón de Copiar

    /// <summary>
    /// Ícono de fuente dentro del botón de copiar, utilizado para la retroalimentación visual.
    /// </summary>
    private FontIcon? _copyButtonIcon;

    /// <summary>
    /// ToolTip asociado al botón de copiar para proporcionar retroalimentación textual.
    /// </summary>
    private ToolTip? _copyToolTip;

    /// <summary>
    /// Temporizador para controlar la duración de la retroalimentación visual del botón de copiar.
    /// </summary>
    private DispatcherTimer? _copyFeedbackTimer;

    #endregion

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="WinUIColorPicker"/>.
    /// </summary>
    public WinUIColorPicker()
    {
        // Define la clave de estilo predeterminada para este control.
        this.DefaultStyleKey = typeof(WinUIColorPicker);
        // Suscribe un manejador al evento Unloaded para detener el temporizador de retroalimentación si el control se descarga.
        this.Unloaded += (s, e) => _copyFeedbackTimer?.Stop();
    }

    /// <summary>
    /// Se llama cuando se aplica la plantilla del control.
    /// Obtiene referencias a los elementos con nombre de la plantilla
    /// y adjunta los manejadores de eventos necesarios.
    /// </summary>
    protected override void OnApplyTemplate()
    {
        // Desuscribe los eventos de los elementos de plantilla anteriores para evitar fugas de memoria
        // si la plantilla se aplica más de una vez (por ejemplo, al recalcularla).
        if (_colorModelComboBox != null) _colorModelComboBox.SelectionChanged -= OnColorModelChanged;
        if (_paletteGridView != null) _paletteGridView.ItemClick -= OnPaletteColorClicked;
        if (_copyHexButton != null) _copyHexButton.Click -= OnCopyHexButtonClicked;
        if (_colorStringTextBox != null) _colorStringTextBox.KeyDown -= OnColorStringTextBoxKeyDown;
        if (_accentBar != null) _accentBar.ColorSelected -= OnAccentColorSelected;
        if (_navigationBar != null) _navigationBar.ViewSelected -= OnViewSelected;
        if (_copyFeedbackTimer != null) _copyFeedbackTimer.Tick -= OnCopyFeedbackTimerTick;

        // Llama a la implementación base de OnApplyTemplate.
        base.OnApplyTemplate();

        // Obtiene referencias a los elementos con nombre definidos en la plantilla XAML del control.
        _colorModelComboBox = GetTemplateChild("ColorModelComboBox") as ComboBox;
        _rgbaPanel = GetTemplateChild("RgbaPanel") as StackPanel;
        _hsvaPanel = GetTemplateChild("HsvaPanel") as StackPanel;

        // =========================================================================================
        // === CORRECCIÓN APLICADA AQUÍ ===
        // =========================================================================================
        // El nombre del GridView en el XAML es "PaletteViewContainer", no "PaletteGridView".
        // Se corrige el nombre en esta llamada para que la referencia se obtenga correctamente
        // y el evento ItemClick pueda ser suscrito.
        _paletteGridView = GetTemplateChild("PaletteViewContainer") as GridView;
        // =========================================================================================

        _copyHexButton = GetTemplateChild("CopyHexButton") as Button;
        _colorStringTextBox = GetTemplateChild("ColorStringTextBox") as TextBox;
        _accentBar = GetTemplateChild("AccentBar") as ColorPickerAccent;
        _navigationBar = GetTemplateChild("NavigationBar") as SegmentedNavigationView;
        _wheelViewContainer = GetTemplateChild("WheelViewContainer") as FrameworkElement;
        _paletteViewContainer = GetTemplateChild("PaletteViewContainer") as FrameworkElement;
        _settingsViewContainer = GetTemplateChild("SettingsViewContainer") as FrameworkElement;

        // Obtiene referencias adicionales para la lógica de copia y configura el temporizador.
        _copyButtonIcon = GetTemplateChild("CopyButtonIcon") as FontIcon;
        _copyToolTip = GetTemplateChild("CopyToolTip") as ToolTip;
        _copyFeedbackTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
        if (_copyFeedbackTimer != null) _copyFeedbackTimer.Tick += OnCopyFeedbackTimerTick;

        // Suscribe los manejadores de eventos a los elementos de plantilla recién obtenidos.
        if (_colorModelComboBox != null) { _colorModelComboBox.SelectionChanged += OnColorModelChanged; UpdatePanelVisibility(); }
        if (_paletteGridView != null) _paletteGridView.ItemClick += OnPaletteColorClicked;
        if (_copyHexButton != null) _copyHexButton.Click += OnCopyHexButtonClicked;
        if (_colorStringTextBox != null) _colorStringTextBox.KeyDown += OnColorStringTextBoxKeyDown;
        if (_accentBar != null) _accentBar.ColorSelected += OnAccentColorSelected;
        if (_navigationBar != null) _navigationBar.ViewSelected += OnViewSelected;

        // Realiza una actualización inicial de los componentes del control.
        UpdateInternalPalette();
        UpdateAllComponents(this.Color);
        UpdateNavigationView();
    }

    /// <summary>
    /// Se ejecuta cuando el temporizador de retroalimentación de copia ha terminado.
    /// Restaura el ícono del botón de copiar y el texto del ToolTip a sus estados originales,
    /// y detiene el temporizador.
    /// </summary>
    /// <param name="sender">El origen del evento (el <see cref="DispatcherTimer"/>).</param>
    /// <param name="e">Datos del evento.</param>
    private void OnCopyFeedbackTimerTick(object? sender, object e)
    {
        _copyFeedbackTimer?.Stop(); // Detiene el temporizador.
        if (_copyButtonIcon != null)
        {
            _copyButtonIcon.Glyph = "\uE8C8"; // Establece el ícono de "Copiar" (glifo de Fluent UI).
        }
        if (_copyToolTip != null)
        {
            _copyToolTip.IsOpen = false; // Cierra el ToolTip.
            _copyToolTip.Content = "Copiar HEX"; // Restaura el texto original del ToolTip.
        }
    }

    /// <summary>
    /// Actualiza la paleta de colores interna utilizada por el control,
    /// asignando nombres a los colores de la paleta externa si está disponible.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de convertir la colección de colores simples de la interfaz
    /// <see cref="IColorPalette"/> en una lista de objetos <see cref="NamedColors.NamedColor"/>,
    /// lo que permite mostrar nombres de color descriptivos junto con los valores de color.
    /// También maneja duplicados en los nombres asignando un sufijo numérico.
    /// </remarks>
    internal void UpdateInternalPalette()
    {
        // Verifica si una paleta externa válida ha sido establecida y si contiene colores.
        if (this.Palette is IColorPalette p && p.Colors != null)
        {
            var namedPalette = new List<NamedColors.NamedColor>();
            var nameCounts = new Dictionary<string, int>(); // Para manejar nombres de color duplicados.

            foreach (var color in p.Colors)
            {
                // Encuentra el nombre de color más cercano para el color actual.
                var closestNamedColor = NamedColors.FindClosestColor(color);
                string baseName = closestNamedColor.Name;
                string finalName;

                // Si el nombre base no ha sido visto antes, úsalo directamente.
                if (!nameCounts.ContainsKey(baseName))
                {
                    finalName = baseName;
                    nameCounts.Add(baseName, 1);
                }
                else // Si el nombre base ya existe, añade un sufijo numérico.
                {
                    int count = nameCounts[baseName];
                    count++;
                    nameCounts[baseName] = count;
                    finalName = $"{baseName} ({count})";
                }
                namedPalette.Add(new NamedColors.NamedColor(finalName, color));
            }

            // Asigna la paleta de colores con nombres al control.
            this.InternalPalette = namedPalette;
        }
        else
        {
            // Si no hay paleta válida, establece la paleta interna como nula.
            this.InternalPalette = null;
        }
    }

    /// <summary>
    /// Actualiza la barra de navegación segmentada (<see cref="_navigationBar"/>)
    /// para reflejar el estado de las propiedades de habilitación de vista
    /// (<see cref="IsWheelViewEnabled"/>, <see cref="IsPaletteViewEnabled"/>, <see cref="IsSettingsViewEnabled"/>).
    /// </summary>
    private void UpdateNavigationView()
    {
        if (_navigationBar == null) return; // Sale si la barra de navegación no está disponible.

        // Pasa el estado de habilitación de las vistas a la barra de navegación.
        _navigationBar.IsWheelViewEnabled = this.IsWheelViewEnabled;
        _navigationBar.IsPaletteViewEnabled = this.IsPaletteViewEnabled;
        _navigationBar.IsSettingsViewEnabled = this.IsSettingsViewEnabled;

        // Actualiza la vista activa basándose en el índice seleccionado actualmente en la barra de navegación.
        UpdateActiveView(_navigationBar.SelectedIndex);
    }

    /// <summary>
    /// Maneja el evento <see cref="SegmentedNavigationView.ViewSelected"/> cuando una vista es seleccionada
    /// en la barra de navegación.
    /// </summary>
    /// <param name="sender">El origen del evento (<see cref="SegmentedNavigationView"/>).</param>
    /// <param name="e">Datos del evento <see cref="ViewSelectedEventArgs"/>, que contienen el índice de la vista seleccionada.</param>
    private void OnViewSelected(object? sender, ViewSelectedEventArgs e)
    {
        // Actualiza la visibilidad de los contenedores de vista basándose en el índice seleccionado.
        UpdateActiveView(e.SelectedIndex);
    }

    /// <summary>
    /// Controla la visibilidad de los contenedores de vista (rueda, paleta, ajustes)
    /// basándose en el índice proporcionado y las propiedades de habilitación correspondientes.
    /// </summary>
    /// <param name="index">El índice de la vista que debe estar activa (0 para rueda, 1 para paleta, 2 para ajustes).</param>
    private void UpdateActiveView(int index)
    {
        // Establece la visibilidad de cada contenedor de vista.
        // Un contenedor es visible solo si su índice coincide con el índice activo Y su propiedad de habilitación es verdadera.
        if (_wheelViewContainer != null) _wheelViewContainer.Visibility = (index == 0 && IsWheelViewEnabled) ? Visibility.Visible : Visibility.Collapsed;
        if (_paletteViewContainer != null) _paletteViewContainer.Visibility = (index == 1 && IsPaletteViewEnabled) ? Visibility.Visible : Visibility.Collapsed;
        if (_settingsViewContainer != null) _settingsViewContainer.Visibility = (index == 2 && IsSettingsViewEnabled) ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Maneja el evento <see cref="UIElement.KeyDown"/> del campo de texto <see cref="_colorStringTextBox"/>.
    /// </summary>
    /// <param name="sender">El origen del evento (el <see cref="TextBox"/>).</param>
    /// <param name="e">Datos del evento <see cref="KeyRoutedEventArgs"/>.</param>
    private void OnColorStringTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
        // Si la tecla presionada es Enter.
        if (e.Key == VirtualKey.Enter)
        {
            // Fuerza la actualización del origen del binding para que el valor del TextBox se refleje en la propiedad.
            _colorStringTextBox?.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            // Mueve el foco al botón de copiar (si existe) programáticamente.
            _copyHexButton?.Focus(FocusState.Programmatic);
        }
    }

    /// <summary>
    /// Maneja el evento <see cref="ColorPickerAccent.ColorSelected"/> de la barra de acento.
    /// Cuando se selecciona un color en la barra de acento, actualiza el color principal del selector.
    /// </summary>
    /// <param name="sender">El origen del evento (<see cref="ColorPickerAccent"/>).</param>
    /// <param name="e">Datos del evento <see cref="ColorSelectedEventArgs"/>, que contienen el nuevo color seleccionado.</param>
    private void OnAccentColorSelected(object? sender, ColorSelectedEventArgs e)
    {
        this.Color = e.NewColor; // Establece el color principal del control al color seleccionado en la barra de acento.
    }

    /// <summary>
    /// Maneja el evento <see cref="Selector.SelectionChanged"/> del <see cref="_colorModelComboBox"/>.
    /// Actualiza la visibilidad de los paneles RGBA o HSVA según la selección del modelo de color.
    /// </summary>
    /// <param name="sender">El origen del evento (el <see cref="ComboBox"/>).</param>
    /// <param name="e">Datos del evento <see cref="SelectionChangedEventArgs"/>.</param>
    private void OnColorModelChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdatePanelVisibility(); // Llama al método para actualizar la visibilidad de los paneles.
    }

    /// <summary>
    /// Maneja el evento de clic de un elemento en el <see cref="GridView"/> de la paleta.
    /// Cuando se hace clic en un color de la paleta, actualiza el color principal del selector.
    /// </summary>
    /// <param name="sender">El origen del evento (el <see cref="GridView"/>).</param>
    /// <param name="e">Datos del evento <see cref="ItemClickEventArgs"/>, que contienen el elemento clicado.</param>
    private void OnPaletteColorClicked(object sender, ItemClickEventArgs e)
    {
        // Verifica si el elemento clicado es de tipo NamedColors.NamedColor.
        if (e.ClickedItem is NamedColors.NamedColor namedColor)
        {
            this.Color = namedColor.Color; // Establece el color principal del control al color de la paleta seleccionada.
        }
    }

    /// <summary>
    /// Maneja el evento de clic del botón de copiar HEX (<see cref="_copyHexButton"/>).
    /// Copia el valor de color HEX actual al portapapeles y proporciona retroalimentación visual al usuario.
    /// </summary>
    /// <param name="sender">El origen del evento (el <see cref="Button"/>).</param>
    /// <param name="e">Datos del evento <see cref="RoutedEventArgs"/>.</param>
    private void OnCopyHexButtonClicked(object sender, RoutedEventArgs e)
    {
        var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
        dataPackage.SetText(this.ColorString); // Establece el texto a copiar (el valor HEX del color).

        try
        {
            Clipboard.SetContent(dataPackage); // Intenta copiar el contenido al portapapeles.
        }
        catch (Exception ex)
        {
            // En caso de error (por ejemplo, el portapapeles está en uso), escribe un mensaje de depuración.
            Debug.WriteLine($"No se pudo copiar al portapapeles: {ex.Message}");
        }

        // Lógica de retroalimentación visual para el botón de copiar.
        if (_copyButtonIcon != null)
        {
            _copyButtonIcon.Glyph = "\uE10B"; // Cambia el ícono a una marca de verificación.
        }
        if (_copyToolTip != null)
        {
            _copyToolTip.Content = "¡Copiado!"; // Cambia el texto del ToolTip.
            _copyToolTip.IsOpen = true; // Muestra el ToolTip.
        }
        _copyFeedbackTimer?.Start(); // Inicia el temporizador para restaurar el estado original del botón.
    }

    /// <summary>
    /// Actualiza todas las propiedades relacionadas con el color (Hue, Saturation, Value, Alpha, Red, Green, Blue, AlphaByte, ColorString, PureHueColor)
    /// basándose en un nuevo valor de <see cref="Color"/>.
    /// </summary>
    /// <param name="color">El nuevo color que se utilizará para actualizar todos los componentes.</param>
    private void UpdateAllComponents(Color color)
    {
        _isUpdating = true; // Establece el indicador de actualización para evitar bucles de eventos.

        // Convierte el color a HSV para actualizar las propiedades HSV.
        var hsv = color.ToHsv();
        this.Hue = hsv.H;
        this.Saturation = hsv.S;
        this.Value = hsv.V;
        this.Alpha = hsv.A;

        // Actualiza las propiedades RGB.
        this.Red = color.R;
        this.Green = color.G;
        this.Blue = color.B;
        this.AlphaByte = color.A;

        // Actualiza la cadena de color HEX.
        this.ColorString = color.ToHex();

        // Calcula y actualiza el color "PureHue" (mismo matiz, máxima saturación y valor, sin transparencia).
        this.PureHueColor = new HsvColor { H = hsv.H, S = 1, V = 1, A = 1 }.ToColor();

        _isUpdating = false; // Restablece el indicador de actualización.
    }

    /// <summary>
    /// Actualiza las propiedades de color RGB y HEX, así como el color principal del control,
    /// basándose en los cambios en las propiedades de color HSV (Hue, Saturation, Value, Alpha).
    /// </summary>
    private void UpdateFromHsvChange()
    {
        _isUpdating = true; // Establece el indicador de actualización.

        // Crea un nuevo color a partir de las propiedades HSV actuales.
        var newColor = new HsvColor { H = this.Hue, S = this.Saturation, V = this.Value, A = this.Alpha }.ToColor();

        // Actualiza el color principal del control y las propiedades RGB.
        this.Color = newColor;
        this.Red = newColor.R;
        this.Green = newColor.G;
        this.Blue = newColor.B;

        // Actualiza el componente alfa en formato byte y la cadena HEX.
        this.AlphaByte = newColor.A;
        this.ColorString = newColor.ToHex();

        // Actualiza el color "PureHue".
        this.PureHueColor = new HsvColor { H = this.Hue, S = 1, V = 1, A = 1 }.ToColor();

        _isUpdating = false; // Restablece el indicador de actualización.
    }

    /// <summary>
    /// Actualiza las propiedades de color HSV y HEX, así como el color principal del control,
    /// basándose en los cambios en las propiedades de color RGB (Red, Green, Blue, AlphaByte).
    /// </summary>
    private void UpdateFromRgbChange()
    {
        _isUpdating = true; // Establece el indicador de actualización.

        // Crea un nuevo color a partir de las propiedades RGB actuales.
        var newColor = Color.FromArgb((byte)this.AlphaByte, (byte)this.Red, (byte)this.Green, (byte)this.Blue);
        // Convierte el nuevo color a HSV para actualizar las propiedades HSV.
        var hsv = newColor.ToHsv();

        // Actualiza el color principal del control y las propiedades HSV.
        this.Color = newColor;
        this.Hue = hsv.H;
        this.Saturation = hsv.S;
        this.Value = hsv.V;

        // Actualiza el componente alfa en formato flotante y la cadena HEX.
        this.Alpha = hsv.A;
        this.ColorString = newColor.ToHex();

        // Actualiza el color "PureHue".
        this.PureHueColor = new HsvColor { H = hsv.H, S = 1, V = 1, A = 1 }.ToColor();

        _isUpdating = false; // Restablece el indicador de actualización.
    }

    /// <summary>
    /// Actualiza la visibilidad de los paneles de entrada de color (<see cref="_rgbaPanel"/> y <see cref="_hsvaPanel"/>)
    /// basándose en la selección actual del modelo de color en el <see cref="_colorModelComboBox"/>.
    /// </summary>
    private void UpdatePanelVisibility()
    {
        // Sale si los elementos de la UI necesarios no están disponibles.
        if (_rgbaPanel == null || _hsvaPanel == null || _colorModelComboBox == null) return;

        // Determina si el modo RGBA está seleccionado (asumiendo que el índice 0 es RGBA).
        bool isRgbaMode = _colorModelComboBox.SelectedIndex == 0;

        // Establece la visibilidad de los paneles.
        _rgbaPanel.Visibility = isRgbaMode ? Visibility.Visible : Visibility.Collapsed;
        _hsvaPanel.Visibility = isRgbaMode ? Visibility.Collapsed : Visibility.Visible;
    }
}