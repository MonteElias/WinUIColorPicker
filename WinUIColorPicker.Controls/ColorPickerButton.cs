// Archivo: WinUIColorPicker.Controls/ColorPickerButton.cs
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Representa un control de tipo botón que muestra una vista previa de un color. Al hacer clic,
/// despliega un selector de color completo ('WinUIColorPicker') dentro de un control flotante (Flyout).
/// </summary>
/// <remarks>
/// Este es un control con plantilla (templated control). Actúa como una fachada, exponiendo
/// propiedades clave que controlan la apariencia y el comportamiento del WinUIColorPicker que contiene.
/// </remarks>
public sealed partial class ColorPickerButton : Control
{
    #region Propiedad Principal del Color
    /// <summary>
    /// Obtiene o establece el color actualmente seleccionado.
    /// </summary>
    public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Color"/>.
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPickerButton), new PropertyMetadata(Colors.Black));
    #endregion

    #region Propiedades de Visibilidad de Vistas
    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de rueda de color está habilitada en el selector de color interno.
    /// </summary>
    public bool IsWheelViewEnabled { get => (bool)GetValue(IsWheelViewEnabledProperty); set => SetValue(IsWheelViewEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsWheelViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsWheelViewEnabledProperty = DependencyProperty.Register(nameof(IsWheelViewEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de paleta de color está habilitada en el selector de color interno.
    /// </summary>
    public bool IsPaletteViewEnabled { get => (bool)GetValue(IsPaletteViewEnabledProperty); set => SetValue(IsPaletteViewEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsPaletteViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsPaletteViewEnabledProperty = DependencyProperty.Register(nameof(IsPaletteViewEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de ajustes de color está habilitada en el selector de color interno.
    /// </summary>
    public bool IsSettingsViewEnabled { get => (bool)GetValue(IsSettingsViewEnabledProperty); set => SetValue(IsSettingsViewEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsSettingsViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSettingsViewEnabledProperty = DependencyProperty.Register(nameof(IsSettingsViewEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));
    #endregion

    #region Propiedades de Visibilidad de Componentes

    /// <summary>
    /// Obtiene o establece un valor que indica si el slider de Alfa junto a la rueda de color en el picker interno está visible.
    /// </summary>
    public bool IsWheelAlphaSliderEnabled { get => (bool)GetValue(IsWheelAlphaSliderEnabledProperty); set => SetValue(IsWheelAlphaSliderEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsWheelAlphaSliderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsWheelAlphaSliderEnabledProperty = DependencyProperty.Register(nameof(IsWheelAlphaSliderEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la fila del canal Alfa está visible en la vista de Ajustes del picker interno.
    /// </summary>
    public bool IsSettingsAlphaChannelEnabled { get => (bool)GetValue(IsSettingsAlphaChannelEnabledProperty); set => SetValue(IsSettingsAlphaChannelEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsSettingsAlphaChannelEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSettingsAlphaChannelEnabledProperty = DependencyProperty.Register(nameof(IsSettingsAlphaChannelEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si el slider de Valor/Brillo está visible junto a la rueda de color en el picker interno.
    /// </summary>
    public bool IsValueSliderEnabled { get => (bool)GetValue(IsValueSliderEnabledProperty); set => SetValue(IsValueSliderEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsValueSliderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsValueSliderEnabledProperty = DependencyProperty.Register(nameof(IsValueSliderEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la barra de acentos en la parte inferior del picker interno está visible.
    /// </summary>
    public bool IsAccentBarEnabled { get => (bool)GetValue(IsAccentBarEnabledProperty); set => SetValue(IsAccentBarEnabledProperty, value); }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsAccentBarEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsAccentBarEnabledProperty = DependencyProperty.Register(nameof(IsAccentBarEnabled), typeof(bool), typeof(ColorPickerButton), new PropertyMetadata(true));

    #endregion

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ColorPickerButton"/>.
    /// </summary>
    public ColorPickerButton()
    {
        this.DefaultStyleKey = typeof(ColorPickerButton);
    }
}