using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Cursor
{
    /// <summary>
    /// int mdbx_cursor_open(MDBX_txn *txn, MDBX_dbi dbi, MDBX_cursor **cursor)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int OpenDelegate(IntPtr txn, uint dbi, out IntPtr cursor);

    private static OpenDelegate? _openDelegate = null;

    /// <summary>
    /// Открывает новый курсор для заданной базы данных в транзакции.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <returns>Указатель на созданный курсор.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при открытии курсора.</exception>
    internal static IntPtr Open(IntPtr txn, uint dbi)
    {
        if (_openDelegate is null)
            throw new InvalidOperationException("Cursor.Open called before Library.Load()");
        IntPtr ptr;
        int err = _openDelegate(txn, dbi, out ptr);
        if (err != 0)
            throw new MdbxException("mdbx_cursor_open", err);
        return ptr;
    }
}
