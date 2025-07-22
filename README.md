WinUI Color Picker Controls

Advertencia Este Proyecto fue realizado en gran parte gracias a la ayuda de la IA , por lo que puede contener errores o no funcionar como se espera. Si encuentras algún problema, por favor, abre un issue en el repositorio de GitHub.
Una biblioteca de controles de selección de color moderna, de alto rendimiento y completamente personalizable para WinUI 3 y .NET 8. Diseñada desde cero para ofrecer una experiencia de usuario excepcional y una integración flexible en cualquier proyecto.

![alt text](https://img.shields.io/badge/nuget-v1.0.0-blue.svg)
![alt text](https://img.shields.io/badge/license-MIT-green.svg)

✨ Características Principales
Arquitectura Modular: Controles independientes (ColorWheel, ColorPickerSlider, ColorPickerAccent) que se componen en un selector de color principal robusto.
Altamente Configurable: Activa o desactiva casi cualquier parte de la interfaz de usuario (rueda, paletas, barra de acentos, sliders individuales) a través de propiedades de dependencia para adaptar el control a tus necesidades exactas.

Rendimiento con Win2D: La rueda de color está renderizada con Win2D, asegurando una interacción fluida y sin retardos, incluso al redimensionar.

Múltiples Modos de Selección:

Rueda de Color: Selección visual e intuitiva de Tono y Saturación.

Paletas de Colores: Utiliza la paleta moderna FluentPalette incluida o inyecta la tuya propia implementando la interfaz IColorPalette.

Ajustes Detallados: Sliders y campos de entrada numérica para un control preciso sobre los canales RGBA y HSVA.

Experiencia de Usuario Pulida:

Tooltips Informativos: El ColorWheel muestra dinámicamente el nombre del color más cercano y los ColorPickerAccent muestran el valor HEX de cada tonalidad.

Feedback Instantáneo: El botón de "Copiar" cambia de ícono y muestra un tooltip de confirmación.

Animaciones Fluidas: Transiciones suaves en la barra de navegación y animaciones sutiles en los controles interactivos.

Fácil Integración: Utiliza el ColorPickerButton para añadir un selector de color completo en un Flyout con una sola línea de XAML.

🎨 Vistas del Control
Aquí puedes ver cómo luce la biblioteca en acción.
Selector de Color Principal (WinUIColorPicker)
El control principal con sus tres vistas: Rueda de Color, Paleta y Ajustes.

![img](https://i.imgur.com/3RVHiWk.gif)

Descripción: Vista principal con la rueda de color Win2D, el slider de Valor (luminosidad) a la izquierda y el de Alfa (transparencia) a la derecha.
![img](https://i.imgur.com/ur6rEqv.png)

Descripción: Vista de paleta mostrando la FluentPalette por defecto. Cada color muestra su nombre en un tooltip.
![img](https://i.imgur.com/AXwhdqR.png)

Descripción: Vista de ajustes con los sliders para los canales RGBA, el campo de texto para HEX y el selector de modelo de color.
Botón Selector de Color (ColorPickerButton)
El control ColorPickerButton mostrando el WinUIColorPicker dentro de un Flyout.
![img](https://i.imgur.com/NqWTrOu.png)

Descripción: El ColorPickerButton desplegando el selector de color completo al hacer clic.
🚀 Instalación
Puedes instalar esta biblioteca en tu proyecto a través del gestor de paquetes NuGet.

Package Manager Console:

Install-Package WinUIColorPicker.Controls

.NET CLI:

dotnet add package WinUIColorPicker.Controls

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
🛠️ Uso Básico
La forma más sencilla de empezar es usando el ColorPickerButton.
1. Añade el namespace de la biblioteca a tu página XAML:
Generated xml
xmlns:colorpickers="using:WinUIColorPicker.Controls"
2. Coloca el ColorPickerButton y enlaza su propiedad Color a una propiedad en tu ViewModel o code-behind.
Generated xml
<colorpickers:ColorPickerButton 
    Color="{x:Bind ViewModel.MyColorProperty, Mode=TwoWay}" />
3. Si prefieres usar el selector completo directamente, puedes insertarlo en tu layout:
Generated xml
<colorpickers:WinUIColorPicker
    Color="{x:Bind ViewModel.MyColorProperty, Mode=TwoWay}"
    HorizontalAlignment="Stretch"
    VerticalAlignment="Stretch" />
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
⚙️ Personalización y API
Puedes personalizar la apariencia y funcionalidad del selector a través de sus propiedades de dependencia.
Propiedades Principales
Propiedad	Tipo	Descripción
Color	Windows.UI.Color	El color actualmente seleccionado. Esta es la propiedad principal del control.
ColorString	string	La representación en cadena de texto del color (ej. #AARRGGBB).
Visibilidad de Vistas
Controla qué pestañas de navegación están disponibles para el usuario.
Propiedad	Tipo	Predeterminado	Descripción
IsWheelViewEnabled	bool	true	Muestra u oculta la vista de la rueda de color.
IsPaletteViewEnabled	bool	true	Muestra u oculta la vista de paletas.
IsSettingsViewEnabled	bool	true	Muestra u oculta la vista de ajustes detallados.
Visibilidad de Componentes
Controla la visibilidad de elementos específicos dentro de las vistas.
Propiedad	Tipo	Predeterminado	Descripción
IsAccentBarEnabled	bool	true	Muestra u oculta la barra inferior de tintas y sombras.
IsValueSliderEnabled	bool	true	Muestra u oculta el slider de Valor/Luminosidad junto a la rueda.
IsWheelAlphaSliderEnabled	bool	true	Muestra u oculta el slider de Alfa junto a la rueda.
IsSettingsAlphaChannelEnabled	bool	true	Muestra u oculta la fila completa del canal Alfa en la vista de Ajustes.
Paletas Personalizadas (IColorPalette)
La biblioteca es extensible. Puedes proporcionar tu propia paleta de colores creando una clase que implemente la interfaz IColorPalette.
1. Define tu paleta:

Generated csharp

using System.Collections.Generic;

using Windows.UI;

using WinUIColorPicker.Controls;

public class MyCorporatePalette : IColorPalette
{
    public IEnumerable<Color> Colors => new List<Color>
    {
        Color.FromArgb(255, 0, 83, 161),  // Azul Corporativo
        Color.FromArgb(255, 243, 112, 33), // Naranja Corporativo
        Color.FromArgb(255, 128, 189, 41), // Verde Corporativo
        Color.FromArgb(255, 118, 118, 118) // Gris Corporativo
    };
}

2. Asigna una instancia de tu paleta al control en XAML:
Primero, declara tu paleta como un recurso:

Generated xml
<Page.Resources>
    <local:MyCorporatePalette x:Key="CorporatePalette" />
</Page.Resources>

Luego, asigna el recurso a la propiedad Palette:

Generated xml
<colorpickers:WinUIColorPicker
    Color="{x:Bind ViewModel.SelectedColor, Mode=TwoWay}"
    Palette="{StaticResource CorporatePalette}" />

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
🏗️ Arquitectura Interna
La biblioteca está construida sobre una base sólida de componentes bien definidos.
Componentes Principales
WinUIColorPicker: El orquestador principal que une todas las partes.

ColorPickerButton: Una fachada conveniente que usa un DropDownButton para alojar al WinUIColorPicker en un Flyout.

SegmentedNavigationView: El control de navegación superior con animaciones fluidas para cambiar entre vistas.

ColorWheel: Control de alto rendimiento basado en Win2D para la selección de Tono/Saturación.

ColorPickerSlider: Un Slider personalizado que genera dinámicamente un fondo de degradado según el canal de color que modifica.

ColorPickerAccent: La barra inferior que genera 6 tonalidades (3 más claras, 3 más oscuras) a partir del color base.

ColorPreviewer: Un control simple que muestra un color sobre un fondo de tablero de ajedrez para visualizar la transparencia correctamente.
Clases de Ayuda y Conversores

ColorHelper: Clase estática con métodos de extensión para conversiones entre modelos de color (RGB, HSV, HSL, CMYK) y formatos de cadena.

ColorParser: Parsea cadenas de texto en formatos HEX, RGB y RGBA para convertirlas a un objeto Color.

NamedColors: Contiene una lista exhaustiva de colores con nombre y un método para encontrar el color más cercano a un valor dado.

Conversores: Un conjunto de IValueConverter para facilitar los enlaces de datos en XAML (BooleanToVisibilityConverter, HsvValueConverter, etc.).

🤝 Contribuciones
Las contribuciones son bienvenidas. Si deseas mejorar esta biblioteca, por favor, abre un "issue" para discutir tus ideas o envía un "pull request" con tus cambios.

📄 Licencia
Este proyecto está bajo la Licencia MIT. Consulta el archivo LICENSE.md para más detalles.
