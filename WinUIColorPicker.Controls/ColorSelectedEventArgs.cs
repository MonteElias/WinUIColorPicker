// Archivo: WinUIColorPicker.Controls/ColorSelectedEventArgs.cs
using System;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Proporciona datos para eventos que notifican la selección de un color.
/// </summary>
/// <remarks>
/// Esta clase es interna al ensamblado de controles. Si un evento que la utiliza
/// se hace público, esta clase también debería hacerse pública.
/// </remarks>
internal class ColorSelectedEventArgs : EventArgs
{
    /// <summary>
    /// Obtiene el nuevo color que fue seleccionado por el usuario.
    /// </summary>
    public Color NewColor { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ColorSelectedEventArgs"/>.
    /// </summary>
    /// <param name="newColor">El color que fue seleccionado.</param>
    public ColorSelectedEventArgs(Color newColor)
    {
        NewColor = newColor;
    }
}