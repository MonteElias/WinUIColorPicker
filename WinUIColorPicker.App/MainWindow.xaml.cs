using Microsoft.UI.Xaml;
using WinUIColorPicker.App.Views; // Asegúrate de que este 'using' sea correcto

namespace WinUIColorPicker.App;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Title = "Demostración de WinUI Color Picker";

        // Suscríbete al evento 'Closed' de la ventana.
        this.Closed += OnWindowClosed;

        // Navega a nuestra página principal.
        MyFrame.Navigate(typeof(MainPage));
    }

    /// <summary>
    /// Se dispara cuando la ventana está a punto de cerrarse.
    /// </summary>
    private void OnWindowClosed(object sender, WindowEventArgs args)
    {
        // Obtiene la página actual que se muestra en el Frame.
        var currentPage = MyFrame.Content as MainPage;

        // Comprueba si la página es la MainPage y si no es nula.
        if (currentPage != null)
        {
            // Llama al método público de la página para que guarde su estado.
            currentPage.SaveCurrentColor();
        }
    }
}