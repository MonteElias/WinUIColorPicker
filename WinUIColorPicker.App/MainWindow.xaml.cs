using Microsoft.UI.Xaml;
using WinUIColorPicker.App.Views; // Aseg�rate de que este 'using' sea correcto

namespace WinUIColorPicker.App;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Title = "Demostraci�n de WinUI Color Picker";

        // Suscr�bete al evento 'Closed' de la ventana.
        this.Closed += OnWindowClosed;

        // Navega a nuestra p�gina principal.
        MyFrame.Navigate(typeof(MainPage));
    }

    /// <summary>
    /// Se dispara cuando la ventana est� a punto de cerrarse.
    /// </summary>
    private void OnWindowClosed(object sender, WindowEventArgs args)
    {
        // Obtiene la p�gina actual que se muestra en el Frame.
        var currentPage = MyFrame.Content as MainPage;

        // Comprueba si la p�gina es la MainPage y si no es nula.
        if (currentPage != null)
        {
            // Llama al m�todo p�blico de la p�gina para que guarde su estado.
            currentPage.SaveCurrentColor();
        }
    }
}