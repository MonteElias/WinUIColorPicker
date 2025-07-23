using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using Windows.UI;

namespace WinUIColorPicker.Controls
{
    /// <summary>
    /// Un control interno que muestra una barra de acentos de color, con un color base
    /// y varias tonalidades más claras (tintas) y oscuras (sombras).
    /// </summary>
    internal sealed partial class ColorPickerAccent : UserControl
    {
        #region Constantes de Diseño
        private const double LightnessStep1 = 0.15;
        private const double LightnessStep2 = 0.30;
        private const double LightnessStep3 = 0.45;
        #endregion

        private Button[]? _buttons;
        private Button? _activeButton;

        #region Evento ColorSelected
        /// <summary>
        /// Se dispara cuando el usuario hace clic en un botón de la barra, seleccionando un nuevo color.
        /// </summary>
        public event EventHandler<ColorSelectedEventArgs>? ColorSelected;
        #endregion

        #region AccentColor DependencyProperty
        /// <summary>
        /// Obtiene o establece el color base a partir del cual se genera la rampa de colores.
        /// </summary>
        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.Register(
                nameof(AccentColor),
                typeof(Color),
                typeof(ColorPickerAccent),
                new PropertyMetadata(Colors.DodgerBlue, OnAccentColorChanged)); // Mantenemos un valor por defecto robusto

        private static void OnAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPickerAccent bar && e.NewValue is Color newColor)
            {
                // Este callback ahora se activará tanto por el binding como por los cambios de tema.
                bar.UpdateColorRamp(newColor);
            }
        }
        #endregion

        public ColorPickerAccent()
        {
            this.InitializeComponent();

            // ===================================================================================
            // === INICIO DE LA CORRECCIÓN: Suscripción a eventos de ciclo de vida del control ===
            // ===================================================================================
            // Gestionamos el ciclo de vida del control para suscribirnos y desuscribirnos
            // de forma segura a los eventos de cambio de tema.
            this.Loaded += OnAccentBarLoaded;
            this.Unloaded += OnAccentBarUnloaded;
            // ===================================================================================
        }

        // ===================================================================================
        // === INICIO DE LA CORRECCIÓN: Nuevos métodos para gestionar el tema dinámico   ===
        // ===================================================================================

        /// <summary>
        /// Se ejecuta cuando el control se carga en el árbol visual.
        /// Prepara los botones y se suscribe a los cambios de tema.
        /// </summary>
        private void OnAccentBarLoaded(object sender, RoutedEventArgs e)
        {
            _buttons = new[] { ButtonNeg3, ButtonNeg2, ButtonNeg1, ButtonA, ButtonPos1, ButtonPos2, ButtonPos3 };
            foreach (var button in _buttons)
            {
                button.Click += OnStepButtonClick;
            }

            // Suscribirse al evento que se dispara cuando el tema de la aplicación
            // (y por tanto, los recursos de ThemeResource) cambia.
            // Esto hace que el control sea reactivo.
            this.ActualThemeChanged += OnActualThemeChanged;

            // Llama a UpdateColorRamp al inicio con el valor actual de AccentColor
            // que viene del binding del control padre.
            UpdateColorRamp(this.AccentColor);
            SetActiveButton(ButtonA, isInitialSetup: true);
        }

        /// <summary>
        /// Se ejecuta cuando el control se descarga del árbol visual.
        /// Es CRUCIAL darse de baja de los eventos para evitar fugas de memoria.
        /// </summary>
        private void OnAccentBarUnloaded(object sender, RoutedEventArgs e)
        {
            this.ActualThemeChanged -= OnActualThemeChanged;

            if (_buttons != null)
            {
                foreach (var button in _buttons)
                {
                    button.Click -= OnStepButtonClick;
                }
            }
        }

        /// <summary>
        /// Manejador del evento de cambio de tema.
        /// Cuando el tema cambia, simplemente forzamos una re-evaluación de la rampa de color
        /// usando el valor actual de la propiedad AccentColor.
        /// </summary>
        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            // El AccentColor ya está enlazado a la propiedad Color del picker.
            // Un cambio de tema no debería cambiarlo directamente, pero sí puede
            // afectar a otros recursos. Forzar una actualización de la rampa
            // asegura que todo se vuelva a calcular con los recursos más recientes.
            UpdateColorRamp(this.AccentColor);
        }
        // ===================================================================================
        // === FIN DE LA CORRECCIÓN                                                        ===
        // ===================================================================================

        private void OnStepButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                SetActiveButton(clickedButton);
                if (clickedButton.Background is SolidColorBrush brush)
                {
                    ColorSelected?.Invoke(this, new ColorSelectedEventArgs(brush.Color));
                }
            }
        }

        private ScaleTransform? GetButtonScaleTransform(Button? b)
        {
            if (b == null || VisualTreeHelper.GetChildrenCount(b) == 0) return null;
            var root = VisualTreeHelper.GetChild(b, 0) as FrameworkElement;
            var contentPresenter = root?.FindName("ContentPresenter") as ContentPresenter;
            return contentPresenter?.RenderTransform as ScaleTransform;
        }

        private void SetActiveButton(Button buttonToActivate, bool isInitialSetup = false)
        {
            if (_activeButton == buttonToActivate) return;

            if (_activeButton != null)
            {
                VisualStateManager.GoToState(_activeButton, "Unselected", !isInitialSetup);
                LayoutGrid.ColumnDefinitions[Grid.GetColumn(_activeButton)].Width = new GridLength(1, GridUnitType.Star);
            }

            LayoutGrid.ColumnDefinitions[Grid.GetColumn(buttonToActivate)].Width = new GridLength(1.5, GridUnitType.Star);
            var transform = GetButtonScaleTransform(buttonToActivate);
            if (transform != null)
            {
                transform.CenterX = buttonToActivate.ActualWidth / 2;
                transform.CenterY = buttonToActivate.ActualHeight / 2;
            }
            VisualStateManager.GoToState(buttonToActivate, "Selected", !isInitialSetup);
            _activeButton = buttonToActivate;
        }

        private void UpdateColorRamp(Color baseColor)
        {
            if (_buttons == null) return;

            var baseHsl = baseColor.ToHsl();
            var colors = new Color[7];
            var names = new[]
            {
                "Sombra Muy Oscura", "Sombra Oscura", "Sombra Ligera",
                "Color Base",
                "Tinta Ligera", "Tinta Clara", "Tinta Muy Clara"
            };

            colors[3] = baseColor;
            colors[2] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep1, 0, 1), A = baseHsl.A }.ToColor();
            colors[1] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep2, 0, 1), A = baseHsl.A }.ToColor();
            colors[0] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep3, 0, 1), A = baseHsl.A }.ToColor();
            colors[4] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep1, 0, 1), A = baseHsl.A }.ToColor();
            colors[5] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep2, 0, 1), A = baseHsl.A }.ToColor();
            colors[6] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep3, 0, 1), A = baseHsl.A }.ToColor();

            for (int i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];
                var color = colors[i];
                button.Background = new SolidColorBrush(color);
                var toolTip = new ToolTip { Content = $"{names[i]}\n{color.ToHex()}" };
                ToolTipService.SetToolTip(button, toolTip);
            }
        }
    }
}
