// Archivo: WinUIColorPicker.Controls/ColorWheel.cs
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Representa un control de rueda de color para seleccionar el Tono y la Saturación.
/// </summary>
[TemplatePart(Name = "WheelCanvas", Type = typeof(Canvas))]
[TemplatePart(Name = "WheelThumb", Type = typeof(Thumb))]
[TemplatePart(Name = "PART_CanvasControl", Type = typeof(CanvasControl))]
[TemplatePart(Name = "ColorNamePopup", Type = typeof(Popup))]
[TemplatePart(Name = "ColorNameTextBlock", Type = typeof(TextBlock))]
internal sealed partial class ColorWheel : Control
{
    private Canvas? _wheelCanvas;
    private Thumb? _wheelThumb;
    private CanvasControl? _canvasControl;
    private CanvasBitmap? _wheelBitmap;
    private bool _isPointerPressed;

    private Popup? _colorNamePopup;
    private TextBlock? _colorNameTextBlock;

    public double Hue { get => (double)GetValue(HueProperty); set => SetValue(HueProperty, value); }
    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(ColorWheel), new PropertyMetadata(0.0, OnColorChanged));

    public double Saturation { get => (double)GetValue(SaturationProperty); set => SetValue(SaturationProperty, value); }
    public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorWheel), new PropertyMetadata(1.0, OnColorChanged));

    public ColorWheel()
    {
        DefaultStyleKey = typeof(ColorWheel);
    }

    protected override void OnApplyTemplate()
    {
        if (_wheelThumb != null)
        {
            _wheelThumb.DragDelta -= Thumb_DragDelta;
            _wheelThumb.DragCompleted -= Thumb_DragCompleted;
        }
        if (_canvasControl != null)
        {
            _canvasControl.PointerPressed -= Canvas_PointerPressed;
            _canvasControl.PointerMoved -= Canvas_PointerMoved;
            _canvasControl.PointerReleased -= Canvas_PointerReleased;
            _canvasControl.CreateResources -= Canvas_CreateResources;
            _canvasControl.Draw -= Canvas_Draw;
            _canvasControl.SizeChanged -= OnCanvasSizeChanged;
        }

        base.OnApplyTemplate();

        _wheelCanvas = GetTemplateChild("WheelCanvas") as Canvas;
        _wheelThumb = GetTemplateChild("WheelThumb") as Thumb;
        _canvasControl = GetTemplateChild("PART_CanvasControl") as CanvasControl;
        _colorNamePopup = GetTemplateChild("ColorNamePopup") as Popup;
        _colorNameTextBlock = GetTemplateChild("ColorNameTextBlock") as TextBlock;

        if (_wheelThumb != null)
        {
            _wheelThumb.DragDelta += Thumb_DragDelta;
            _wheelThumb.DragCompleted += Thumb_DragCompleted;
        }
        if (_canvasControl != null)
        {
            _canvasControl.PointerPressed += Canvas_PointerPressed;
            _canvasControl.PointerMoved += Canvas_PointerMoved;
            _canvasControl.PointerReleased += Canvas_PointerReleased;
            _canvasControl.CreateResources += Canvas_CreateResources;
            _canvasControl.Draw += Canvas_Draw;
            _canvasControl.SizeChanged += OnCanvasSizeChanged;
        }
    }

    private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        if (_colorNamePopup != null)
        {
            _colorNamePopup.IsOpen = false;
        }
    }

    private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e) => UpdateThumbPosition();
    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ColorWheel)?.UpdateThumbPosition();
    private void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args) => args.TrackAsyncAction(CreateWheelBitmap(sender).AsAsyncAction());

    private Task CreateWheelBitmap(ICanvasResourceCreator resourceCreator)
    {
        _wheelBitmap?.Dispose();
        var size = new Size(512, 512); // Renderizamos en una textura de alta resolución
        var renderTarget = new CanvasRenderTarget(resourceCreator, (float)size.Width, (float)size.Height, 96);
        using (var ds = renderTarget.CreateDrawingSession())
        {
            ds.Clear(Colors.Transparent);
            var center = new Vector2((float)size.Width / 2, (float)size.Height / 2);
            var radius = center.X;
            // Dibuja el anillo de tonos puros
            for (int i = 0; i < 360; i++)
            {
                var color = new HsvColor(i, 1, 1, 1).ToColor();
                var angle1 = (float)(i * Math.PI / 180.0);
                var angle2 = (float)((i + 1.1) * Math.PI / 180.0);
                var pathBuilder = new CanvasPathBuilder(resourceCreator);
                pathBuilder.BeginFigure(center);
                pathBuilder.AddLine(center + new Vector2((float)(radius * Math.Cos(angle1)), (float)(radius * Math.Sin(angle1))));
                pathBuilder.AddArc(center + new Vector2((float)(radius * Math.Cos(angle2)), (float)(radius * Math.Sin(angle2))), radius, radius, 0, CanvasSweepDirection.Clockwise, CanvasArcSize.Small);
                pathBuilder.EndFigure(CanvasFigureLoop.Closed);
                using var pieSliceGeometry = CanvasGeometry.CreatePath(pathBuilder);
                ds.FillGeometry(pieSliceGeometry, color);
            }
            // Superpone el degradado de saturación (de blanco en el centro a transparente en el borde)
            using var radialBrush = new CanvasRadialGradientBrush(resourceCreator, Colors.White, Color.FromArgb(0, 255, 255, 255)) { Center = center, RadiusX = radius, RadiusY = radius };
            ds.FillCircle(center, radius, radialBrush);
        }
        _wheelBitmap = renderTarget;
        return Task.CompletedTask;
    }

    private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        if (_wheelBitmap == null) return;
        try
        {
            // Dibuja el bitmap pre-renderizado en el canvas, escalándolo al tamaño actual del control.
            args.DrawingSession.DrawImage(_wheelBitmap, new Rect(0, 0, sender.Size.Width, sender.Size.Height));
        }
        catch (Exception e) when (sender.Device.IsDeviceLost(e.HResult))
        {
            // Si se pierde el dispositivo gráfico, invalida el control para que vuelva a crear los recursos.
            sender.Invalidate();
        }
    }

    private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement element || _canvasControl is null) return;
        _isPointerPressed = true;
        element.CapturePointer(e.Pointer);
        UpdateColorFromPoint(e.GetCurrentPoint(_canvasControl).Position);
    }

    private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (_isPointerPressed && _canvasControl is not null)
        {
            UpdateColorFromPoint(e.GetCurrentPoint(_canvasControl).Position);
        }
    }

    private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement element) return;
        _isPointerPressed = false;
        element.ReleasePointerCapture(e.Pointer);
        if (_colorNamePopup != null) _colorNamePopup.IsOpen = false;
    }

    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (_wheelThumb == null) return;
        var newX = Canvas.GetLeft(_wheelThumb) + e.HorizontalChange;
        var newY = Canvas.GetTop(_wheelThumb) + e.VerticalChange;
        UpdateColorFromPoint(new Point(newX, newY));
    }

    private void UpdateColorFromPoint(Point point)
    {
        if (_canvasControl == null || _canvasControl.ActualWidth == 0) return;
        var center = new Vector2((float)_canvasControl.ActualWidth / 2, (float)_canvasControl.ActualHeight / 2);
        var radius = center.X;
        var vector = new Vector2((float)point.X, (float)point.Y) - center;
        var distance = vector.Length();

        // La saturación es la distancia desde el centro, normalizada por el radio.
        var saturation = Math.Clamp(distance / radius, 0, 1);

        // El tono es el ángulo del vector.
        if (distance > 0.001) // Evitar división por cero o Atan2 indefinido en el centro
        {
            var angle = Math.Atan2(vector.Y, vector.X);
            var hue = angle * 180.0 / Math.PI;
            if (hue < 0) hue += 360;
            Hue = hue;
        }
        Saturation = saturation;

        // Mostrar el popup con el nombre del color más cercano
        var currentColor = new HsvColor(Hue, Saturation, 1.0, 1.0).ToColor();
        var closestNamedColor = NamedColors.FindClosestColor(currentColor); // Asume una clase helper 'NamedColors'
        if (_colorNamePopup != null && _colorNameTextBlock != null)
        {
            _colorNameTextBlock.Text = closestNamedColor.Name;
            var angleRad = Hue * Math.PI / 180.0;
            var thumbDistance = Saturation * radius;
            var thumbX = center.X + Math.Cos(angleRad) * thumbDistance;
            var thumbY = center.Y + Math.Sin(angleRad) * thumbDistance;
            _colorNamePopup.HorizontalOffset = thumbX;
            _colorNamePopup.VerticalOffset = thumbY - 35; // Posicionar arriba del thumb
            _colorNamePopup.IsOpen = true;
        }
    }

    private void UpdateThumbPosition()
    {
        if (_wheelThumb == null || _canvasControl == null || _canvasControl.ActualWidth == 0) return;
        var center = new Point(_canvasControl.ActualWidth / 2, _canvasControl.ActualHeight / 2);
        var radius = center.X;
        var angleRad = Hue * Math.PI / 180.0;
        var distance = Saturation * radius;
        // Convierte coordenadas polares (ángulo/distancia) a cartesianas (x/y) para posicionar el thumb.
        var x = center.X + Math.Cos(angleRad) * distance;
        var y = center.Y + Math.Sin(angleRad) * distance;
        Canvas.SetLeft(_wheelThumb, x);
        Canvas.SetTop(_wheelThumb, y);
    }

    /// <summary>
    /// Estructura interna para conversiones rápidas de HSV a Color.
    /// </summary>
    private struct HsvColor
    {
        public double H, S, V, A;
        public HsvColor(double h, double s, double v, double a) { H = h; S = s; V = v; A = a; }
        public Color ToColor()
        {
            double r, g, b;
            if (S == 0) { r = g = b = V; }
            else
            {
                double h = H >= 360 ? 0 : H / 60;
                int i = (int)h;
                double f = h - i;
                double p = V * (1 - S);
                double q = V * (1 - S * f);
                double t = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0: r = V; g = t; b = p; break;
                    case 1: r = q; g = V; b = p; break;
                    case 2: r = p; g = V; b = t; break;
                    case 3: r = p; g = q; b = V; break;
                    case 4: r = t; g = p; b = V; break;
                    default: r = V; g = p; b = q; break;
                }
            }
            return Color.FromArgb((byte)Math.Round(A * 255), (byte)Math.Round(r * 255), (byte)Math.Round(g * 255), (byte)Math.Round(b * 255));
        }
    }
}