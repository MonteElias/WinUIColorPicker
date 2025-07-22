// Archivo: WinUIColorPicker.Controls/ColorPreviewer.cs
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Un control personalizado interno que muestra una vista previa de un color.
/// Incluye automáticamente un fondo de tablero de ajedrez para visualizar correctamente la transparencia.
/// </summary>
/// <remarks>
/// Este es un control con plantilla, cuya apariencia se define en <c>ColorPreviewer.xaml</c>.
/// </remarks>
internal sealed partial class ColorPreviewer : Control
{
    /// <summary>
    /// Obtiene o establece el color que se muestra en el previsualizador.
    /// </summary>
    /// <value>Un objeto <see cref="Color"/>. El valor por defecto es <c>Colors.Transparent</c>.</value>
    public Color DisplayColor
    {
        get { return (Color)GetValue(DisplayColorProperty); }
        set { SetValue(DisplayColorProperty, value); }
    }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="DisplayColor"/>.
    /// </summary>
    public static readonly DependencyProperty DisplayColorProperty =
        DependencyProperty.Register(
            nameof(DisplayColor),
            typeof(Color),
            typeof(ColorPreviewer),
            new PropertyMetadata(Colors.Transparent));

    // =========================================================================================
    // === NUEVA PROPIEDAD AÑADIDA AQUÍ ===
    // =========================================================================================
    /// <summary>
    /// Obtiene o establece el tamaño de los cuadrados en el fondo de tablero de ajedrez.
    /// Esta propiedad permite que la plantilla o el contenedor del ColorPreviewer especifique un tamaño.
    /// </summary>
    public double CheckerboardSquareSize
    {
        get { return (double)GetValue(CheckerboardSquareSizeProperty); }
        set { SetValue(CheckerboardSquareSizeProperty, value); }
    }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="CheckerboardSquareSize"/>.
    /// </summary>
    public static readonly DependencyProperty CheckerboardSquareSizeProperty =
        DependencyProperty.Register(
            nameof(CheckerboardSquareSize),
            typeof(double),
            typeof(ColorPreviewer),
            new PropertyMetadata(10.0)); // El valor por defecto se mantiene en 10.0 para uso general.
    // =========================================================================================

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ColorPreviewer"/>.
    /// </summary>
    public ColorPreviewer()
    {
        this.DefaultStyleKey = typeof(ColorPreviewer);
    }
}