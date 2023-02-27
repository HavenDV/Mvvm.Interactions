# [Mvvm.CommonInteractions](https://github.com/HavenDV/Mvvm.CommonInteractions/) 
Common MVVM Level Interactions(like open/save file) for WPF/UWP/WinUI/Uno/Avalonia/Maui platforms.
Features:
- Enables drag-and-drop support at the ViewModel level.
- Wide choice of platforms
- Dependency injection friendly

### NuGet

[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Core.svg?style=flat-square&label=Mvvm.CommonInteractions.Core)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Core/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Wpf.svg?style=flat-square&label=Mvvm.CommonInteractions.Wpf)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Wpf/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Uno.svg?style=flat-square&label=Mvvm.CommonInteractions.Uno)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Uno/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Uwp.svg?style=flat-square&label=Mvvm.CommonInteractions.Uwp)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Uwp/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.WinUI.svg?style=flat-square&label=Mvvm.CommonInteractions.WinUI)](https://www.nuget.org/packages/Mvvm.CommonInteractions.WinUI/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Avalonia.svg?style=flat-square&label=Mvvm.CommonInteractions.Avalonia)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Avalonia/)
[![NuGet](https://img.shields.io/nuget/dt/Mvvm.CommonInteractions.Maui.svg?style=flat-square&label=Mvvm.CommonInteractions.Maui)](https://www.nuget.org/packages/Mvvm.CommonInteractions.Maui/)

```
Install-Package Mvvm.CommonInteractions.Core
Install-Package Mvvm.CommonInteractions.Wpf
Install-Package Mvvm.CommonInteractions.Uno
Install-Package Mvvm.CommonInteractions.Uwp
Install-Package Mvvm.CommonInteractions.WinUI
Install-Package Mvvm.CommonInteractions.Avalonia
Install-Package Mvvm.CommonInteractions.Maui
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
                services.AddCommonInteractions();
                
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
xmlns:dragAndDrop="clr-namespace:Mvvm.CommonInteractions;assembly=Mvvm.CommonInteractions.Wpf" 
// UWP/Uno
xmlns:dragAndDrop="using:Mvvm.CommonInteractions"
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
