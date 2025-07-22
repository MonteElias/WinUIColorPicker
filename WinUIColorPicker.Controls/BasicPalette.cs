// Archivo: WinUIColorPicker.Controls/BasicPalette.cs
using System.Collections.Generic;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona una paleta de colores básicos que incluye los colores del arcoíris,
/// además de blanco y negro.
/// </summary>
/// <remarks>
/// Esta clase es una implementación simple de <see cref="IColorPalette"/>, ideal para
/// casos de uso que requieren una selección de colores primarios y secundarios.
/// </remarks>
public class BasicPalette : IColorPalette
{
    /// <summary>
    /// Obtiene una colección que contiene 9 colores básicos:
    /// Rojo, Naranja, Amarillo, Verde, Azul, Índigo, Violeta, Blanco y Negro.
    /// </summary>
    public IEnumerable<Color> Colors => new List<Color>
    {
        // Usamos los colores predefinidos de la clase estática Microsoft.UI.Colors para
        // garantizar la consistencia y legibilidad del código.
        Microsoft.UI.Colors.Red,
        Microsoft.UI.Colors.Orange,
        Microsoft.UI.Colors.Yellow,
        Microsoft.UI.Colors.Green,
        Microsoft.UI.Colors.Blue,
        Microsoft.UI.Colors.Indigo,
        Microsoft.UI.Colors.Violet,
        Microsoft.UI.Colors.White,
        Microsoft.UI.Colors.Black
    };
}