// Archivo: WinUIColorPicker.Controls.Converters/ColorToCheckerboardVisibilityConverter.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using Windows.UI;

namespace WinUIColorPicker.Controls.Converters;

/// <summary>
/// Convierte un objeto <see cref="Color"/> a un valor de <see cref="Visibility"/>.
/// Devuelve <see cref="Visibility.Visible"/> si el color tiene transparencia (canal Alfa Menor a 255),
/// de lo contrario, devuelve <see cref="Visibility.Collapsed"/>.
/// </summary>
/// <remarks>
/// Este conversor es ideal para enlazar la visibilidad de un fondo de tablero de ajedrez (checkerboard)
/// a una propiedad de color. El tablero solo se mostrará cuando el color sea translúcido.
/// </remarks>
public sealed partial class ColorToCheckerboardVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor de <see cref="Color"/> a un valor de <see cref="Visibility"/>.
    /// </summary>
    /// <param name="value">El valor producido por el origen del enlace. Se espera un objeto <see cref="Color"/>.</param>
    /// <param name="targetType">El tipo de la propiedad de destino del enlace. Se espera <see cref="Visibility"/>.</param>
    /// <param name="parameter">Parámetro de conversor no utilizado.</param>
    /// <param name="language">Información de idioma no utilizada.</param>
    /// <returns>
    /// <see cref="Visibility.Visible"/> si el canal alfa del color es menor a 255;
    /// de lo contrario, <see cref="Visibility.Collapsed"/>.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Color color)
        {
            // Si el color no es completamente opaco, el tablero debe ser visible.
            return color.A < 255 ? Visibility.Visible : Visibility.Collapsed;
        }

        // Por defecto, o si el valor no es un color, el elemento se oculta.
        return Visibility.Collapsed;
    }

    /// <summary>
    /// La conversión inversa no está soportada por este conversor.
    /// </summary>
    /// <param name="value">El valor que es producido por el destino del enlace (ignorado).</param>
    /// <param name="targetType">El tipo al que se va a convertir (ignorado).</param>
    /// <param name="parameter">Parámetro de conversor no utilizado (ignorado).</param>
    /// <param name="language">Información de idioma no utilizada (ignorada).</param>
    /// <returns>Siempre lanza una <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Se lanza siempre, ya que la conversión inversa no tiene un caso de uso válido aquí.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // La conversión en sentido inverso no es necesaria para este caso de uso.
        throw new NotImplementedException("No se puede convertir un valor de Visibilidad de vuelta a un Color.");
    }
}