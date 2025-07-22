using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace WinUIColorPicker.App;

public sealed partial class CodeSample : UserControl
{
    // Propiedad de dependencia para pasar el fragmento de c�digo al control.
    public string Code
    {
        get { return (string)GetValue(CodeProperty); }
        set { SetValue(CodeProperty, value); }
    }

    public static readonly DependencyProperty CodeProperty =
        DependencyProperty.Register(nameof(Code), typeof(string), typeof(CodeSample), new PropertyMetadata(string.Empty, OnCodeChanged));

    private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CodeSample)d;
        control.CodeTextBlock.Text = e.NewValue as string ?? string.Empty;
    }

    public CodeSample()
    {
        this.InitializeComponent();
    }

    // L�gica para el bot�n de copiar.
    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        var dataPackage = new DataPackage();
        dataPackage.SetText(this.Code);
        try
        {
            Clipboard.SetContent(dataPackage);
        }
        catch (Exception)
        {
            // Opcional: Mostrar un error si el portapapeles no est� disponible.
        }
    }
}