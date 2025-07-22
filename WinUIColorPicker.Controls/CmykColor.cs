// Archivo: WinUIColorPicker.Controls/CmykColor.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Representa un color en el espacio de color CMYK (Cyan, Magenta, Yellow, Key/Black).
/// Los valores están normalizados en un rango de 0.0 a 1.0.
/// </summary>
public struct CmykColor
{
    /// <summary>
    /// Obtiene o establece el componente Cian (Cyan). Rango: 0.0 a 1.0.
    /// </summary>
    public double C { get; set; }

    /// <summary>
    /// Obtiene o establece el componente Magenta. Rango: 0.0 a 1.0.
    /// </summary>
    public double M { get; set; }

    /// <summary>
    /// Obtiene o establece el componente Amarillo (Yellow). Rango: 0.0 a 1.0.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Obtiene o establece el componente Clave/Negro (Key/Black). Rango: 0.0 a 1.0.
    /// </summary>
    public double K { get; set; }

    /// <summary>
    /// Obtiene o establece el canal Alfa (transparencia). Rango: 0.0 (transparente) a 1.0 (opaco).
    /// </summary>
    public double A { get; set; }
}