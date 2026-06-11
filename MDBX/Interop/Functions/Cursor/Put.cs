using MDBX.Interop.Models;
using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropCursor
{
    /// <summary>
    /// int mdbx_cursor_put(MDBX_cursor *cursor, const MDBX_val *key, const MDBX_val *data, mdbx_put_flags_t flags)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PutDelegate(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorPutOption option);

    private static PutDelegate? _putDelegate = null;

    /// <summary>
    /// Записывает пару ключ/значение через курсор.
    /// </summary>
    /// <param name="cursor">Указатель на курсор.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="option">Опции операции.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции put.</exception>
    internal static void Put(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorPutOption option)
    {
        if (_putDelegate is null)
            throw new InvalidOperationException("Cursor.Put called before Library.Load()");
        int err = _putDelegate(cursor, ref key, ref value, option);
        if (err != 0)
            throw new MdbxException("mdbx_cursor_put", err);
    }
}
