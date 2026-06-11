using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropCursor
{
    /// <summary>
    /// int mdbx_cursor_count(MDBX_cursor *cursor, size_t *count)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CountDelegate(IntPtr cursor, ref IntPtr count);

    private static CountDelegate? _countDelegate = null;

    /// <summary>
    /// Возвращает количество дубликатов для текущего ключа.
    /// </summary>
    /// <param name="cursor">Указатель на курсор.</param>
    /// <returns>Количество дубликатов.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при подсчёте.</exception>
    internal static int Count(IntPtr cursor)
    {
        if (_countDelegate is null)
            throw new InvalidOperationException("Cursor.Count called before Library.Load()");
        IntPtr count = IntPtr.Zero;
        int err = _countDelegate(cursor, ref count);
        if (err != 0)
            throw new MdbxException("mdbx_cursor_count", err);
        return count.ToInt32();
    }
}
