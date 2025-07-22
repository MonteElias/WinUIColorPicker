// Archivo: WinUIColorPicker.Controls/ColorParser.cs
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona métodos para analizar una cadena de texto y convertirla a un Color.
/// Soporta formatos como HEX, RGB y RGBA.
/// </summary>
public static class ColorParser
{
    // Regex para #AARRGGBB, #RRGGBB, #ARGB, #RGB
    private static readonly Regex HexRegex = new(@"^#?([0-9a-fA-F]{3,8})$", RegexOptions.Compiled);

    // Regex para rgb(r, g, b) o rgba(r, g, b, a)
    private static readonly Regex RgbRegex = new(
        @"^rgba?\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*(?:,\s*([0-9\.]+)\s*)?\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Intenta analizar una cadena y convertirla en un Color.
    /// </summary>
    /// <param name="input">La cadena de texto a analizar.</param>
    /// <param name="color">El color resultante si el análisis tiene éxito.</param>
    /// <returns>True si el análisis fue exitoso, de lo contrario False.</returns>
    public static bool TryParse(string input, out Color color)
    {
        color = new Color();
        if (string.IsNullOrWhiteSpace(input)) return false;

        input = input.Trim();

        // Intento 1: Parsear como HEX
        var hexMatch = HexRegex.Match(input);
        if (hexMatch.Success)
        {
            var hex = hexMatch.Groups[1].Value;
            byte a, r, g, b;

            try
            {
                if (hex.Length == 8) // AARRGGBB
                {
                    a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                }
                else if (hex.Length == 6) // RRGGBB
                {
                    a = 255;
                    r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                }
                else if (hex.Length == 4) // ARGB
                {
                    a = byte.Parse(hex.Substring(0, 1) + hex.Substring(0, 1), NumberStyles.HexNumber);
                    r = byte.Parse(hex.Substring(1, 1) + hex.Substring(1, 1), NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(2, 1) + hex.Substring(2, 1), NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(3, 1) + hex.Substring(3, 1), NumberStyles.HexNumber);
                }
                else if (hex.Length == 3) // RGB
                {
                    a = 255;
                    r = byte.Parse(hex.Substring(0, 1) + hex.Substring(0, 1), NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(1, 1) + hex.Substring(1, 1), NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(2, 1) + hex.Substring(2, 1), NumberStyles.HexNumber);
                }
                else
                {
                    return false;
                }

                color = Color.FromArgb(a, r, g, b);
                return true;
            }
            catch { return false; }
        }

        // Intento 2: Parsear como RGB/RGBA
        var rgbMatch = RgbRegex.Match(input);
        if (rgbMatch.Success)
        {
            try
            {
                var r = byte.Parse(rgbMatch.Groups[1].Value);
                var g = byte.Parse(rgbMatch.Groups[2].Value);
                var b = byte.Parse(rgbMatch.Groups[3].Value);
                byte a = 255;

                if (rgbMatch.Groups[4].Success) // Si el grupo alfa existe
                {
                    var alphaValue = double.Parse(rgbMatch.Groups[4].Value, CultureInfo.InvariantCulture);
                    a = (byte)(Math.Clamp(alphaValue, 0.0, 1.0) * 255);
                }

                color = Color.FromArgb(a, r, g, b);
                return true;
            }
            catch { return false; }
        }

        return false;
    }
}