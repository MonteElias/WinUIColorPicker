// Archivo: WinUIColorPicker.Controls/ColorPickerSlider.cs
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Foundation;
using Windows.UI;

namespace WinUIColorPicker.Controls;

/// <summary>
/// Un control Slider interno y altamente personalizado para editar un único canal de color.
/// El fondo del slider se actualiza dinámicamente para mostrar un degradado
/// relevante para el canal de color que se está editando.
/// </summary>
internal sealed partial class ColorPickerSlider : Slider
{
    private Thumb? _thumb;
    private Border? _colorTrack;
    private Grid? _alphaTrackGrid;
    private Border? _alphaGradientBorder;
    private Grid? _rootGrid;
    private double _lastKnownLength = 0;

    #region Dependency Properties
    public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register(nameof(Channel), typeof(ColorChannel), typeof(ColorPickerSlider), new PropertyMetadata(ColorChannel.Hue, OnChannelOrColorChanged));
    public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(nameof(CurrentColor), typeof(Color), typeof(ColorPickerSlider), new PropertyMetadata(Colors.Red, OnChannelOrColorChanged));

    /// <summary>
    /// Obtiene o establece el canal de color que este slider controla.
    /// </summary>
    public ColorChannel Channel { get => (ColorChannel)GetValue(ChannelProperty); set => SetValue(ChannelProperty, value); }

    /// <summary>
    /// Obtiene o establece el color actual. Se utiliza para generar los degradados
    /// para los canales que dependen de otros componentes del color (ej. Saturación).
    /// </summary>
    public Color CurrentColor { get => (Color)GetValue(CurrentColorProperty); set => SetValue(CurrentColorProperty, value); }
    #endregion

    public ColorPickerSlider()
    {
        this.DefaultStyleKey = typeof(ColorPickerSlider);
        this.ValueChanged += (s, e) => UpdateThumbPosition();
        this.SizeChanged += OnSliderSizeChanged;
    }

    protected override void OnApplyTemplate()
    {
        if (_thumb != null) { _thumb.DragDelta -= Thumb_DragDelta; }
        if (_colorTrack != null) { _colorTrack.PointerPressed -= Track_PointerPressed; }
        if (_alphaTrackGrid != null) { _alphaTrackGrid.PointerPressed -= Track_PointerPressed; }

        base.OnApplyTemplate();

        _thumb = GetTemplateChild("Thumb") as Thumb;
        _colorTrack = GetTemplateChild("ColorTrack") as Border;
        _alphaTrackGrid = GetTemplateChild("AlphaTrackGrid") as Grid;
        _alphaGradientBorder = GetTemplateChild("AlphaGradientBorder") as Border;
        _rootGrid = GetTemplateChild("RootGrid") as Grid;

        if (_thumb != null) { _thumb.DragDelta += Thumb_DragDelta; }
        if (_colorTrack != null) { _colorTrack.PointerPressed += Track_PointerPressed; }
        if (_alphaTrackGrid != null) { _alphaTrackGrid.PointerPressed += Track_PointerPressed; }

        OnChannelOrColorChanged();
    }

    private double GetSanitizedValue(double newValue)
    {
        switch (this.Channel)
        {
            case ColorChannel.Red:
            case ColorChannel.Green:
            case ColorChannel.Blue:
            case ColorChannel.Hue:
                // Para estos canales, siempre queremos un valor entero.
                return Math.Round(newValue);
            default:
                // Para Saturación, Valor, Alfa (0-1), mantenemos los decimales.
                return newValue;
        }
    }

