// Archivo: WinUIColorPicker.Controls/SegmentedNavigationView.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinUIColorPicker.Controls
{
    /// <summary>
    /// Proporciona datos para el evento <see cref="SegmentedNavigationView.ViewSelected"/>.
    /// </summary>
    public class ViewSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Obtiene el índice de la vista seleccionada.
        /// </summary>
        public int SelectedIndex { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ViewSelectedEventArgs"/>.
        /// </summary>
        /// <param name="selectedIndex">El índice de la vista que ha sido seleccionada.</param>
        public ViewSelectedEventArgs(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }
    }

    /// <summary>
    /// Representa un control de navegación segmentado que permite al usuario
    /// seleccionar entre diferentes vistas (rueda de color, paleta, ajustes).
    /// </summary>
    /// <remarks>
    /// Este control es un <see cref="UserControl"/> con una plantilla que gestiona
    /// la visibilidad y el estado de selección de los botones de vista internos.
    /// Utiliza animaciones para transiciones suaves entre las selecciones.
    /// </remarks>
    public sealed partial class SegmentedNavigationView : UserControl
    {
        #region Eventos
        /// <summary>
        /// Ocurre cuando una nueva vista ha sido seleccionada en el control de navegación.
        /// </summary>
        public event EventHandler<ViewSelectedEventArgs>? ViewSelected;
        #endregion

        #region Campos Privados
        /// <summary>
        /// Transformación de traslación utilizada para animar el indicador de selección.
        /// </summary>
        private TranslateTransform? _indicatorTransform;

        /// <summary>
        /// Transformación de traslación utilizada para animar el fondo de la selección.
        /// </summary>
        private TranslateTransform? _backgroundTransform;

        /// <summary>
        /// Indica si el control ya ha sido cargado. Utilizado para evitar actualizaciones de UI
        /// antes de que el árbol visual esté completamente disponible.
        /// </summary>
        private bool _isLoaded = false;
        #endregion

        #region Propiedades de Dependencia

        // --- Propiedad para el índice seleccionado ---
        /// <summary>
        /// Obtiene o establece el índice de la vista actualmente seleccionada.
        /// El índice corresponde al orden de los botones (rueda=0, paleta=1, ajustes=2).
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Identifica la propiedad de dependencia <see cref="SelectedIndex"/>.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(SegmentedNavigationView), new PropertyMetadata(0, OnSelectedIndexChanged));

        /// <summary>
        /// Método de callback invocado cuando la propiedad de dependencia <see cref="SelectedIndex"/> cambia.
        /// </summary>
        /// <param name="d">El <see cref="DependencyObject"/> en el que la propiedad cambió.</param>
        /// <param name="e">Los datos del evento que describen el cambio de la propiedad.</param>
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navView = (SegmentedNavigationView)d;
            // Solo actualizar si el control ya está cargado para asegurar que los elementos visuales están disponibles.
            if (navView._isLoaded)
            {
                navView.UpdateSelectionFromProperty((int)e.NewValue);
            }
        }

        // --- Propiedades para habilitar/deshabilitar vistas ---
        /// <summary>
        /// Método de callback invocado cuando una de las propiedades de visibilidad de vista cambia.
        /// </summary>
        /// <param name="d">El <see cref="DependencyObject"/> en el que la propiedad cambió.</param>
        /// <param name="e">Los datos del evento que describen el cambio de la propiedad.</param>
        private static void OnViewEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navView = (SegmentedNavigationView)d;
            // Solo actualizar si el control ya está cargado para asegurar que los elementos visuales están disponibles.
            if (navView._isLoaded)
            {
                navView.UpdateLayout();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si la vista de rueda de color está habilitada y visible.
        /// </summary>
        public bool IsWheelViewEnabled
        {
            get { return (bool)GetValue(IsWheelViewEnabledProperty); }
            set { SetValue(IsWheelViewEnabledProperty, value); }
        }

        /// <summary>
        /// Identifica la propiedad de dependencia <see cref="IsWheelViewEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsWheelViewEnabledProperty =
            DependencyProperty.Register(nameof(IsWheelViewEnabled), typeof(bool), typeof(SegmentedNavigationView), new PropertyMetadata(true, OnViewEnabledChanged));

        /// <summary>
        /// Obtiene o establece un valor que indica si la vista de paleta de color está habilitada y visible.
        /// </summary>
        public bool IsPaletteViewEnabled
        {
            get { return (bool)GetValue(IsPaletteViewEnabledProperty); }
            set { SetValue(IsPaletteViewEnabledProperty, value); }
        }

        /// <summary>
        /// Identifica la propiedad de dependencia <see cref="IsPaletteViewEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsPaletteViewEnabledProperty =
            DependencyProperty.Register(nameof(IsPaletteViewEnabled), typeof(bool), typeof(SegmentedNavigationView), new PropertyMetadata(true, OnViewEnabledChanged));

        /// <summary>
        /// Obtiene o establece un valor que indica si la vista de ajustes de color está habilitada y visible.
        /// </summary>
        public bool IsSettingsViewEnabled
        {
            get { return (bool)GetValue(IsSettingsViewEnabledProperty); }
            set { SetValue(IsSettingsViewEnabledProperty, value); }
        }

        /// <summary>
        /// Identifica la propiedad de dependencia <see cref="IsSettingsViewEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsSettingsViewEnabledProperty =
            DependencyProperty.Register(nameof(IsSettingsViewEnabled), typeof(bool), typeof(SegmentedNavigationView), new PropertyMetadata(true, OnViewEnabledChanged));

        #endregion

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SegmentedNavigationView"/>.
        /// </summary>
        public SegmentedNavigationView()
        {
            this.InitializeComponent(); // Este método se genera automáticamente para inicializar componentes XAML.
            this.Loaded += OnControlLoaded; // Suscribe el método OnControlLoaded al evento Loaded.
        }

        /// <summary>
        /// Maneja el evento <see cref="FrameworkElement.Loaded"/> del control.
        /// Se llama cuando el control ha sido cargado en el árbol visual.
        /// </summary>
        /// <param name="sender">El origen del evento, que es este control.</param>
        /// <param name="e">Datos del evento <see cref="RoutedEventArgs"/>.</param>
        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true; // Marca el control como cargado.

            // Asigna las transformaciones de traslación desde los elementos con plantilla.
            // Se asume que estos nombres (IndicatorTranslateTransform, BackgroundTranslateTransform)
            // corresponden a elementos definidos en la plantilla XAML del control.
            _indicatorTransform = IndicatorTranslateTransform;
            _backgroundTransform = BackgroundTranslateTransform;

            // Suscribe los manejadores de eventos Checked a los botones de radio de cada vista.
            WheelViewButton.Checked += OnViewChanged;
            PaletteViewButton.Checked += OnViewChanged;
            SettingsViewButton.Checked += OnViewChanged;

            UpdateLayout(); // Realiza una actualización inicial del diseño.
        }

        /// <summary>
        /// Actualiza el diseño del control basándose en las propiedades de visibilidad
        /// de las vistas y ajusta las definiciones de columna de la cuadrícula.
        /// </summary>
        private void UpdateLayout()
        {
            // Crea una lista de botones visibles.
            var buttons = new List<RadioButton>();
            if (IsWheelViewEnabled) buttons.Add(WheelViewButton);
            if (IsPaletteViewEnabled) buttons.Add(PaletteViewButton);
            if (IsSettingsViewEnabled) buttons.Add(SettingsViewButton);

            // Establece la visibilidad de los botones en el XAML.
            WheelViewButton.Visibility = IsWheelViewEnabled ? Visibility.Visible : Visibility.Collapsed;
            PaletteViewButton.Visibility = IsPaletteViewEnabled ? Visibility.Visible : Visibility.Collapsed;
            SettingsViewButton.Visibility = IsSettingsViewEnabled ? Visibility.Visible : Visibility.Collapsed;

            // Limpia las definiciones de columna existentes y crea nuevas basadas en los botones visibles.
            NavigationGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < buttons.Count; i++)
            {
                NavigationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // Asigna cada botón a su columna correspondiente en la cuadrícula.
                Grid.SetColumn(buttons[i], i);
            }

            // Si el botón actualmente seleccionado ha sido deshabilitado, selecciona el primer botón disponible.
            var selectedButton = buttons.FirstOrDefault(b => b.IsChecked == true);
            if (selectedButton == null)
            {
                var firstAvailableButton = buttons.FirstOrDefault();
                if (firstAvailableButton != null)
                {
                    firstAvailableButton.IsChecked = true;
                    // El evento 'Checked' de ese botón se disparará y se encargará del resto de la actualización.
                    return;
                }
            }

            // Actualiza y anima la selección, indicando que es una actualización inicial.
            UpdateAndAnimateSelection(isInitialUpdate: true);
        }

        /// <summary>
        /// Maneja el evento cuando un <see cref="RadioButton"/> es seleccionado.
        /// Actualiza el <see cref="SelectedIndex"/> y dispara el evento <see cref="ViewSelected"/>.
        /// </summary>
        /// <param name="sender">El origen del evento, que es el <see cref="RadioButton"/> que ha sido seleccionado.</param>
        /// <param name="e">Datos del evento <see cref="RoutedEventArgs"/>.</param>
        private void OnViewChanged(object sender, RoutedEventArgs e)
        {
            // Verifica que el remitente sea un RadioButton y que esté marcado.
            if (sender is not RadioButton { IsChecked: true } selectedButton) return;

            var originalButtonIndex = -1;
            // Determina el índice original del botón seleccionado.
            if (selectedButton == WheelViewButton) originalButtonIndex = 0;
            else if (selectedButton == PaletteViewButton) originalButtonIndex = 1;
            else if (selectedButton == SettingsViewButton) originalButtonIndex = 2;

            // Actualiza la propiedad de dependencia SelectedIndex si es diferente.
            if (SelectedIndex != originalButtonIndex)
            {
                SelectedIndex = originalButtonIndex;
            }

            // Dispara el evento ViewSelected para notificar a los suscriptores.
            ViewSelected?.Invoke(this, new ViewSelectedEventArgs(originalButtonIndex));
            // Actualiza y anima la posición del indicador.
            UpdateAndAnimateSelection();
        }

        /// <summary>
        /// Actualiza la selección del botón de radio basándose en el nuevo valor de la propiedad <see cref="SelectedIndex"/>.
        /// </summary>
        /// <param name="newIndex">El nuevo índice seleccionado.</param>
        private void UpdateSelectionFromProperty(int newIndex)
        {
            // Array de referencia para acceder a los botones por índice.
            var buttons = new[] { WheelViewButton, PaletteViewButton, SettingsViewButton };
            if (newIndex >= 0 && newIndex < buttons.Length)
            {
                // Solo marca el botón como 'checked' si está visible.
                if (buttons[newIndex].Visibility == Visibility.Visible)
                {
                    buttons[newIndex].IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Actualiza la posición del fondo y el indicador de selección, opcionalmente con animaciones.
        /// </summary>
        /// <param name="isInitialUpdate">
        /// Si es `true`, la actualización es instantánea sin animación (útil en la carga inicial).
        /// </param>
        private void UpdateAndAnimateSelection(bool isInitialUpdate = false)
        {
            // Obtiene la lista de botones de vista que están actualmente visibles.
            var visibleButtons = new List<RadioButton>();
            if (IsWheelViewEnabled) visibleButtons.Add(WheelViewButton);
            if (IsPaletteViewEnabled) visibleButtons.Add(PaletteViewButton);
            if (IsSettingsViewEnabled) visibleButtons.Add(SettingsViewButton);
            if (!visibleButtons.Any()) return; // Si no hay botones visibles, no hay nada que actualizar.

            // Encuentra el botón actualmente seleccionado. Si no hay ninguno, selecciona el primero disponible.
            var selectedButton = visibleButtons.FirstOrDefault(b => b.IsChecked == true);
            if (selectedButton == null)
            {
                selectedButton = visibleButtons.First();
                selectedButton.IsChecked = true; // Asegura que al menos un botón esté marcado.
            }

            // Realiza verificaciones de nulidad y tamaño antes de proceder con los cálculos.
            if (NavigationGrid == null || NavigationGrid.ActualWidth == 0 || _indicatorTransform == null || _backgroundTransform == null) return;

            // Calcula el ancho de cada columna en la cuadrícula de navegación.
            var columnWidth = NavigationGrid.ActualWidth / NavigationGrid.ColumnDefinitions.Count;
            // Obtiene el índice de columna del botón seleccionado.
            var buttonIndexInGrid = Grid.GetColumn(selectedButton);
            // Establece el ancho del fondo de selección para que coincida con el ancho de una columna.
            SelectionBackground.Width = columnWidth;

            // Calcula la posición objetivo para el fondo de selección.
            var targetBackgroundX = columnWidth * buttonIndexInGrid;
            // Calcula la posición objetivo para el indicador de selección (centrado en la columna).
            var targetIndicatorX = targetBackgroundX + (columnWidth / 2) - (SelectedIndicator.Width / 2);

            // Aplica la posición directamente si es la actualización inicial o el control no está cargado.
            if (isInitialUpdate || !this.IsLoaded)
            {
                _backgroundTransform.X = targetBackgroundX;
                _indicatorTransform.X = targetIndicatorX;
            }
            else // De lo contrario, utiliza animaciones para una transición suave.
            {
                var storyboard = new Storyboard();
                var duration = new Duration(TimeSpan.FromMilliseconds(250)); // Duración de la animación.
                var easingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }; // Función de easing para una animación suave.

                // Animación para el fondo de selección.
                var backgroundAnimation = new DoubleAnimation { To = targetBackgroundX, Duration = duration, EasingFunction = easingFunction };
                Storyboard.SetTarget(backgroundAnimation, _backgroundTransform);
                Storyboard.SetTargetProperty(backgroundAnimation, "X");
                storyboard.Children.Add(backgroundAnimation);

                // Animación para el indicador de selección.
                var indicatorAnimation = new DoubleAnimation { To = targetIndicatorX, Duration = duration, EasingFunction = easingFunction };
                Storyboard.SetTarget(indicatorAnimation, _indicatorTransform);
                Storyboard.SetTargetProperty(indicatorAnimation, "X");
                storyboard.Children.Add(indicatorAnimation);

                storyboard.Begin(); // Inicia las animaciones.
            }
        }
    }
}