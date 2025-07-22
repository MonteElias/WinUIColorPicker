// Archivo: WinUIColorPicker.Controls/FluentPalette.cs
using System.Collections.Generic;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona una paleta de colores predefinida, vibrante y moderna.
/// </summary>
/// <remarks>
/// Esta paleta ha sido curada para ofrecer una amplia gama de colores distintos y útiles
/// para el diseño de interfaces de usuario, organizados por tono para una fácil selección visual.
/// Contiene 49 colores únicos (una cuadrícula de 7x7).
/// </remarks>
public class FluentPalette : IColorPalette // La interfaz IColorPalette también debería estar definida en tu proyecto
{
    /// <summary>
    /// Lista estática y de solo lectura que almacena la colección de colores de la paleta Fluent.
    /// </summary>
    private static readonly List<Color> _fluentColors;

    /// <summary>
    /// Constructor estático que inicializa la lista de colores <see cref="_fluentColors"/>.
    /// Se ejecuta una única vez, la primera vez que se accede a la clase <see cref="FluentPalette"/>.
    /// </summary>
    static FluentPalette()
    {
        _fluentColors = new List<Color>
    {
            // Fila 1: Rojos y Rosas
            Color.FromArgb(255, 239, 68, 68),   // Rojo
            Color.FromArgb(255, 244, 63, 94),   // Rosa
            Color.FromArgb(255, 236, 72, 153),  // Fucsia
            Color.FromArgb(255, 217, 70, 239),  // Magenta
            Color.FromArgb(255, 168, 85, 247),  // Púrpura
            Color.FromArgb(255, 139, 92, 246),  // Violeta
            Color.FromArgb(255, 99, 102, 241),  // Índigo

            // Fila 2: Azules
            Color.FromArgb(255, 75, 85, 99),    // Gris Azulado Oscuro
            Color.FromArgb(255, 59, 130, 246),  // Azul
            Color.FromArgb(255, 14, 165, 233),  // Azul Cielo
            Color.FromArgb(255, 6, 182, 212),   // Cian
            Color.FromArgb(255, 20, 184, 166),  // Verde Azulado (Teal)
            Color.FromArgb(255, 16, 185, 129),  // Esmeralda
            Color.FromArgb(255, 34, 197, 94),   // Verde

            // Fila 3: Verdes y Limas
            Color.FromArgb(255, 55, 65, 81),    // Gris Verdoso Oscuro
            Color.FromArgb(255, 107, 114, 128), // Gris Medio
            Color.FromArgb(255, 132, 204, 22),  // Lima
            Color.FromArgb(255, 234, 179, 8),   // Amarillo
            Color.FromArgb(255, 249, 115, 22),  // Ámbar
            Color.FromArgb(255, 245, 158, 11),  // Naranja
            Color.FromArgb(255, 234, 88, 12),   // Naranja Oscuro

            // Fila 4: Tonos de Marrón y Neutros Cálidos
            Color.FromArgb(255, 120, 113, 108), // Piedra
            Color.FromArgb(255, 168, 162, 158), // Neutro
            Color.FromArgb(255, 124, 58, 237),  // Amatista Intenso
            Color.FromArgb(255, 22, 163, 74),   // Verde Intenso
            Color.FromArgb(255, 202, 138, 4),   // Amarillo Intenso
            Color.FromArgb(255, 220, 38, 38),   // Rojo Intenso
            Color.FromArgb(255, 100, 116, 139), // Pizarra

            // Fila 5: Tonos Pastel Claros
            Color.FromArgb(255, 254, 202, 202), // Rojo Pastel
            Color.FromArgb(255, 254, 226, 226), // Rosa Pastel
            Color.FromArgb(255, 253, 230, 138), // Amarillo Pastel
            Color.FromArgb(255, 187, 247, 208), // Verde Pastel
            Color.FromArgb(255, 199, 210, 254), // Índigo Pastel
            Color.FromArgb(255, 191, 219, 254), // Azul Pastel
            Color.FromArgb(255, 224, 231, 255), // Lavanda Pastel

            // Fila 6: Tonos Medios Desaturados
            Color.FromArgb(255, 156, 163, 175), // Gris Frío
            Color.FromArgb(255, 229, 231, 235), // Gris Claro
            Color.FromArgb(255, 209, 213, 219), // Gris
            Color.FromArgb(255, 107, 114, 128), // Gris Oscuro
            Color.FromArgb(255, 55, 65, 81),    // Gris Muy Oscuro
            Color.FromArgb(255, 31, 41, 55),    // Gris Casi Negro
            Color.FromArgb(255, 17, 24, 39),    // Azul Medianoche

            // Fila 7: Colores Vibrantes Adicionales
            Color.FromArgb(255, 255, 255, 255), // Blanco
            Color.FromArgb(255, 220, 252, 231), // Menta
            Color.FromArgb(255, 254, 240, 138), // Limón
            Color.FromArgb(255, 253, 186, 116), // Melocotón
            Color.FromArgb(255, 252, 165, 165), // Salmón
            Color.FromArgb(255, 192, 132, 252), // Orquídea
            Color.FromArgb(255, 0, 0, 0)        // Negro
        };
    }

    /// <summary>
    /// Obtiene una colección de los colores que componen esta paleta.
    /// </summary>
    /// <value>Una colección de objetos <see cref="Color"/> que representa los colores de la paleta.</value>
    public IEnumerable<Color> Colors => _fluentColors;
}
