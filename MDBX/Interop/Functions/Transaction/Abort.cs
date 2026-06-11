using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropTransaction
{
    /// <summary>
    /// int mdbx_txn_abort(MDBX_txn *txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int AbortDelegate(IntPtr txn);

    private static AbortDelegate? _abortDelegate = null;

    /// <summary>
    /// Отменяет транзакцию.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при отмене транзакции.</exception>
    internal static void Abort(IntPtr txn)
    {
        if (_abortDelegate is null)
            throw new InvalidOperationException("Txn.Abort called before Library.Load()");
        int err = _abortDelegate(txn);
        if (err != 0)
            throw new MdbxException("mdbx_txn_abort", err);
    }
}
