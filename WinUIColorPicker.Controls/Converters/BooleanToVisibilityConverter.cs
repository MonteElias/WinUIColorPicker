// Archivo: WinUIColorPicker.Controls.Converters/BooleanToVisibilityConverter.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace WinUIColorPicker.Controls.Converters;

/// <summary>
/// Un convertidor de valores que convierte un valor booleano en un valor <see cref="Visibility"/>.
/// </summary>
/// <remarks>
/// Un valor `true` se convierte en <see cref="Visibility.Visible"/>.
/// Un valor `false` se convierte en <see cref="Visibility.Collapsed"/>.
/// </remarks>
public partial class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor booleano en un valor <see cref="Visibility"/>.
    /// </summary>
    /// <param name="value">El valor booleano a convertir (esperado como <see cref="bool"/>).</param>
    /// <param name="targetType">El tipo del argumento de destino de la propiedad (esperado como <see cref="Visibility"/>).</param>
    /// <param name="parameter">Parámetro opcional, no utilizado en esta implementación.</param>
    /// <param name="language">El idioma a usar para la conversión, no utilizado en esta implementación.</param>
    /// <returns>
    /// <see cref="Visibility.Visible"/> si el valor es `true`; de lo contrario, <see cref="Visibility.Collapsed"/>.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Verifica si el valor es un booleano y si es verdadero.
        bool boolValue = value is bool b && b;
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Convierte un valor <see cref="Visibility"/> de nuevo a un valor booleano.
    /// </summary>
    /// <param name="value">El valor <see cref="Visibility"/> a convertir (esperado como <see cref="Visibility"/>).</param>
    /// <param name="targetType">El tipo del argumento de destino de la propiedad (esperado como <see cref="bool"/>).</param>
    /// <param name="parameter">Parámetro opcional, no utilizado en esta implementación.</param>
    /// <param name="language">El idioma a usar para la conversión, no utilizado en esta implementación.</param>
    /// <returns>
    /// `true` si el valor es <see cref="Visibility.Visible"/>; de lo contrario, `false`.
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // Verifica si el valor es de tipo Visibility y si es Visible.
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}