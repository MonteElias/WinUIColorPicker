// Archivo: WinUIColorPicker.Controls/ColorHelper.cs
using System;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona métodos de extensión para conversiones matemáticas entre diferentes
/// modelos de color (RGB, HSV, HSL, CMYK) y formatos de cadena.
/// </summary>
// CAMBIO: La clase ahora es 'public' para ser accesible desde otros proyectos,
// como la aplicación de demostración que necesita usar el método ToHex().
public static class ColorHelper
{
    #region HSV / HSL Conversions
    /// <summary>
    /// Convierte un color de RGBA a HSV.
    /// </summary>
    public static HsvColor ToHsv(this Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;
        double a = color.A / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double chroma = max - min;

        double h = 0;
        double s = 0;
        double v = max;

        if (chroma > 0)
        {
            if (max == r) h = (g - b) / chroma;
            else if (max == g) h = (b - r) / chroma + 2;
            else h = (r - g) / chroma + 4;

            h *= 60;
            if (h < 0) h += 360;

            if (max > 0) s = chroma / max;
        }

        return new HsvColor { H = h, S = s, V = v, A = a };
    }

    /// <summary>
    /// Convierte un color de RGBA a HSL.
    /// </summary>
    public static HslColor ToHsl(this Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;
        double a = color.A / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double h = 0, s = 0, l = (max + min) / 2.0;

        if (max != min)
        {
            double d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == r) h = (g - b) / d + (g < b ? 6 : 0);
            else if (max == g) h = (b - r) / d + 2;
            else h = (r - g) / d + 4;

            h /= 6;
        }

        return new HslColor { H = h * 360, S = s, L = l, A = a };
    }

    /// <summary>
    /// Convierte un color de HSV a RGBA.
    /// </summary>
    public static Color ToColor(this HsvColor hsv)
    {
        double r = 0, g = 0, b = 0;
        double h = hsv.H;
        double s = hsv.S;
        double v = hsv.V;

        int i = (int)Math.Floor(h / 60) % 6;
        double f = h / 60 - Math.Floor(h / 60);
        double p = v * (1 - s);
        double q = v * (1 - f * s);
        double t = v * (1 - (1 - f) * s);

        switch (i)
        {
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            case 5: r = v; g = p; b = q; break;
        }

        return Color.FromArgb((byte)(hsv.A * 255), (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }

    /// <summary>
    /// Convierte un color de HSL a RGBA.
    /// </summary>
    public static Color ToColor(this HslColor hsl)
    {
        double r, g, b;

        if (hsl.S == 0)
        {
            r = g = b = hsl.L; // Acromático
        }
        else
        {
            Func<double, double, double, double> hue2rgb = (p, q, t) =>
            {
                if (t < 0) t += 1;
                if (t > 1) t -= 1;
                if (t < 1.0 / 6) return p + (q - p) * 6 * t;
                if (t < 1.0 / 2) return q;
                if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
                return p;
            };

            var q = hsl.L < 0.5 ? hsl.L * (1 + hsl.S) : hsl.L + hsl.S - hsl.L * hsl.S;
            var p = 2 * hsl.L - q;
            r = hue2rgb(p, q, hsl.H / 360 + 1.0 / 3);
            g = hue2rgb(p, q, hsl.H / 360);
            b = hue2rgb(p, q, hsl.H / 360 - 1.0 / 3);
        }

        return Color.FromArgb((byte)(hsl.A * 255), (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }
    #endregion

    #region CMYK Conversions
    /// <summary>
    /// Convierte un color de RGBA a CMYK.
    /// </summary>
    public static CmykColor ToCmyk(this Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;
        double a = color.A / 255.0;

        double k = 1 - Math.Max(r, Math.Max(g, b));
        if (k == 1)
        {
            return new CmykColor { C = 0, M = 0, Y = 0, K = 1, A = a };
        }

        double c = (1 - r - k) / (1 - k);
        double m = (1 - g - k) / (1 - k);
        double y = (1 - b - k) / (1 - k);

        return new CmykColor { C = c, M = m, Y = y, K = k, A = a };
    }

    /// <summary>
    /// Convierte un color de CMYK a RGBA.
    /// </summary>
    public static Color ToColor(this CmykColor cmyk)
    {
        double r = 255 * (1 - cmyk.C) * (1 - cmyk.K);
        double g = 255 * (1 - cmyk.M) * (1 - cmyk.K);
        double b = 255 * (1 - cmyk.Y) * (1 - cmyk.K);

        return Color.FromArgb((byte)(cmyk.A * 255), (byte)r, (byte)g, (byte)b);
    }
    #endregion

    #region String Conversions
    /// <summary>
    /// Convierte un color a su representación hexadecimal de 8 dígitos (#AARRGGBB).
    /// </summary>
    public static string ToHex(this Color color) => $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";

    /// <summary>
    /// Convierte un color a su representación de cadena RGB, ej: "rgb(255, 100, 0)".
    /// </summary>
    public static string ToRgbString(this Color color) => $"rgb({color.R}, {color.G}, {color.B})";

    /// <summary>
    /// Convierte un color a su representación de cadena RGBA, ej: "rgba(255, 100, 0, 0.50)".
    /// </summary>
    public static string ToRgbaString(this Color color) => $"rgba({color.R}, {color.G}, {color.B}, {color.A / 255.0:F2})";
    #endregion
}