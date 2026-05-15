using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Cursor
{
    /// <summary>
    /// void mdbx_cursor_close(MDBX_cursor *cursor)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void CloseDelegate(IntPtr cursor);

    private static CloseDelegate? _closeDelegate = null;

    /// <summary>
    /// Закрывает курсор и освобождает его ресурсы.
    /// </summary>
    /// <param name="cursor">Указатель на курсор.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    internal static void Close(IntPtr cursor)
    {
        if (_closeDelegate is null)
            throw new InvalidOperationException("Cursor.Close called before Library.Load()");
        _closeDelegate(cursor);
    }
}
