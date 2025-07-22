// Archivo: WinUIColorPicker.Controls/Converters/HsvValueConverter.cs
using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace WinUIColorPicker.Controls.Converters;

/// <summary>
/// Convierte los valores de componentes HSV (Hue, Saturation, Value) entre su
/// modelo de datos numérico (double) y su representación textual en la interfaz de usuario.
/// Se utiliza para formatear los valores en los cuadros de texto y para analizar la entrada del usuario.
/// </summary>
public sealed partial class HsvValueConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor numérico (double) a una cadena (string) formateada para la UI.
    /// </summary>
    /// <param name="value">El valor de entrada, que se espera sea un 'double'. Representa el componente HSV.</param>
    /// <param name="targetType">El tipo de destino de la conversión (ignorado, se asume string).</param>
    /// <param name="parameter">Un parámetro de tipo string que indica el componente a formatear ("H", "S" o "V").</param>
    /// <param name="language">La información de idioma/cultura (ignorada).</param>
    /// <returns>
    /// Una cadena formateada:
    /// - Para Matiz ('H'): como un número entero con el símbolo de grado (ej. "180°").
    /// - Para Saturación ('S') o Valor ('V'): como un porcentaje (ej. "50%").
    /// - Si el parámetro es desconocido o nulo, devuelve el número redondeado como una cadena.
    /// </returns>
    public object? Convert(object value, Type targetType, object? parameter, string? language)
    {
        if (value is not double val)
        {
            return string.Empty;
        }

        string formatType = parameter as string ?? string.Empty;

        switch (formatType)
        {
            case "H": // Hue
                return $"{Math.Round(val)}°";
            case "S": // Saturation
            case "V": // Value
                return $"{Math.Round(val * 100)}%";
            default:
                return Math.Round(val).ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Convierte una cadena (string) desde la UI de vuelta a su valor numérico (double) para el modelo de datos.
    /// </summary>
    /// <param name="value">La cadena de entrada del usuario (ej. "180°", "50%").</param>
    /// <param name="targetType">El tipo de destino de la conversión (ignorado, se asume double).</param>
    /// <param name="parameter">Un parámetro de tipo string que indica el componente que se está analizando ("H", "S" o "V").</param>
    /// <param name="language">La información de idioma/cultura (ignorada).</param>
    /// <returns>
    /// Un valor 'double' que representa el componente HSV:
    /// - Para Matiz ('H'): se normaliza en el rango de 0 a 360.
    /// - Para Saturación ('S') y Valor ('V'): se normaliza en el rango de 0.0 a 1.0.
    /// - Devuelve 0.0 si la cadena de entrada no es válida.
    /// </returns>
    public object? ConvertBack(object value, Type targetType, object? parameter, string? language)
    {
        if (value is not string valStr || string.IsNullOrWhiteSpace(valStr))
        {
            return 0.0;
        }

        // Limpia el string de símbolos comunes ('°', '%') para poder analizarlo numéricamente.
        valStr = valStr.Replace("°", string.Empty).Replace("%", string.Empty).Trim();

        if (!double.TryParse(valStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return 0.0;
        }

        string formatType = parameter as string ?? string.Empty;

        switch (formatType)
        {
            case "H": // Hue
                return Math.Clamp(result, 0, 360);
            case "S": // Saturation
            case "V": // Value
                // Convierte el valor porcentual (0-100) a su equivalente decimal (0.0-1.0).
                return Math.Clamp(result / 100.0, 0.0, 1.0);
            default:
                return result;
        }
    }
}