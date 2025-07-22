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
        /// Obtiene el �ndice de la vista seleccionada.
        /// </summary>
        public int SelectedIndex { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ViewSelectedEventArgs"/>.
        /// </summary>
        /// <param name="selectedIndex">El �ndice de la vista que ha sido seleccionada.</param>
        public ViewSelectedEventArgs(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }
    }

    /// <summary>
    /// Representa un control de navegaci�n segmentado que permite al usuario
    /// seleccionar entre diferentes vistas (rueda de color, paleta, ajustes).
    /// </summary>
    /// <remarks>
    /// Este control es un <see cref="UserControl"/> con una plantilla que gestiona
    /// la visibilidad y el estado de selecci�n de los botones de vista internos.
    /// Utiliza animaciones para transiciones suaves entre las selecciones.
    /// </remarks>
    public sealed partial class SegmentedNavigationView : UserControl
    {
        #region Eventos
        /// <summary>
        /// Ocurre cuando una nueva vista ha sido seleccionada en el control de navegaci�n.
        /// </summary>
        public event EventHandler<ViewSelectedEventArgs>? ViewSelected;
        #endregion

        #region Campos Privados
        /// <summary>
        /// Transformaci�n de traslaci�n utilizada para animar el indicador de selecci�n.
        /// </summary>
        private TranslateTransform? _indicatorTransform;

        /// <summary>
        /// Transformaci�n de traslaci�n utilizada para animar el fondo de la selecci�n.
        /// </summary>
        private TranslateTransform? _backgroundTransform;

        /// <summary>
        /// Indica si el control ya ha sido cargado. Utilizado para evitar actualizaciones de UI
        /// antes de que el �rbol visual est� completamente disponible.
        /// </summary>
        private bool _isLoaded = false;
        #endregion

        #region Propiedades de Dependencia

        // --- Propiedad para el �ndice seleccionado ---
        /// <summary>
        /// Obtiene o establece el �ndice de la vista actualmente seleccionada.
        /// El �ndice corresponde al orden de los botones (rueda=0, paleta=1, ajustes=2).
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
        /// M�todo de callback invocado cuando la propiedad de dependencia <see cref="SelectedIndex"/> cambia.
        /// </summary>
        /// <param name="d">El <see cref="DependencyObject"/> en el que la propiedad cambi�.</param>
        /// <param name="e">Los datos del evento que describen el cambio de la propiedad.</param>
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navView = (SegmentedNavigationView)d;
            // Solo actualizar si el control ya est� cargado para asegurar que los elementos visuales est�n disponibles.
            if (navView._isLoaded)
            {
                navView.UpdateSelectionFromProperty((int)e.NewValue);
            }
        }

        // --- Propiedades para habilitar/deshabilitar vistas ---
        /// <summary>
        /// M�todo de callback invocado cuando una de las propiedades de visibilidad de vista cambia.
        /// </summary>
        /// <param name="d">El <see cref="DependencyObject"/> en el que la propiedad cambi�.</param>
        /// <param name="e">Los datos del evento que describen el cambio de la propiedad.</param>
        private static void OnViewEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navView = (SegmentedNavigationView)d;
            // Solo actualizar si el control ya est� cargado para asegurar que los elementos visuales est�n disponibles.
            if (navView._isLoaded)
            {
                navView.UpdateLayout();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si la vista de rueda de color est� habilitada y visible.
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
        /// Obtiene o establece un valor que indica si la vista de paleta de color est� habilitada y visible.
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
        /// Obtiene o establece un valor que indica si la vista de ajustes de color est� habilitada y visible.
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
            this.InitializeComponent(); // Este m�todo se genera autom�ticamente para inicializar componentes XAML.
            this.Loaded += OnControlLoaded; // Suscribe el m�todo OnControlLoaded al evento Loaded.
        }

        /// <summary>
        /// Maneja el evento <see cref="FrameworkElement.Loaded"/> del control.
        /// Se llama cuando el control ha sido cargado en el �rbol visual.
        /// </summary>
        /// <param name="sender">El origen del evento, que es este control.</param>
        /// <param name="e">Datos del evento <see cref="RoutedEventArgs"/>.</param>
        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true; // Marca el control como cargado.

            // Asigna las transformaciones de traslaci�n desde los elementos con plantilla.
            // Se asume que estos nombres (IndicatorTranslateTransform, BackgroundTranslateTransform)
            // corresponden a elementos definidos en la plantilla XAML del control.
            _indicatorTransform = IndicatorTranslateTransform;
            _backgroundTransform = BackgroundTranslateTransform;

            // Suscribe los manejadores de eventos Checked a los botones de radio de cada vista.
            WheelViewButton.Checked += OnViewChanged;
            PaletteViewButton.Checked += OnViewChanged;
            SettingsViewButton.Checked += OnViewChanged;

            UpdateLayout(); // Realiza una actualizaci�n inicial del dise�o.
        }

        /// <summary>
        /// Actualiza el dise�o del control bas�ndose en las propiedades de visibilidad
        /// de las vistas y ajusta las definiciones de columna de la cuadr�cula.
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
                // Asigna cada bot�n a su columna correspondiente en la cuadr�cula.
                Grid.SetColumn(buttons[i], i);
            }

            // Si el bot�n actualmente seleccionado ha sido deshabilitado, selecciona el primer bot�n disponible.
            var selectedButton = buttons.FirstOrDefault(b => b.IsChecked == true);
            if (selectedButton == null)
            {
                var firstAvailableButton = buttons.FirstOrDefault();
                if (firstAvailableButton != null)
                {
                    firstAvailableButton.IsChecked = true;
                    // El evento 'Checked' de ese bot�n se disparar� y se encargar� del resto de la actualizaci�n.
                    return;
                }
            }

            // Actualiza y anima la selecci�n, indicando que es una actualizaci�n inicial.
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
            // Verifica que el remitente sea un RadioButton y que est� marcado.
            if (sender is not RadioButton { IsChecked: true } selectedButton) return;

            var originalButtonIndex = -1;
            // Determina el �ndice original del bot�n seleccionado.
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
            // Actualiza y anima la posici�n del indicador.
            UpdateAndAnimateSelection();
        }

        /// <summary>
        /// Actualiza la selecci�n del bot�n de radio bas�ndose en el nuevo valor de la propiedad <see cref="SelectedIndex"/>.
        /// </summary>
        /// <param name="newIndex">El nuevo �ndice seleccionado.</param>
        private void UpdateSelectionFromProperty(int newIndex)
        {
            // Array de referencia para acceder a los botones por �ndice.
            var buttons = new[] { WheelViewButton, PaletteViewButton, SettingsViewButton };
            if (newIndex >= 0 && newIndex < buttons.Length)
            {
                // Solo marca el bot�n como 'checked' si est� visible.
                if (buttons[newIndex].Visibility == Visibility.Visible)
                {
                    buttons[newIndex].IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Actualiza la posici�n del fondo y el indicador de selecci�n, opcionalmente con animaciones.
        /// </summary>
        /// <param name="isInitialUpdate">
        /// Si es `true`, la actualizaci�n es instant�nea sin animaci�n (�til en la carga inicial).
        /// </param>
        private void UpdateAndAnimateSelection(bool isInitialUpdate = false)
        {
            // Obtiene la lista de botones de vista que est�n actualmente visibles.
            var visibleButtons = new List<RadioButton>();
            if (IsWheelViewEnabled) visibleButtons.Add(WheelViewButton);
            if (IsPaletteViewEnabled) visibleButtons.Add(PaletteViewButton);
            if (IsSettingsViewEnabled) visibleButtons.Add(SettingsViewButton);
            if (!visibleButtons.Any()) return; // Si no hay botones visibles, no hay nada que actualizar.

            // Encuentra el bot�n actualmente seleccionado. Si no hay ninguno, selecciona el primero disponible.
            var selectedButton = visibleButtons.FirstOrDefault(b => b.IsChecked == true);
            if (selectedButton == null)
            {
                selectedButton = visibleButtons.First();
                selectedButton.IsChecked = true; // Asegura que al menos un bot�n est� marcado.
            }

            // Realiza verificaciones de nulidad y tama�o antes de proceder con los c�lculos.
            if (NavigationGrid == null || NavigationGrid.ActualWidth == 0 || _indicatorTransform == null || _backgroundTransform == null) return;

            // Calcula el ancho de cada columna en la cuadr�cula de navegaci�n.
            var columnWidth = NavigationGrid.ActualWidth / NavigationGrid.ColumnDefinitions.Count;
            // Obtiene el �ndice de columna del bot�n seleccionado.
            var buttonIndexInGrid = Grid.GetColumn(selectedButton);
            // Establece el ancho del fondo de selecci�n para que coincida con el ancho de una columna.
            SelectionBackground.Width = columnWidth;

            // Calcula la posici�n objetivo para el fondo de selecci�n.
            var targetBackgroundX = columnWidth * buttonIndexInGrid;
            // Calcula la posici�n objetivo para el indicador de selecci�n (centrado en la columna).
            var targetIndicatorX = targetBackgroundX + (columnWidth / 2) - (SelectedIndicator.Width / 2);

            // Aplica la posici�n directamente si es la actualizaci�n inicial o el control no est� cargado.
            if (isInitialUpdate || !this.IsLoaded)
            {
                _backgroundTransform.X = targetBackgroundX;
                _indicatorTransform.X = targetIndicatorX;
            }
            else // De lo contrario, utiliza animaciones para una transici�n suave.
            {
                var storyboard = new Storyboard();
                var duration = new Duration(TimeSpan.FromMilliseconds(250)); // Duraci�n de la animaci�n.
                var easingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }; // Funci�n de easing para una animaci�n suave.

                // Animaci�n para el fondo de selecci�n.
                var backgroundAnimation = new DoubleAnimation { To = targetBackgroundX, Duration = duration, EasingFunction = easingFunction };
                Storyboard.SetTarget(backgroundAnimation, _backgroundTransform);
                Storyboard.SetTargetProperty(backgroundAnimation, "X");
                storyboard.Children.Add(backgroundAnimation);

                // Animaci�n para el indicador de selecci�n.
                var indicatorAnimation = new DoubleAnimation { To = targetIndicatorX, Duration = duration, EasingFunction = easingFunction };
                Storyboard.SetTarget(indicatorAnimation, _indicatorTransform);
                Storyboard.SetTargetProperty(indicatorAnimation, "X");
                storyboard.Children.Add(indicatorAnimation);

                storyboard.Begin(); // Inicia las animaciones.
            }
        }
    }
}