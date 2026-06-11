using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropTransaction
{
    /// <summary>
    /// int mdbx_txn_renew(MDBX_txn *txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int RenewDelegate(IntPtr txn);

    private static RenewDelegate? _renewDelegate = null;

    /// <summary>
    /// Обновляет транзакцию только для чтения.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при обновлении транзакции.</exception>
    internal static void Renew(IntPtr txn)
    {
        if (_renewDelegate is null)
            throw new InvalidOperationException("Txn.Renew called before Library.Load()");
        int err = _renewDelegate(txn);
        if (err != 0)
            throw new MdbxException("mdbx_txn_renew", err);
    }
}
