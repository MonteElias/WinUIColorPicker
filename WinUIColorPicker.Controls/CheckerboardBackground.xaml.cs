// Archivo: WinUIColorPicker.Controls/CheckerboardBackground.xaml.cs
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Control de usuario que dibuja un fondo de tablero de ajedrez usando Win2D.
/// Este control es 'internal' porque está diseñado para ser una parte componente
/// de otros controles de la biblioteca, como el ColorPreviewer, y no para uso directo.
/// </summary>
public sealed partial class CheckerboardBackground : UserControl
{
    /// <summary>
    /// Pincel utilizado para dibujar el patrón de tablero de ajedrez.
    /// </summary>
    private ICanvasBrush? _checkerboardBrush;

    /// <summary>
    /// El creador de recursos de Win2D, necesario para crear objetos gráficos como pinceles.
    /// </summary>
    private ICanvasResourceCreator? _resourceCreator;

    // Define los dos colores utilizados para las casillas del tablero.
    /// <summary>
    /// El primer color utilizado para las casillas del tablero de ajedrez.
    /// </summary>
    private readonly Color _color1 = Colors.White;

    /// <summary>
    /// El segundo color utilizado para las casillas del tablero de ajedrez.
    /// </summary>
    private readonly Color _color2 = Color.FromArgb(255, 230, 230, 230); // Gris claro

    /// <summary>
    /// Obtiene o establece el tamaño en píxeles de cada cuadrado en el tablero de ajedrez.
    /// </summary>
    public double SquareSize
    {
        get { return (double)GetValue(SquareSizeProperty); }
        set { SetValue(SquareSizeProperty, value); }
    }

    /// <summary>
    /// Identifica la propiedad de dependencia <see cref="SquareSizeProperty"/>.
    /// </summary>
    public static readonly DependencyProperty SquareSizeProperty =
    DependencyProperty.Register(
      nameof(SquareSize),
      typeof(double),
      typeof(CheckerboardBackground),
      new PropertyMetadata(10.0, OnSquareSizeChanged)); // El valor por defecto es 10.0

    /// <summary>
    /// Se invoca cuando el valor de la propiedad de dependencia SquareSize cambia.
    /// </summary>
    /// <param name="d">El <see cref="DependencyObject"/> donde cambió la propiedad.</param>
    /// <param name="e">Datos del evento para la propiedad de dependencia.</param>
    private static void OnSquareSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CheckerboardBackground)d;
        // Cuando el tamaño cambia, es necesario recrear el pincel con las nuevas dimensiones
        // y solicitar un redibujado del control.
        if (control._resourceCreator != null)
        {
            control.CreateCheckerboardBrush(control._resourceCreator);
            control.FindCanvasControl()?.Invalidate(); // Invalidate fuerza la llamada al evento Draw.
        }
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CheckerboardBackground"/>.
    /// </summary>
    public CheckerboardBackground()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Método auxiliar para obtener la referencia al <see cref="CanvasControl"/> definido en el XAML.
    /// </summary>
    /// <returns>El <see cref="CanvasControl"/> asociado a este control de usuario, o null si no se encuentra.</returns>
    private CanvasControl? FindCanvasControl()
    {
        return this.Content as CanvasControl;
    }

    /// <summary>
    /// Manejador del evento CreateResources. Se llama cuando el control está listo para crear
    /// recursos gráficos que dependen del dispositivo (como pinceles e imágenes).
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="args">Datos del evento para recursos de creación de Canvas.</param>
    private void CanvasControl_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
    {
        _resourceCreator = sender; // Almacena el creador de recursos para uso futuro.
        args.TrackAsyncAction(CreateCheckerboardBrush(sender).AsAsyncAction());
    }

    /// <summary>
    /// Crea el pincel de tablero de ajedrez. Utiliza una lista de comandos para dibujar
    /// un patrón de 2x2 cuadrados, que luego se usa como imagen para el pincel.
    /// </summary>
    /// <param name="resourceCreator">El creador de recursos utilizado para crear el pincel y sus componentes.</param>
    /// <returns>Una <see cref="Task"/> que representa la operación asíncrona.</returns>
    private Task CreateCheckerboardBrush(ICanvasResourceCreator resourceCreator)
    {
        // Libera recursos del pincel anterior para evitar fugas de memoria.
        _checkerboardBrush?.Dispose();

        using var commandList = new CanvasCommandList(resourceCreator);
        var squareSizeFloat = (float)this.SquareSize;

        // Dibuja el patrón base 2x2 en una lista de comandos.
        using (var drawingSession = commandList.CreateDrawingSession())
        {
            drawingSession.FillRectangle(0, 0, squareSizeFloat, squareSizeFloat, _color1);
            drawingSession.FillRectangle(squareSizeFloat, squareSizeFloat, squareSizeFloat, squareSizeFloat, _color1);
            drawingSession.FillRectangle(0, squareSizeFloat, squareSizeFloat, squareSizeFloat, _color2);
            drawingSession.FillRectangle(squareSizeFloat, 0, squareSizeFloat, squareSizeFloat, _color2);
        }

        // Crea un CanvasImageBrush a partir de la lista de comandos.
        _checkerboardBrush = new CanvasImageBrush(resourceCreator, commandList)
        {
            // Configura el pincel para que repita (Wrap) el patrón en ambas direcciones.
            ExtendX = CanvasEdgeBehavior.Wrap,
            ExtendY = CanvasEdgeBehavior.Wrap,
            // Define el área de origen del pincel para que coincida con el patrón 2x2.
            SourceRectangle = new Windows.Foundation.Rect(0, 0, squareSizeFloat * 2, squareSizeFloat * 2)
        };

        return Task.CompletedTask;
    }

    /// <summary>
    /// Manejador del evento Draw. Se llama cada vez que el contenido del control necesita ser dibujado.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="args">Datos del evento para el dibujo de Canvas.</param>
    private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        if (_checkerboardBrush == null) return;

        // Rellena todo el rectángulo del control con el pincel de tablero de ajedrez.
        // El pincel se encarga de repetir el patrón automáticamente.
        args.DrawingSession.FillRectangle(new Windows.Foundation.Rect(0, 0, (float)sender.ActualWidth, (float)sender.ActualHeight), _checkerboardBrush);
    }
}