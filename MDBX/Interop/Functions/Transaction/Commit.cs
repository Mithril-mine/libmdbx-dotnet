using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Transaction
{
    /// <summary>
    /// int mdbx_txn_commit(MDBX_txn *txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CommitDelegate(IntPtr txn);

    private static CommitDelegate? _commitDelegate = null;

    /// <summary>
    /// Фиксирует транзакцию.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при фиксации транзакции.</exception>
    internal static void Commit(IntPtr txn)
    {
        if (_commitDelegate is null)
            throw new InvalidOperationException("Txn.Commit called before Library.Load()");
        int err = _commitDelegate(txn);
        if (err != 0)
            throw new MdbxException("mdbx_txn_commit", err);
    }
}
