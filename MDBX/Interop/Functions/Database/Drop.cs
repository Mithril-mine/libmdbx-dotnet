using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropDataBase
{
    /// <summary>
    /// int mdbx_drop(MDBX_txn *txn, MDBX_dbi dbi, int del)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DropDelegate(IntPtr txn, uint dbi, int del);

    private static DropDelegate? _dropDelegate = null;

    /// <summary>
    /// Удаляет базу данных или очищает её содержимое.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="del">Если true - удаляет базу полностью, иначе только очищает.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции drop.</exception>
    internal static void Drop(IntPtr txn, uint dbi, bool del)
    {
        if (_dropDelegate is null)
            throw new InvalidOperationException("Dbi.Drop called before Library.Load()");
        int err = _dropDelegate(txn, dbi, del ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_drop", err);
    }
}
