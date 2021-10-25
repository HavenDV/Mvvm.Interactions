namespace H.ReactiveUI;

public readonly record struct SaveFileArguments(
    string FileName,
    string Extension,
    string FilterName,
    Func<Task<byte[]>> BytesFunc);
