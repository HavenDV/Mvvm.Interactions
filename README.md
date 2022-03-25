# [H.ReactiveUI.CommonInteractions](https://github.com/HavenDV/H.ReactiveUI.CommonInteractions/) 
Common ReactiveUI MVVM Level Interactions(like open/save file) for WPF/UWP/WinUI/Uno/Avalonia platforms.

### NuGet

[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Core.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Core)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Core/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Wpf.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Wpf)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Wpf/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Uno.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Uno)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Uno/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Uwp.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Uwp)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Uwp/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.WinUI.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.WinUI)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.WinUI/)
[![NuGet](https://img.shields.io/nuget/dt/H.ReactiveUI.CommonInteractions.Avalonia.svg?style=flat-square&label=H.ReactiveUI.CommonInteractions.Avalonia)](https://www.nuget.org/packages/H.ReactiveUI.CommonInteractions.Avalonia/)

```
Install-Package H.ReactiveUI.CommonInteractions.Core
Install-Package H.ReactiveUI.CommonInteractions.Wpf
Install-Package H.ReactiveUI.CommonInteractions.Uno
Install-Package H.ReactiveUI.CommonInteractions.Uwp
Install-Package H.ReactiveUI.CommonInteractions.WinUI
Install-Package H.ReactiveUI.CommonInteractions.Avalonia
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
// Open
var file = await FileInteractions.OpenFile.Handle(new OpenFileArguments
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

// Save (you need to save file from previuos step)
await file.WriteTextAsync(text).ConfigureAwait(false);

// Save As
var file = await FileInteractions.SaveFile.Handle(new SaveFileArguments(".txt")
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
await MessageInteractions.Message.Handle("Message");
await MessageInteractions.Warning.Handle("Warning");
await MessageInteractions.Exception.Handle(new InvalidOperationException("Exception"));
bool question = await MessageInteractions.Question.Handle(new QuestionData("Are you sure?"));
```

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

### WebInteractions
```cs
await WebInteractions.OpenUrl.Handle("https://www.google.com/");
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