    private void Track_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not FrameworkElement trackElement) return;
        var position = e.GetCurrentPoint(trackElement).Position;
        double ratio = (Orientation == Orientation.Horizontal)
            ? Math.Clamp(position.X / trackElement.ActualWidth, 0.0, 1.0)
            : 1 - Math.Clamp(position.Y / trackElement.ActualHeight, 0.0, 1.0);

        var newValue = Minimum + (ratio * (Maximum - Minimum));
        Value = GetSanitizedValue(Math.Clamp(newValue, Minimum, Maximum));
    }

    private void OnSliderSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateLayout();
        UpdateThumbPosition();
    }

    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (_rootGrid == null) return;
        _lastKnownLength = (Orientation == Orientation.Horizontal) ? _rootGrid.ActualWidth : _rootGrid.ActualHeight;
        if (_lastKnownLength <= 0) return;

        var pixelChange = (Orientation == Orientation.Horizontal) ? e.HorizontalChange : -e.VerticalChange;
        double valueChange = (pixelChange / _lastKnownLength) * (Maximum - Minimum);
        Value = GetSanitizedValue(Math.Clamp(Value + valueChange, Minimum, Maximum));
    }

    private new void UpdateLayout()
    {
        if (_colorTrack == null || _alphaTrackGrid == null) return;
        bool isHorizontal = Orientation == Orientation.Horizontal;
        var trackHeight = isHorizontal ? 10.0 : double.NaN;
        var trackWidth = isHorizontal ? double.NaN : 10.0;
        var verticalAlign = isHorizontal ? VerticalAlignment.Center : VerticalAlignment.Stretch;
        var horizontalAlign = isHorizontal ? HorizontalAlignment.Stretch : HorizontalAlignment.Center;
        _colorTrack.Height = trackHeight;
        _colorTrack.Width = trackWidth;
        _colorTrack.VerticalAlignment = verticalAlign;
        _colorTrack.HorizontalAlignment = horizontalAlign;
        _alphaTrackGrid.Height = trackHeight;
        _alphaTrackGrid.Width = trackWidth;
        _alphaTrackGrid.VerticalAlignment = verticalAlign;
        _alphaTrackGrid.HorizontalAlignment = horizontalAlign;
    }

    private void UpdateThumbPosition()
    {
        if (_thumb == null || _rootGrid == null || Maximum <= Minimum) return;
        _lastKnownLength = (Orientation == Orientation.Horizontal) ? _rootGrid.ActualWidth : _rootGrid.ActualHeight;
        if (_lastKnownLength <= 0) return;

        double valueRatio = (Value - Minimum) / (Maximum - Minimum);
        if (_thumb.RenderTransform is not TranslateTransform transform)
        {
            transform = new TranslateTransform();
            _thumb.RenderTransform = transform;
        }

        if (Orientation == Orientation.Horizontal)
        {
            transform.X = valueRatio * _lastKnownLength - (_thumb.ActualWidth / 2);
            transform.Y = 0;
            _thumb.VerticalAlignment = VerticalAlignment.Center;
            _thumb.HorizontalAlignment = HorizontalAlignment.Left;
        }
        else
        {
            transform.Y = (1 - valueRatio) * _lastKnownLength - (_thumb.ActualHeight / 2);
            transform.X = 0;
            _thumb.VerticalAlignment = VerticalAlignment.Top;
            _thumb.HorizontalAlignment = HorizontalAlignment.Center;
        }
    }

    private static void OnChannelOrColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorPickerSlider slider)
        {
            slider.OnChannelOrColorChanged();
        }
    }

    private void OnChannelOrColorChanged()
    {
        UpdateLayout();
        UpdateSliderProperties();
        UpdateSliderBackground();
        UpdateThumbPosition();
    }

    private void UpdateSliderProperties()
    {
        switch (Channel)
        {
            case ColorChannel.Hue: Minimum = 0; Maximum = 359.99; StepFrequency = 0.01; break;
            case ColorChannel.Saturation:
            case ColorChannel.Value:
            case ColorChannel.Alpha: Minimum = 0; Maximum = 1.0; StepFrequency = 0.01; break;
            case ColorChannel.Red:
            case ColorChannel.Green:
            case ColorChannel.Blue: Minimum = 0; Maximum = 255; StepFrequency = 1; break;
        }
    }

    private void UpdateSliderBackground()
    {
        if (_colorTrack == null || _alphaTrackGrid == null || _alphaGradientBorder == null) return;
        if (Channel == ColorChannel.Alpha)
        {
            _colorTrack.Visibility = Visibility.Collapsed;
            _alphaTrackGrid.Visibility = Visibility.Visible;
            var alphaBrush = new LinearGradientBrush();
            if (Orientation == Orientation.Horizontal) { alphaBrush.StartPoint = new Point(0, 0.5); alphaBrush.EndPoint = new Point(1, 0.5); }
            else { alphaBrush.StartPoint = new Point(0.5, 1); alphaBrush.EndPoint = new Point(0.5, 0); }
            var colorWithoutAlpha = Color.FromArgb(255, CurrentColor.R, CurrentColor.G, CurrentColor.B);
            alphaBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 0.0 });
            alphaBrush.GradientStops.Add(new GradientStop { Color = colorWithoutAlpha, Offset = 1.0 });
            _alphaGradientBorder.Background = alphaBrush;
            return;
        }
        _colorTrack.Visibility = Visibility.Visible;
        _alphaTrackGrid.Visibility = Visibility.Collapsed;
        var hsv = CurrentColor.ToHsv();
        var brush = new LinearGradientBrush();
        if (Orientation == Orientation.Horizontal) { brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5); }
        else { brush.StartPoint = new Point(0.5, 1); brush.EndPoint = new Point(0.5, 0); }
        switch (Channel)
        {
            case ColorChannel.Hue: brush.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 0.167 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Green, Offset = 0.333 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Cyan, Offset = 0.5 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Blue, Offset = 0.667 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Magenta, Offset = 0.833 }); brush.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 1.0 }); break;
            case ColorChannel.Value: brush.GradientStops.Add(new GradientStop { Color = new HsvColor { H = hsv.H, S = hsv.S, V = 0, A = 1 }.ToColor(), Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = new HsvColor { H = hsv.H, S = hsv.S, V = 1, A = 1 }.ToColor(), Offset = 1.0 }); break;
            case ColorChannel.Saturation: brush.GradientStops.Add(new GradientStop { Color = new HsvColor { H = hsv.H, S = 0, V = hsv.V, A = 1 }.ToColor(), Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = new HsvColor { H = hsv.H, S = 1, V = hsv.V, A = 1 }.ToColor(), Offset = 1.0 }); break;
            case ColorChannel.Red: brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, 0, CurrentColor.G, CurrentColor.B), Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, 255, CurrentColor.G, CurrentColor.B), Offset = 1.0 }); break;
            case ColorChannel.Green: brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, CurrentColor.R, 0, CurrentColor.B), Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, CurrentColor.R, 255, CurrentColor.B), Offset = 1.0 }); break;
            case ColorChannel.Blue: brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, CurrentColor.R, CurrentColor.G, 0), Offset = 0.0 }); brush.GradientStops.Add(new GradientStop { Color = Color.FromArgb(255, CurrentColor.R, CurrentColor.G, 255), Offset = 1.0 }); break;
        }
        this.Background = brush;
    }
}