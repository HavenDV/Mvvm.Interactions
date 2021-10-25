namespace H.ReactiveUI;

public readonly record struct OpenFileArguments(
    string FileName,
    string[] Extensions,
    string FilterName);
