using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class MdbxInteropTransaction
{
    /// <summary>
    /// int mdbx_txn_begin(MDBX_env *env, MDBX_txn *parent, mdbx_txn_flags_t flags, MDBX_txn **txn)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int BeginDelegate(IntPtr env, IntPtr parent, [MarshalAs(UnmanagedType.U4)] int flags, out IntPtr txn);

    private static BeginDelegate? _beginDelegate = null;

    /// <summary>
    /// Начинает новую транзакцию.
    /// </summary>
    /// <param name="env">Указатель на среду.</param>
    /// <param name="parent">Указатель на родительскую транзакцию (или IntPtr.Zero).</param>
    /// <param name="flags">Флаги транзакции.</param>
    /// <returns>Указатель на созданную транзакцию.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при создании транзакции.</exception>
    internal static IntPtr Begin(IntPtr env, IntPtr parent, TransactionOption flags)
    {
        if (_beginDelegate is null)
            throw new InvalidOperationException("Txn.Begin called before Library.Load()");
        IntPtr ptr;
        int err = _beginDelegate(env, parent, (int)flags, out ptr);
        if (err != 0)
            throw new MdbxException("mdbx_txn_begin", err);
        return ptr;
    }
}
