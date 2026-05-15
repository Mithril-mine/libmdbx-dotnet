using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Cursor
{
    /// <summary>
    /// int mdbx_cursor_del(MDBX_cursor *cursor, mdbx_del_flags_t flags)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DelDelegate(IntPtr cursor, CursorDelOption option);

    private static DelDelegate? _delDelegate = null;

    /// <summary>
    /// Удаляет текущий элемент через курсор.
    /// </summary>
    /// <param name="cursor">Указатель на курсор.</param>
    /// <param name="option">Опции удаления.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции удаления.</exception>
    internal static void Del(IntPtr cursor, CursorDelOption option)
    {
        if (_delDelegate is null)
            throw new InvalidOperationException("Cursor.Del called before Library.Load()");
        int err = _delDelegate(cursor, option);
        if (err != 0)
            throw new MdbxException("mdbx_cursor_del", err);
    }
}
