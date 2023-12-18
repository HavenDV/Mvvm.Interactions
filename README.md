# [Mvvm.Interactions](https://github.com/HavenDV/Mvvm.Interactions/) 
Common MVVM Level Interactions(like open/save file) for WPF/UWP/WinUI/Uno/Avalonia/Maui platforms.
Features:
- Easy interactions with files, message boxes, web from MVVM level.
- Enables drag-and-drop support at the ViewModel level.
- Wide choice of platforms
- Dependency injection friendly

### NuGet

[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Core.svg?style=flat-square&label=Mvvm.Interactions.Core)](https://www.nuget.org/packages/Mvvm.Interactions.Core/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Wpf.svg?style=flat-square&label=Mvvm.Interactions.Wpf)](https://www.nuget.org/packages/Mvvm.Interactions.Wpf/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Uno.svg?style=flat-square&label=Mvvm.Interactions.Uno)](https://www.nuget.org/packages/Mvvm.Interactions.Uno/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Uwp.svg?style=flat-square&label=Mvvm.Interactions.Uwp)](https://www.nuget.org/packages/Mvvm.Interactions.Uwp/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.WinUI.svg?style=flat-square&label=Mvvm.Interactions.WinUI)](https://www.nuget.org/packages/Mvvm.Interactions.WinUI/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Avalonia.svg?style=flat-square&label=Mvvm.Interactions.Avalonia)](https://www.nuget.org/packages/Mvvm.Interactions.Avalonia/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.Interactions.Maui.svg?style=flat-square&label=Mvvm.Interactions.Maui)](https://www.nuget.org/packages/Mvvm.Interactions.Maui/)

```
Install-Package Mvvm.Interactions.Core
Install-Package Mvvm.Interactions.Wpf
Install-Package Mvvm.Interactions.Uno
Install-Package Mvvm.Interactions.Uwp
Install-Package Mvvm.Interactions.WinUI
Install-Package Mvvm.Interactions.Avalonia
Install-Package Mvvm.Interactions.Maui
```

## Usage
Add to your App constructors:
```cs

public sealed partial class App
{
    private IHost AppHost { get; }

    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices(static services =>
            {
                // Add all available interactions
                services.AddMvvmInteractions();
                
                // or add only what you need
                services.AddSingleton<IFileInteractions, FileInteractions>();
                services.AddSingleton<IMessageInteractions, MessageInteractions>();
                services.AddSingleton<IWebInteractions, WebInteractions>();
            })
            .Build();

        // Optional. Displays unhandled exceptions using MessageInteractions.Exception.
        AppHost.Services.GetRequiredService<IMessageInteractions>().CatchUnhandledExceptions(this);

        // your code
    }
}
```

### FileInteractions
```cs
// Open
var file = await FileInteractions.OpenFileAsync(new OpenFileArguments
{
    SuggestedFileName = "my.txt",
    Extensions = new[] { ".txt" },
    FilterName = "My txt files",
});
if (file == null)
{
    return;
}
var text = await file.ReadTextAsync().ConfigureAwait(true);

// Save (if you need to save file from previuos step)
await file.WriteTextAsync(text).ConfigureAwait(false);

// Save As
var file = await FileInteractions.SaveFileAsync(new SaveFileArguments(".txt")
{
    SuggestedFileName = "my.txt",
    FilterName = "My txt files",
});
if (file == null)
{
    return;
}
await file.WriteTextAsync(text).ConfigureAwait(false);
```

### MessageInteractions
```cs
await MessageInteractions.ShowMessageAsync("Message");
await MessageInteractions.ShowWarningAsync("Warning");
await MessageInteractions.ShowExceptionAsync(new InvalidOperationException("Exception"));
bool question = await MessageInteractions.ShowQuestionAsync(new QuestionData("Are you sure?"));
```

WinUI requires a window to display the ContentDialog, so you'll need to set it explicitly in your App.OnLaunched:
```cs
protected override void OnLaunched(LaunchActivatedEventArgs args)
{
#if HAS_WINUI
    var window = new Window();
    MessageInteractions.Window = window;
#endif
}
```

### WebInteractions
```cs
await WebInteractions.OpenUrlAsync("https://www.google.com/");
```

### DragAndDropExtensions
```
// WPF
xmlns:dragAndDrop="clr-namespace:Mvvm.Interactions;assembly=Mvvm.Interactions.Wpf" 
// UWP/Uno
xmlns:dragAndDrop="using:Mvvm.Interactions"
```
```xml
<Element
    AllowDrop="True"
    dragAndDrop:DragAndDropExtensions.DragFilesEnterCommand="{Binding DragFilesEnter}"
    dragAndDrop:DragAndDropExtensions.DragTextEnterCommand="{Binding DragTextEnter}"
    dragAndDrop:DragAndDropExtensions.DragLeaveCommand="{Binding DragLeave}"
    dragAndDrop:DragAndDropExtensions.DropFilesCommand="{Binding DropFiles}"
    dragAndDrop:DragAndDropExtensions.DropTextCommand="{Binding DropText}"
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
