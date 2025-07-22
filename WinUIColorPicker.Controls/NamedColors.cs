// Archivo: WinUIColorPicker.Controls/NamedColors.cs
using System;
using System.Collections.Generic;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona una lista estática de colores con nombre y un método de utilidad para encontrar
/// el color con nombre más cercano a un color de destino dado.
/// </summary>
/// <remarks>
/// La lista de colores se carga una sola vez en un constructor estático para mayor eficiencia.
/// </remarks>
public static class NamedColors
{
    /// <summary>
    /// Representa un par inmutable de un nombre de color y su valor de color correspondiente.
    /// El uso de 'record struct' proporciona igualdad basada en valores de forma automática.
    /// </summary>
    public readonly record struct NamedColor(string Name, Color Color);

    private static readonly List<NamedColor> _namedColors;

    /// <summary>
    /// Inicializador estático que puebla la lista de colores con nombre una sola vez cuando la clase se carga por primera vez.
    /// </summary>
    static NamedColors()
    {
        _namedColors = new List<NamedColor>
        {
            // Rojos y Rosas
            new("Rojo Indio", Color.FromArgb(255, 0xCD, 0x5C, 0x5C)),
            new("Salmón Claro", Color.FromArgb(255, 0xFF, 0xA0, 0x7A)),
            new("Salmón Oscuro", Color.FromArgb(255, 0xE9, 0x96, 0x7A)),
            new("Salmón", Color.FromArgb(255, 0xFA, 0x80, 0x72)),
            new("Carmesí", Color.FromArgb(255, 0xDC, 0x14, 0x3C)),
            new("Rojo", Color.FromArgb(255, 0xFF, 0x00, 0x00)),
            new("Rojo Ladrillo", Color.FromArgb(255, 0xB2, 0x22, 0x22)),
            new("Rojo Oscuro", Color.FromArgb(255, 0x8B, 0x00, 0x00)),
            new("Rosa", Color.FromArgb(255, 0xFF, 0xC0, 0xCB)),
            new("Rosa Claro", Color.FromArgb(255, 0xFF, 0xB6, 0xC1)),
            new("Rosa Fuerte", Color.FromArgb(255, 0xFF, 0x69, 0xB4)),
            new("Rosa Profundo", Color.FromArgb(255, 0xFF, 0x14, 0x93)),
            new("Violeta Pálido", Color.FromArgb(255, 0xDB, 0x70, 0x93)),

            // Naranjas
            new("Coral", Color.FromArgb(255, 0xFF, 0x7F, 0x50)),
            new("Tomate", Color.FromArgb(255, 0xFF, 0x63, 0x47)),
            new("Rojo Naranja", Color.FromArgb(255, 0xFF, 0x45, 0x00)),
            new("Naranja Oscuro", Color.FromArgb(255, 0xFF, 0x8C, 0x00)),
            new("Naranja", Color.FromArgb(255, 0xFF, 0xA5, 0x00)),

            // Amarillos y Dorados
            new("Oro", Color.FromArgb(255, 0xFF, 0xD7, 0x00)),
            new("Amarillo", Color.FromArgb(255, 0xFF, 0xFF, 0x00)),
            new("Amarillo Claro", Color.FromArgb(255, 0xFF, 0xFF, 0xE0)),
            new("Limón", Color.FromArgb(255, 0xFF, 0xFA, 0xCD)),
            new("Dorado Pálido", Color.FromArgb(255, 0xEE, 0xE8, 0xAA)),
            new("Caqui", Color.FromArgb(255, 0xF0, 0xE6, 0x8C)),
            new("Caqui Oscuro", Color.FromArgb(255, 0xBD, 0xB7, 0x6B)),
            new("Mostaza", Color.FromArgb(255, 0xFF, 0xDB, 0x58)),

            // Verdes
            new("Verde Amarillento", Color.FromArgb(255, 0xAD, 0xFF, 0x2F)),
            new("Verde Chartreuse", Color.FromArgb(255, 0x7F, 0xFF, 0x00)),
            new("Verde Césped", Color.FromArgb(255, 0x7C, 0xFC, 0x00)),
            new("Lima", Color.FromArgb(255, 0x00, 0xFF, 0x00)),
            new("Verde Lima", Color.FromArgb(255, 0x32, 0xCD, 0x32)),
            new("Verde Pálido", Color.FromArgb(255, 0x98, 0xFB, 0x98)),
            new("Verde Claro", Color.FromArgb(255, 0x90, 0xEE, 0x90)),
            new("Verde Mar Medio", Color.FromArgb(255, 0x3C, 0xB3, 0x71)),
            new("Verde Mar", Color.FromArgb(255, 0x2E, 0x8B, 0x57)),
            new("Verde Bosque", Color.FromArgb(255, 0x22, 0x8B, 0x22)),
            new("Verde", Color.FromArgb(255, 0x00, 0x80, 0x00)),
            new("Verde Oscuro", Color.FromArgb(255, 0x00, 0x64, 0x00)),
            new("Verde Oliva", Color.FromArgb(255, 0x80, 0x80, 0x00)),
            new("Oliva Oscuro", Color.FromArgb(255, 0x55, 0x6B, 0x2F)),
            new("Aguamarina", Color.FromArgb(255, 0x7F, 0xFF, 0xD4)),
            new("Menta", Color.FromArgb(255, 0x3E, 0xB4, 0x89)),
            new("Jade", Color.FromArgb(255, 0x00, 0xA8, 0x6B)),
            new("Esmeralda", Color.FromArgb(255, 0x50, 0xC8, 0x78)),
            new("Celadón", Color.FromArgb(255, 0xAC, 0xE1, 0xAF)),

            // Cianes y Azules
            new("Cian", Color.FromArgb(255, 0x00, 0xFF, 0xFF)),
            new("Cian Claro", Color.FromArgb(255, 0xE0, 0xFF, 0xFF)),
            new("Turquesa Pálido", Color.FromArgb(255, 0xAF, 0xEE, 0xEE)),
            new("Turquesa", Color.FromArgb(255, 0x40, 0xE0, 0xD0)),
            new("Turquesa Medio", Color.FromArgb(255, 0x48, 0xD1, 0xCC)),
            new("Turquesa Oscuro", Color.FromArgb(255, 0x00, 0xCE, 0xD1)),
            new("Azul Cadete", Color.FromArgb(255, 0x5F, 0x9E, 0xA0)),
            new("Acero Claro", Color.FromArgb(255, 0xB0, 0xC4, 0xDE)),
            new("Azul Acero", Color.FromArgb(255, 0x46, 0x82, 0xB4)),
            new("Azul Polvo", Color.FromArgb(255, 0xB0, 0xE0, 0xE6)),
            new("Azul Claro", Color.FromArgb(255, 0xAD, 0xD8, 0xE6)),
            new("Azul Cielo", Color.FromArgb(255, 0x87, 0xCE, 0xEB)),
            new("Azul Cielo Claro", Color.FromArgb(255, 0x87, 0xCE, 0xFA)),
            new("Azul Cielo Profundo", Color.FromArgb(255, 0x00, 0xBF, 0xFF)),
            new("Azul Dodger", Color.FromArgb(255, 0x1E, 0x90, 0xFF)),
            new("Azul Aciano", Color.FromArgb(255, 0x64, 0x95, 0xED)),
            new("Azul Real", Color.FromArgb(255, 0x41, 0x69, 0xE1)),
            new("Azul", Color.FromArgb(255, 0x00, 0x00, 0xFF)),
            new("Azul Medio", Color.FromArgb(255, 0x00, 0x00, 0xCD)),
            new("Azul Oscuro", Color.FromArgb(255, 0x00, 0x00, 0x8B)),
            new("Azul Marino", Color.FromArgb(255, 0x00, 0x00, 0x80)),
            new("Azul Medianoche", Color.FromArgb(255, 0x19, 0x19, 0x70)),
            new("Zafiro", Color.FromArgb(255, 0x0F, 0x52, 0xBA)),

            // Púrpuras, Violetas y Magentas
            new("Lavanda", Color.FromArgb(255, 0xE6, 0xE6, 0xFA)),
            new("Cardo", Color.FromArgb(255, 0xD8, 0xBF, 0xD8)),
            new("Ciruela", Color.FromArgb(255, 0xDD, 0xA0, 0xDD)),
            new("Violeta", Color.FromArgb(255, 0xEE, 0x82, 0xEE)),
            new("Orquídea", Color.FromArgb(255, 0xDA, 0x70, 0xD6)),
            new("Fucsia", Color.FromArgb(255, 0xFF, 0x00, 0xFF)),
            new("Magenta", Color.FromArgb(255, 0xFF, 0x00, 0xFF)),
            new("Orquídea Medio", Color.FromArgb(255, 0xBA, 0x55, 0xD3)),
            new("Púrpura Medio", Color.FromArgb(255, 0x93, 0x70, 0xDB)),
            new("Amatista", Color.FromArgb(255, 0x99, 0x66, 0xCC)),
            new("Violeta Azulado", Color.FromArgb(255, 0x8A, 0x2B, 0xE2)),
            new("Púrpura Oscuro", Color.FromArgb(255, 0x94, 0x00, 0xD3)),
            new("Orquídea Oscuro", Color.FromArgb(255, 0x99, 0x32, 0xCC)),
            new("Magenta Oscuro", Color.FromArgb(255, 0x8B, 0x00, 0x8B)),
            new("Púrpura", Color.FromArgb(255, 0x80, 0x00, 0x80)),
            new("Índigo", Color.FromArgb(255, 0x4B, 0x00, 0x82)),
            new("Azul Pizarra", Color.FromArgb(255, 0x6A, 0x5A, 0xCD)),
            new("Azul Pizarra Oscuro", Color.FromArgb(255, 0x48, 0x3D, 0x8B)),
            new("Lila", Color.FromArgb(255, 0xC8, 0xA2, 0xC8)),

            // Blancos y Beiges
            new("Blanco", Color.FromArgb(255, 0xFF, 0xFF, 0xFF)),
            new("Blanco Nieve", Color.FromArgb(255, 0xFF, 0xFA, 0xFA)),
            new("Blanco Miel", Color.FromArgb(255, 0xF0, 0xFF, 0xF0)),
            new("Menta Crema", Color.FromArgb(255, 0xF5, 0xFF, 0xFA)),
            new("Blanco Celeste", Color.FromArgb(255, 0xF0, 0xFF, 0xFF)),
            new("Azul Alicia", Color.FromArgb(255, 0xF0, 0xF8, 0xFF)),
            new("Blanco Fantasma", Color.FromArgb(255, 0xF8, 0xF8, 0xFF)),
            new("Concha", Color.FromArgb(255, 0xFF, 0xF5, 0xEE)),
            new("Beige", Color.FromArgb(255, 0xF5, 0xF5, 0xDC)),
            new("Encaje Antiguo", Color.FromArgb(255, 0xFD, 0xF5, 0xE6)),
            new("Marfil", Color.FromArgb(255, 0xFF, 0xFF, 0xF0)),
            new("Almendra", Color.FromArgb(255, 0xFF, 0xEB, 0xCD)),
            new("Crema", Color.FromArgb(255, 0xFF, 0xFD, 0xD0)),

            // Marrones y Grises
            new("Marrón", Color.FromArgb(255, 0xA5, 0x2A, 0x2A)),
            new("Siena", Color.FromArgb(255, 0xA0, 0x52, 0x2D)),
            new("SaddleBrown", Color.FromArgb(255, 0x8B, 0x45, 0x13)),
            new("Ocre", Color.FromArgb(255, 0xCC, 0x77, 0x22)),
            new("Chocolate", Color.FromArgb(255, 0xD2, 0x69, 0x1E)),
            new("Perú", Color.FromArgb(255, 0xCD, 0x85, 0x3F)),
            new("Siena Tostada", Color.FromArgb(255, 0xD2, 0xB4, 0x8C)),
            new("Terracota", Color.FromArgb(255, 0xE2, 0x72, 0x5B)),
            new("Sepia", Color.FromArgb(255, 0x70, 0x42, 0x14)),
            new("Gris Pizarra", Color.FromArgb(255, 0x70, 0x80, 0x90)),
            new("Gris Pizarra Claro", Color.FromArgb(255, 0x77, 0x88, 0x99)),
            new("Gris Tenue", Color.FromArgb(255, 0x69, 0x69, 0x69)),
            new("Gris", Color.FromArgb(255, 0x80, 0x80, 0x80)),
            new("Gris Oscuro", Color.FromArgb(255, 0xA9, 0xA9, 0xA9)),
            new("Plata", Color.FromArgb(255, 0xC0, 0xC0, 0xC0)),
            new("Gris Claro", Color.FromArgb(255, 0xD3, 0xD3, 0xD3)),
            new("Carbón", Color.FromArgb(255, 0x36, 0x45, 0x4F)),
            new("Negro", Color.FromArgb(255, 0x00, 0x00, 0x00)),
        };
    }

