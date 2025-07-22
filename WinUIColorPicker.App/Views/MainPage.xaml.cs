using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI;
using WinUIColorPicker.Controls;

namespace WinUIColorPicker.App.Views;

public sealed partial class MainPage : Page, INotifyPropertyChanged
{
    #region INotifyPropertyChanged Implementation
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Cuando cualquier propiedad de configuración cambia, actualizamos ambos snippets de código XAML.
        if (propertyName != nameof(GeneralControlCode) && propertyName != nameof(DropDownButtonCode))
        {
            OnPropertyChanged(nameof(GeneralControlCode));
            OnPropertyChanged(nameof(DropDownButtonCode));
        }
    }
    #endregion

    #region ViewModel Properties

    private Color _selectedColor;
    public Color SelectedColor
    {
        get => _selectedColor;
        set { if (_selectedColor != value) { _selectedColor = value; OnPropertyChanged(); } }
    }

    // Propiedades para los ToggleSwitch
    private bool _showWheelView = true;
    public bool ShowWheelView
    {
        get => _showWheelView;
        set { if (_showWheelView != value) { _showWheelView = value; OnPropertyChanged(); } }
    }
    private bool _showPaletteView = true;
    public bool ShowPaletteView
    {
        get => _showPaletteView;
        set { if (_showPaletteView != value) { _showPaletteView = value; OnPropertyChanged(); } }
    }
    private bool _showSettingsView = true;
    public bool ShowSettingsView
    {
        get => _showSettingsView;
        set { if (_showSettingsView != value) { _showSettingsView = value; OnPropertyChanged(); } }
    }
    private bool _showWheelAlphaSlider = true;
    public bool ShowWheelAlphaSlider
    {
        get => _showWheelAlphaSlider;
        set { if (_showWheelAlphaSlider != value) { _showWheelAlphaSlider = value; OnPropertyChanged(); } }
    }
    private bool _showSettingsAlphaChannel = true;
    public bool ShowSettingsAlphaChannel
    {
        get => _showSettingsAlphaChannel;
        set { if (_showSettingsAlphaChannel != value) { _showSettingsAlphaChannel = value; OnPropertyChanged(); } }
    }
    private bool _showValueSlider = true;
    public bool ShowValueSlider
    {
        get => _showValueSlider;
        set { if (_showValueSlider != value) { _showValueSlider = value; OnPropertyChanged(); } }
    }
    private bool _showAccentBar = true;
    public bool ShowAccentBar
    {
        get => _showAccentBar;
        set { if (_showAccentBar != value) { _showAccentBar = value; OnPropertyChanged(); } }
    }
    #endregion

    #region Code Snippet Properties

    public string GeneralControlCode =>
$@"<!-- Uso del control completo. -->
<controls:WinUIColorPicker 
    Color=""{{x:Bind SelectedColor, Mode=TwoWay}}""
    IsWheelViewEnabled=""{ShowWheelView}""
    IsPaletteViewEnabled=""{ShowPaletteView}""
    IsSettingsViewEnabled=""{ShowSettingsView}""
    IsWheelAlphaSliderEnabled=""{ShowWheelAlphaSlider}""
    IsSettingsAlphaChannelEnabled=""{ShowSettingsAlphaChannel}""
    IsValueSliderEnabled=""{ShowValueSlider}""
    IsAccentBarEnabled=""{ShowAccentBar}""/>";

    public string DropDownButtonCode =>
$@"<!-- Ideal para layouts compactos. -->
<controls:ColorPickerButton 
    Color=""{{x:Bind SelectedColor, Mode=TwoWay}}""
    IsWheelViewEnabled=""{ShowWheelView}""
    IsPaletteViewEnabled=""{ShowPaletteView}""
    IsSettingsViewEnabled=""{ShowSettingsView}""
    IsWheelAlphaSliderEnabled=""{ShowWheelAlphaSlider}""
    IsSettingsAlphaChannelEnabled=""{ShowSettingsAlphaChannel}""
    IsValueSliderEnabled=""{ShowValueSlider}""
    IsAccentBarEnabled=""{ShowAccentBar}""/>";

    public string CSharpViewModelCode { get; } =
@"// Tu página/ViewModel debe implementar
// INotifyPropertyChanged.

public class MyViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private Color _selectedColor;
    public Color SelectedColor
    {
        get => _selectedColor;
        set 
        {
            _selectedColor = value;
            OnPropertyChanged();
        }
    }

    // Propiedades booleanas para los
    // interruptores de configuración
    // para enlazar con x:Bind.
    public bool ShowWheelView { get; set; } = true;
    public bool ShowPaletteView { get; set; } = true;
    // etc...
}";

    #endregion

    #region Constructor & State Persistence
    public MainPage()
    {
        this.InitializeComponent();
        LoadLastColor();
    }

    private void LoadLastColor()
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        if (localSettings.Values.TryGetValue("LastSelectedColor", out object value) && value is string hexColor)
        {
            if (ColorParser.TryParse(hexColor, out Color lastColor))
            {
                SelectedColor = lastColor;
                return;
            }
        }
        SelectedColor = Color.FromArgb(255, 25, 118, 210);
    }

    public void SaveCurrentColor()
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["LastSelectedColor"] = SelectedColor.ToHex();
    }
    #endregion
}