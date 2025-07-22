// Archivo: WinUIColorPicker.Controls.Converters/ColorToSolidColorBrushConverter.cs
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

// ASEGÚRATE DE QUE EL NAMESPACE SEA EL DE TU LIBRERÍA DE CONTROLES
namespace WinUIColorPicker.Controls.Converters;

/// <summary>
/// Un convertidor de valores que convierte un objeto <see cref="Color"/> en un <see cref="SolidColorBrush"/>.
/// </summary>
/// <remarks>
/// Este convertidor es útil para enlazar una propiedad de tipo <see cref="Color"/> directamente a una propiedad
/// de interfaz de usuario que espera un <see cref="Brush"/>, como <see cref="Microsoft.UI.Xaml.Controls.Panel.Background"/>
/// o <see cref="Microsoft.UI.Xaml.Controls.Control.Foreground"/>.
/// Si el valor de entrada no es un <see cref="Color"/>, devuelve un pincel transparente.
/// </remarks>
public partial class ColorToSolidColorBrushConverter : IValueConverter
{
    /// <summary>
    /// Convierte un objeto <see cref="Color"/> en un <see cref="SolidColorBrush"/>.
    /// </summary>
    /// <param name="value">El objeto de origen a convertir, esperado como <see cref="Color"/>.</param>
    /// <param name="targetType">El tipo del argumento de destino de la propiedad, esperado como <see cref="SolidColorBrush"/>.</param>
    /// <param name="parameter">Un parámetro opcional del convertidor (no se utiliza en esta implementación).</param>
    /// <param name="language">El idioma a usar para la conversión (no se utiliza en esta implementación).</param>
    /// <returns>
    /// Un nuevo <see cref="SolidColorBrush"/> con el color especificado,
    /// o un <see cref="SolidColorBrush"/> transparente si el valor de entrada no es un <see cref="Color"/>.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }
        // Si el valor no es un Color, retorna un pincel transparente.
        return new SolidColorBrush(Colors.Transparent);
    }

    /// <summary>
    /// Convierte un <see cref="SolidColorBrush"/> de nuevo a un <see cref="Color"/>.
    /// </summary>
    /// <param name="value">El objeto de destino a convertir de nuevo (esperado como <see cref="SolidColorBrush"/>).</param>
    /// <param name="targetType">El tipo del argumento de destino de la propiedad (esperado como <see cref="Color"/>).</param>
    /// <param name="parameter">Un parámetro opcional del convertidor (no se utiliza en esta implementación).</param>
    /// <param name="language">El idioma a usar para la conversión (no se utiliza en esta implementación).</param>
    /// <returns>
    /// No implementado, siempre lanza una <see cref="NotImplementedException"/>.
    /// </returns>
    /// <exception cref="NotImplementedException">Se lanza siempre, ya que este método no está implementado.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException("La conversión de SolidColorBrush a Color no está implementada.");
    }
}