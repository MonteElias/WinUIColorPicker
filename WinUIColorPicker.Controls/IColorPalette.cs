// Archivo: WinUIColorPicker.Controls/IColorPalette.cs
using System.Collections.Generic;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Define un contrato para las paletas de colores que puede usar el <see cref="WinUIColorPicker"/>.
/// </summary>
/// <remarks>
/// Cualquier clase que implemente esta interfaz puede ser asignada a la propiedad 'Palette'
/// del selector de color, permitiendo a los desarrolladores proporcionar sus propias
/// colecciones de colores personalizadas.
/// </remarks>
public interface IColorPalette
{
    /// <summary>
    /// Obtiene una colección enumerable de los colores que componen la paleta.
    /// </summary>
    /// <value>Una colección de objetos <see cref="Color"/>.</value>
    IEnumerable<Color> Colors { get; }
}