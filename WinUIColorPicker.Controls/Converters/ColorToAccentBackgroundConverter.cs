// Archivo: WinUIColorPicker.Controls.Converters/ColorToAccentBackgroundConverter.cs
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace WinUIColorPicker.Controls.Converters;

/// <summary>
/// Convierte un objeto <see cref="Color"/> a un <see cref="Brush"/> para un fondo de acento.
/// </summary>
/// <remarks>
/// Si el color de entrada tiene transparencia (canal alfa menor a 255), el conversor
/// devuelve un pincel transparente. Esto permite que un fondo de tablero de ajedrez
/// subyacente sea visible. Si el color es completamente opaco, devuelve un pincel
/// de relleno sutil del tema de la aplicación (<c>SubtleFillColorTransparentBrush</c>).
/// </remarks>
public partial class ColorToAccentBackgroundConverter : IValueConverter
{
    /// <summary>
    /// Convierte un objeto <see cref="Color"/> a un <see cref="Brush"/>.
    /// </summary>
    /// <param name="value">El color de entrada. Se espera un objeto de tipo <see cref="Color"/>.</param>
    /// <param name="targetType">El tipo de la propiedad de destino del enlace (ignorado).</param>
    /// <param name="parameter">Un parámetro opcional para el conversor (ignorado).</param>
    /// <param name="language">La información de idioma/cultura (ignorada).</param>
    /// <returns>
    /// Un <see cref="SolidColorBrush"/> con <c>Colors.Transparent</c> si el color de entrada tiene
    /// un canal alfa menor a 255; de lo contrario, devuelve el recurso de pincel
    /// <c>SubtleFillColorTransparentBrush</c> de la aplicación.
    /// </returns>
    public object? Convert(object value, Type targetType, object? parameter, string? language)
    {
        // Si el color entrante tiene algún nivel de transparencia...
        if (value is Color color && color.A < 255)
        {
            // ...el fondo de la barra de acentos debe ser completamente transparente para no interferir
            // con el fondo de tablero de ajedrez que indica la transparencia.
            return new SolidColorBrush(Colors.Transparent);
        }

        // Si el color es completamente opaco, usamos el pincel de relleno sutil del tema actual.
        return Application.Current.Resources["SubtleFillColorTransparentBrush"] as Brush;
    }

    /// <summary>
    /// La conversión inversa no está soportada.
    /// </summary>
    /// <param name="value">El valor producido por el destino del enlace (ignorado).</param>
    /// <param name="targetType">El tipo al que se va a convertir (ignorado).</param>
    /// <param name="parameter">Un parámetro opcional para el conversor (ignorado).</param>
    /// <param name="language">La información de idioma/cultura (ignorada).</param>
    /// <returns>Siempre lanza una excepción <see cref="NotSupportedException"/>.</returns>
    /// <exception cref="NotSupportedException">Se lanza siempre, ya que la conversión inversa no es necesaria.</exception>
    public object? ConvertBack(object value, Type targetType, object? parameter, string? language)
    {
        throw new NotSupportedException("La conversión de Brush a Color no está soportada por este conversor.");
    }
}