global using Mvvm.Interactions.Models;
global using DependencyPropertyGenerator;

#if HAS_AVALONIA
global using Avalonia;
global using Avalonia.Controls;
#elif HAS_WPF
global using System.Windows;
global using System.Windows.Controls;
#elif HAS_WINUI
global using Microsoft.UI.Xaml;
global using Microsoft.UI.Xaml.Controls;
global using Application = Microsoft.UI.Xaml.Application;
#elif HAS_MAUI
#else
global using Windows.UI.Xaml;
global using Windows.UI.Xaml.Controls;
global using Application = Windows.UI.Xaml.Application;
#endif