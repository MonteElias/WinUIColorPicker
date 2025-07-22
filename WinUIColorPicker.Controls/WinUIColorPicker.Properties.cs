// Archivo: WinUIColorPicker.Controls/WinUIColorPicker.Properties.cs
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Clase parcial que contiene las definiciones de las propiedades de dependencia (<see cref="DependencyProperty"/>)
/// y sus correspondientes callbacks de cambio para el control <see cref="WinUIColorPicker"/>.
/// Separar las propiedades en un archivo parcial ayuda a mantener el código organizado.
/// </summary>
public sealed partial class WinUIColorPicker : Control
{
    #region Dependency Property Changed Callbacks

    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;
        if (picker._isUpdating) return;
        picker.UpdateAllComponents((Color)e.NewValue);
    }

    private static void OnHsvComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;
        if (picker._isUpdating) return;
        picker.UpdateFromHsvChange();
    }

    private static void OnRgbaComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;
        if (picker._isUpdating) return;
        picker.UpdateFromRgbChange();
    }

    private static void OnColorStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;
        if (picker._isUpdating) return;
        var newColorString = e.NewValue as string;
        if (string.IsNullOrWhiteSpace(newColorString)) return;
        if (ColorParser.TryParse(newColorString, out var newColor))
        {
            picker.Color = newColor;
        }
    }

    private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;

        if (picker.IsLoaded)
        {
            picker.UpdateInternalPalette();
        }
    }

    private static void OnViewEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (WinUIColorPicker)d;
        picker.UpdateNavigationView();
    }

    #endregion

    #region Color Model & Palette Properties

    /// <summary>
    /// Obtiene o establece el color principal seleccionado en el picker. 
    /// Esta es la propiedad de color central del control.
    /// </summary>
    public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Color"/>.
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(WinUIColorPicker), new PropertyMetadata(Colors.Red, OnColorChanged));

    /// <summary>
    /// Obtiene o establece la representación en cadena de texto del color actual (ej. '#FFFF0000').
    /// </summary>
    public string ColorString { get => (string)GetValue(ColorStringProperty); set => SetValue(ColorStringProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="ColorString"/>.
    /// </summary>
    public static readonly DependencyProperty ColorStringProperty = DependencyProperty.Register(nameof(ColorString), typeof(string), typeof(WinUIColorPicker), new PropertyMetadata("#FFFF0000", OnColorStringChanged));

    /// <summary>
    /// Obtiene o establece el componente Matiz (Hue) del color en el modelo HSV. Rango: 0 a 360.
    /// </summary>
    public double Hue { get => (double)GetValue(HueProperty); set => SetValue(HueProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Hue"/>.
    /// </summary>
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(0.0, OnHsvComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Saturación (Saturation) del color en el modelo HSV. Rango: 0.0 a 1.0.
    /// </summary>
    public double Saturation { get => (double)GetValue(SaturationProperty); set => SetValue(SaturationProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Saturation"/>.
    /// </summary>
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(1.0, OnHsvComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Valor/Brillo (Value) del color en el modelo HSV. Rango: 0.0 a 1.0.
    /// </summary>
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Value"/>.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(1.0, OnHsvComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Alfa (transparencia) del color. Rango: 0.0 (transparente) a 1.0 (opaco).
    /// </summary>
    public double Alpha { get => (double)GetValue(AlphaProperty); set => SetValue(AlphaProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Alpha"/>.
    /// </summary>
    public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(nameof(Alpha), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(1.0, OnHsvComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Rojo (Red) del color en el modelo RGBA. Rango: 0 a 255.
    /// </summary>
    public double Red { get => (double)GetValue(RedProperty); set => SetValue(RedProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Red"/>.
    /// </summary>
    public static readonly DependencyProperty RedProperty = DependencyProperty.Register(nameof(Red), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(255.0, OnRgbaComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Verde (Green) del color en el modelo RGBA. Rango: 0 a 255.
    /// </summary>
    public double Green { get => (double)GetValue(GreenProperty); set => SetValue(GreenProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Green"/>.
    /// </summary>
    public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(nameof(Green), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(0.0, OnRgbaComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Azul (Blue) del color en el modelo RGBA. Rango: 0 a 255.
    /// </summary>
    public double Blue { get => (double)GetValue(BlueProperty); set => SetValue(BlueProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Blue"/>.
    /// </summary>
    public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(nameof(Blue), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(0.0, OnRgbaComponentChanged));

    /// <summary>
    /// Obtiene o establece el componente Alfa (transparencia) como un valor de 8 bits. Rango: 0 a 255.
    /// </summary>
    public double AlphaByte { get => (double)GetValue(AlphaByteProperty); set => SetValue(AlphaByteProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="AlphaByte"/>.
    /// </summary>
    public static readonly DependencyProperty AlphaByteProperty = DependencyProperty.Register(nameof(AlphaByte), typeof(double), typeof(WinUIColorPicker), new PropertyMetadata(255.0, OnRgbaComponentChanged));

    /// <summary>
    /// Obtiene o establece la paleta de colores personalizada que se mostrará en la vista de paletas.
    /// Debe ser una clase que implemente la interfaz <see cref="IColorPalette"/>.
    /// </summary>
    public IColorPalette Palette { get => (IColorPalette)GetValue(PaletteProperty); set => SetValue(PaletteProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="Palette"/>.
    /// </summary>
    public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(nameof(Palette), typeof(IColorPalette), typeof(WinUIColorPicker), new PropertyMetadata(new FluentPalette(), OnPaletteChanged));

    /// <summary>
    /// Obtiene la colección de colores procesada internamente para mostrar en el GridView de la paleta.
    /// Esta propiedad es de solo lectura desde el exterior del control.
    /// </summary>
    public IEnumerable<NamedColors.NamedColor>? InternalPalette { get => (IEnumerable<NamedColors.NamedColor>?)GetValue(InternalPaletteProperty); private set { SetValue(InternalPaletteProperty, value); } }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="InternalPalette"/>.
    /// </summary>
    public static readonly DependencyProperty InternalPaletteProperty = DependencyProperty.Register(nameof(InternalPalette), typeof(IEnumerable<NamedColors.NamedColor>), typeof(WinUIColorPicker), new PropertyMetadata(null));

    /// <summary>
    /// Obtiene el color de tono puro (Saturación y Valor al máximo) correspondiente al Matiz actual.
    /// Se utiliza internamente para generar los degradados en los sliders.
    /// </summary>
    public Color PureHueColor { get => (Color)GetValue(PureHueColorProperty); private set { SetValue(PureHueColorProperty, value); } }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="PureHueColor"/>.
    /// </summary>
    private static readonly DependencyProperty PureHueColorProperty = DependencyProperty.Register(nameof(PureHueColor), typeof(Color), typeof(WinUIColorPicker), new PropertyMetadata(Colors.Red));

    #endregion

    #region View Visibility Properties

    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de Rueda de Color está disponible para el usuario.
    /// </summary>
    public bool IsWheelViewEnabled { get => (bool)GetValue(IsWheelViewEnabledProperty); set => SetValue(IsWheelViewEnabledProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsWheelViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsWheelViewEnabledProperty = DependencyProperty.Register(nameof(IsWheelViewEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true, OnViewEnabledChanged));

    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de Paleta está disponible para el usuario.
    /// </summary>
    public bool IsPaletteViewEnabled { get => (bool)GetValue(IsPaletteViewEnabledProperty); set => SetValue(IsPaletteViewEnabledProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsPaletteViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsPaletteViewEnabledProperty = DependencyProperty.Register(nameof(IsPaletteViewEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true, OnViewEnabledChanged));

    /// <summary>
    /// Obtiene o establece un valor que indica si la vista de Ajustes (sliders) está disponible para el usuario.
    /// </summary>
    public bool IsSettingsViewEnabled { get => (bool)GetValue(IsSettingsViewEnabledProperty); set => SetValue(IsSettingsViewEnabledProperty, value); }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsSettingsViewEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSettingsViewEnabledProperty = DependencyProperty.Register(nameof(IsSettingsViewEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true, OnViewEnabledChanged));

    #endregion

    #region Component Visibility Properties

    /// <summary>
    /// Obtiene o establece un valor que indica si el slider de Alfa junto a la rueda de color está visible.
    /// </summary>
    public bool IsWheelAlphaSliderEnabled
    {
        get { return (bool)GetValue(IsWheelAlphaSliderEnabledProperty); }
        set { SetValue(IsWheelAlphaSliderEnabledProperty, value); }
    }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsWheelAlphaSliderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsWheelAlphaSliderEnabledProperty =
        DependencyProperty.Register(nameof(IsWheelAlphaSliderEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la fila completa del canal Alfa (caja de texto y slider) está visible en la vista de Ajustes.
    /// </summary>
    public bool IsSettingsAlphaChannelEnabled
    {
        get { return (bool)GetValue(IsSettingsAlphaChannelEnabledProperty); }
        set { SetValue(IsSettingsAlphaChannelEnabledProperty, value); }
    }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsSettingsAlphaChannelEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSettingsAlphaChannelEnabledProperty =
        DependencyProperty.Register(nameof(IsSettingsAlphaChannelEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si el slider de Valor/Brillo está visible junto a la rueda de color.
    /// </summary>
    public bool IsValueSliderEnabled
    {
        get { return (bool)GetValue(IsValueSliderEnabledProperty); }
        set { SetValue(IsValueSliderEnabledProperty, value); }
    }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsValueSliderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsValueSliderEnabledProperty =
        DependencyProperty.Register(nameof(IsValueSliderEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true));

    /// <summary>
    /// Obtiene o establece un valor que indica si la barra de acentos en la parte inferior del control está visible.
    /// </summary>
    public bool IsAccentBarEnabled
    {
        get { return (bool)GetValue(IsAccentBarEnabledProperty); }
        set { SetValue(IsAccentBarEnabledProperty, value); }
    }
    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="IsAccentBarEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsAccentBarEnabledProperty =
        DependencyProperty.Register(nameof(IsAccentBarEnabled), typeof(bool), typeof(WinUIColorPicker), new PropertyMetadata(true));

    #endregion
}