    /// <summary>
    /// Calcula la distancia euclidiana al cuadrado entre dos colores en el espacio RGB.
    /// </summary>
    /// <param name="c1">El primer color.</param>
    /// <param name="c2">El segundo color.</param>
    /// <returns>La distancia al cuadrado entre los dos colores.</returns>
    /// <remarks>
    /// Se usa el valor al cuadrado para evitar la costosa operación de raíz cuadrada,
    /// ya que para encontrar el mínimo, solo se necesita comparar las distancias relativas.
    /// </remarks>
    private static double GetColorDistanceSquared(Color c1, Color c2)
    {
        int r = c1.R - c2.R;
        int g = c1.G - c2.G;
        int b = c1.B - c2.B;
        return r * r + g * g + b * b;
    }

    /// <summary>
    /// Encuentra el color con nombre más cercano a un color de destino dado.
    /// </summary>
    /// <param name="targetColor">El color para el que se busca el vecino más cercano.</param>
    /// <returns>El <see cref="NamedColor"/> más cercano de la lista predefinida.</returns>
    public static NamedColor FindClosestColor(Color targetColor)
    {
        NamedColor closestColor = _namedColors[0];
        double minDistance = double.MaxValue;

        foreach (var namedColor in _namedColors)
        {
            double distance = GetColorDistanceSquared(targetColor, namedColor.Color);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestColor = namedColor;
            }
        }

        return closestColor;
    }
}