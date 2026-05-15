using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Transaction
{
    /// <summary>
    /// mdbx_txnid_t mdbx_txn_id(const MDBX_txn *txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate ulong GetTxnIdDelegate(IntPtr txn);

    private static GetTxnIdDelegate? _getTxnIdDelegate = null;

    /// <summary>
    /// Получает идентификатор транзакции.
    /// Для транзакции только для чтения это соответствует снимку, который читается.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <returns>Идентификатор транзакции.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    internal static ulong GetID(IntPtr txn)
    {
        if (_getTxnIdDelegate is null)
            throw new InvalidOperationException("Txn.GetID called before Library.Load()");
        return _getTxnIdDelegate(txn);
    }
}
