using MDBX.Interop.Models;
using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class MdbxInteropCursor
{
    /// <summary>
    /// int mdbx_cursor_get(MDBX_cursor *cursor, MDBX_val *key, MDBX_val *data, mdbx_cursor_op_t op)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetDelegate(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorOption op);

    private static GetDelegate? _getDelegate = null;

    /// <summary>
    /// Получает элемент данных через курсор.
    /// </summary>
    /// <param name="cursor">Указатель на курсор.</param>
    /// <param name="key">Структура ключа (входное значение инициализирует позицию, выходное - получает ключ).</param>
    /// <param name="value">Структура значения (получает данные).</param>
    /// <param name="op">Операция курсора (First, Next, Prev и т.д.).</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции get.</exception>
    internal static void Get(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorOption op)
    {
        if (_getDelegate is null)
            throw new InvalidOperationException("Cursor.Get called before Library.Load()");
        int err = _getDelegate(cursor, ref key, ref value, op);
        if (err != 0)
            throw new MdbxException("mdbx_cursor_get", err);
    }
}
