using MDBX.Interop.Functions;

namespace MDBX.Interop;

/// <summary>
/// Взаимодействие с нативными функциями MDBX для операций с курсором.
/// </summary>
internal static partial class Cursor
{
    /// <summary>
    /// Привязывает все делегаты курсора к нативным функциям библиотеки MDBX.
    /// </summary>
    internal static void Bind()
    {
        _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>(MdbxFunctions.Cursor.Close);
        _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>(MdbxFunctions.Cursor.Open);
        _getDelegate = NativeLibraryLoader.GetProcAddress<GetDelegate>(MdbxFunctions.Cursor.Get);
        _putDelegate = NativeLibraryLoader.GetProcAddress<PutDelegate>(MdbxFunctions.Cursor.Put);
        _delDelegate = NativeLibraryLoader.GetProcAddress<DelDelegate>(MdbxFunctions.Cursor.Del);
        _countDelegate = NativeLibraryLoader.GetProcAddress<CountDelegate>(MdbxFunctions.Cursor.Count);
    }
}
