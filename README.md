# [H.ReactiveUI.CommonInteractions](https://github.com/HavenDV/H.ReactiveUI.CommonInteractions/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/H.ReactiveUI.CommonInteractions/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/H.ReactiveUI.CommonInteractions.svg?label=License&maxAge=86400)](LICENSE.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.0-blue.svg)](https://github.com/microsoft/dotnet/blob/master/releases/net40/README.md)
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.5-blue.svg)](https://github.com/microsoft/dotnet/blob/master/releases/net45/README.md)
[![Build Status](https://github.com/HavenDV/H.ReactiveUI.CommonInteractions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/HavenDV/H.ReactiveUI.CommonInteractions/actions/workflows/dotnet.yml)

Common ReactiveUI MVVM Level Interactions(like open/save file) for WPF/UWP/Uno platform.

### NuGet

[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Core.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Core)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Core/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Wpf.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Wpf)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Wpf/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Uno.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Uno)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Uno/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Uwp.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Uwp)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Uwp/)

```
Install-Package H.ReactiveUI.CommonInteractions.Core
Install-Package H.ReactiveUI.CommonInteractions.Wpf
Install-Package H.ReactiveUI.CommonInteractions.Uno
Install-Package H.ReactiveUI.CommonInteractions.Uwp
```

## Usage
Add to your App constructors:
```cs

public sealed partial class App
{
    private InteractionManager InteractionManager { get; } = new();

    public App()
    {
        InteractionManager.Register();

        // Not necessary. Displays unhandled exceptions using MessageInteractions.Exception.
        // InteractionManager.CatchUnhandledExceptions();

        // your code
    }
}
```

### FileInteractions
```cs
var data = await FileInteractions.OpenFile.Handle(new OpenFileArguments
{
    SuggestedFileName = "my.txt",
    Extensions = new[] { ".txt" },
    FilterName = "My txt files",
});
var text = data.Text;

var fileName = await FileInteractions.SaveFile.Handle(new SaveFileArguments
{
    SuggestedFileName = "my.txt",
    Extension = ".txt",
    FilterName = "My txt files",
    BytesFunc = () => Task.FromResult(Encoding.UTF8.GetBytes(text)),
});
```

### MessageInteractions

WinUI requires a window to display the ContentDialog, so you'll need to set it explicitly in your App.OnLaunched:
```cs
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
#if HAS_WINUI
        var window = new Window();
        MessageInteractionManager.Window = window;
#endif
    }
```


### DragAndDropExtensions
```
// WPF
xmlns:h="clr-namespace:H.ReactiveUI;assembly=H.ReactiveUI.CommonInteractions.Wpf" 
// UWP/Uno
xmlns:h="using:H.ReactiveUI"
```
```xml
<Element
    AllowDrop="True"
    h:DragAndDropExtensions.DragFilesEnterCommand="{Binding DragFilesEnter}"
    h:DragAndDropExtensions.DragTextEnterCommand="{Binding DragTextEnter}"
    h:DragAndDropExtensions.DragLeaveCommand="{Binding DragLeave}"
    h:DragAndDropExtensions.DropFilesCommand="{Binding DropFiles}"
    h:DragAndDropExtensions.DropTextCommand="{Binding DropText}"
    >
```

Command arguments:
DragFilesEnterCommand - `FileData[]` - Array of files.  
DragTextEnterCommand - `string` - Text.  
DragLeaveCommand - `null`.  
DropFilesCommand - `FileData[]` - Array of files.  
DropTextCommand - `string` - Text.  

## Contacts
* [mail](mailto:havendv@gmail.com)