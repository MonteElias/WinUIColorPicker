// Archivo: WinUIColorPicker.Controls/ColorChannel.cs
namespace WinUIColorPicker.Controls;

/// <summary>
/// Especifica los canales de color individuales que pueden ser manipulados
/// por los controles del selector de color.
/// </summary>
/// <remarks>
/// Esta enumeración se utiliza para indicar a un control sobre qué componente
/// de un color (ej. RGBA o HSV) debe operar.
/// </remarks>
public enum ColorChannel
{
    /// <summary>
    /// El canal de Matiz (Hue) en el modelo de color HSV/HSL.
    /// Generalmente se representa como un valor de 0 a 360.
    /// </summary>
    Hue,

    /// <summary>
    /// El canal de Saturación (Saturation) en el modelo de color HSV/HSL.
    /// Generalmente se representa como un valor de 0 a 100 o 0.0 a 1.0.
    /// </summary>
    Saturation,

    /// <summary>
    /// El canal de Valor/Brillo (Value/Brightness) en el modelo de color HSV.
    /// Generalmente se representa como un valor de 0 a 100 o 0.0 a 1.0.
    /// </summary>
    Value,

    /// <summary>
    /// El canal Alfa (Alpha), que representa la opacidad del color.
    /// Generalmente se representa como un valor de 0 a 255 o 0.0 a 1.0.
    /// </summary>
    Alpha,

    /// <summary>
    /// El canal Rojo (Red) en el modelo de color RGBA.
    /// Generalmente se representa como un valor de 0 a 255.
    /// </summary>
    Red,

    /// <summary>
    /// El canal Verde (Green) en el modelo de color RGBA.
    /// Generalmente se representa como un valor de 0 a 255.
    /// </summary>
    Green,

    /// <summary>
    /// El canal Azul (Blue) en el modelo de color RGBA.
    /// Generalmente se representa como un valor de 0 a 255.
    /// </summary>
    Blue
}