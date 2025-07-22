// Archivo: WinUIColorPicker.Controls/HslColor.cs
namespace WinUIColorPicker.Controls;

/// <summary>
/// Representa un color en el espacio de color HSL (Hue, Saturation, Lightness).
/// Esta estructura es interna y se utiliza en la clase de ayuda <c>ColorHelper</c>.
/// </summary>
public struct HslColor
{
    /// <summary>
    /// Obtiene o establece el componente Matiz (Hue).
    /// </summary>
    /// <value>Un valor de 0 a 360, que representa el ángulo en el círculo cromático.</value>
    public double H { get; set; }

    /// <summary>
    /// Obtiene o establece el componente Saturación (Saturation).
    /// </summary>
    /// <value>Un valor de 0.0 (escala de grises) a 1.0 (color puro).</value>
    public double S { get; set; }

    /// <summary>
    /// Obtiene o establece el componente Luminosidad (Lightness).
    /// </summary>
    /// <value>Un valor de 0.0 (negro) a 1.0 (blanco).</value>
    public double L { get; set; }

    /// <summary>
    /// Obtiene o establece el canal Alfa (Alpha/Opacity).
    /// </summary>
    /// <value>Un valor de 0.0 (completamente transparente) a 1.0 (completamente opaco).</value>
    public double A { get; set; }
}