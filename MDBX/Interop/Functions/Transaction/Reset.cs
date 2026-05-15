using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Transaction
{
    /// <summary>
    /// int mdbx_txn_reset(MDBX_txn *txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ResetDelegate(IntPtr txn);

    private static ResetDelegate? _resetDelegate = null;

    /// <summary>
    /// Сбрасывает транзакцию только для чтения.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при сбросе транзакции.</exception>
    internal static void Reset(IntPtr txn)
    {
        if (_resetDelegate is null)
            throw new InvalidOperationException("Txn.Reset called before Library.Load()");
        int err = _resetDelegate(txn);
        if (err != 0)
            throw new MdbxException("mdbx_txn_reset", err);
    }
}
