// Archivo: WinUIColorPicker.Controls/ColorPickerAccent.xaml.cs
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
    /// <remarks>
    /// Su propósito es proporcionar acceso rápido a una paleta monocromática armoniosa
    /// para acelerar el flujo de trabajo de diseño de UI.
    /// </remarks>
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
                new PropertyMetadata(Colors.DodgerBlue, OnAccentColorChanged));

        private static void OnAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPickerAccent bar && e.NewValue is Color newColor)
            {
                bar.UpdateColorRamp(newColor);
            }
        }
        #endregion

        public ColorPickerAccent()
        {
            this.InitializeComponent();
            this.Loaded += OnValueStepperBarLoaded;
        }

        private void OnValueStepperBarLoaded(object sender, RoutedEventArgs e)
        {
            _buttons = new[] { ButtonNeg3, ButtonNeg2, ButtonNeg1, ButtonA, ButtonPos1, ButtonPos2, ButtonPos3 };
            foreach (var button in _buttons)
            {
                button.Click += OnStepButtonClick;
            }
            UpdateColorRamp(this.AccentColor);
            SetActiveButton(ButtonA, isInitialSetup: true);
        }

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

        /// <summary>
        /// Regenera los colores de fondo para todos los botones y asigna un ToolTip informativo a cada uno.
        /// </summary>
        /// <param name="baseColor">El color central a partir del cual se generan las tonalidades.</param>
        private void UpdateColorRamp(Color baseColor)
        {
            if (_buttons == null) return;

            // Se utiliza la clase centralizada ColorHelper para la conversión a HSL.
            var baseHsl = baseColor.ToHsl();

            // Se prepara un array para almacenar los 7 colores de la rampa.
            var colors = new Color[7];

            // Se definen nombres descriptivos para cada paso de la rampa, que se usarán en el ToolTip.
            var names = new[]
            {
                "Sombra Muy Oscura", "Sombra Oscura", "Sombra Ligera",
                "Color Base",
                "Tinta Ligera", "Tinta Clara", "Tinta Muy Clara"
            };

            // Se generan y almacenan todos los colores en el array.
            colors[3] = baseColor; // Color base en el centro.
            // Sombras (menor luminosidad)
            colors[2] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep1, 0, 1), A = baseHsl.A }.ToColor();
            colors[1] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep2, 0, 1), A = baseHsl.A }.ToColor();
            colors[0] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L - LightnessStep3, 0, 1), A = baseHsl.A }.ToColor();
            // Tintas (mayor luminosidad)
            colors[4] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep1, 0, 1), A = baseHsl.A }.ToColor();
            colors[5] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep2, 0, 1), A = baseHsl.A }.ToColor();
            colors[6] = new HslColor { H = baseHsl.H, S = baseHsl.S, L = Math.Clamp(baseHsl.L + LightnessStep3, 0, 1), A = baseHsl.A }.ToColor();

            // Se recorre el array de botones para asignar el color de fondo y el ToolTip.
            for (int i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];
                var color = colors[i];

                // Se establece el color de fondo del botón.
                button.Background = new SolidColorBrush(color);

                // ===================================================================================
                // === IMPLEMENTACIÓN DEL TOOLTIP INFORMATIVO ===
                // ===================================================================================
                // Se crea una nueva instancia de ToolTip para cada botón.
                var toolTip = new ToolTip
                {
                    // El contenido del ToolTip se formatea con el nombre descriptivo y el valor
                    // hexadecimal del color, obtenido a través de nuestro método de extensión ToHex().
                    Content = $"{names[i]}\n{color.ToHex()}"
                };

                // Se asigna el ToolTip al botón usando el servicio ToolTipService.
                ToolTipService.SetToolTip(button, toolTip);
                // ===================================================================================
            }
        }
    }
}