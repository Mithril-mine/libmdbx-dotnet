using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    /// <summary>
    /// Взаимодействие с нативными функциями MDBX для операций с транзакциями.
    /// </summary>
    internal static class Transaction
    {
        /// <summary>
        /// Делегат для функции mdbx_txn_begin.
        /// </summary>
        /// <param name="env">Указатель на среду.</param>
        /// <param name="parent">Указатель на родительскую транзакцию (или null).</param>
        /// <param name="flags">Флаги транзакции.</param>
        /// <param name="txn">Возвращаемый указатель на транзакцию.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
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

        /// <summary>
        /// Делегат для функции mdbx_txn_commit.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
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

        /// <summary>
        /// Делегат для функции mdbx_txn_abort.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
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

        /// <summary>
        /// Делегат для функции mdbx_txn_reset.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
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

        /// <summary>
        /// Делегат для функции mdbx_txn_renew.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
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

        /// <summary>
        /// Делегат для функции mdbx_txn_id.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <returns>Идентификатор транзакции.</returns>
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

        /// <summary>
        /// Привязывает все делегаты транзакций к нативным функциям библиотеки MDBX.
        /// </summary>
        internal static void Bind()
        {
            _beginDelegate = NativeLibraryLoader.GetProcAddress<BeginDelegate>("mdbx_txn_begin");
            _commitDelegate = NativeLibraryLoader.GetProcAddress<CommitDelegate>("mdbx_txn_commit");
            _abortDelegate = NativeLibraryLoader.GetProcAddress<AbortDelegate>("mdbx_txn_abort");
            _resetDelegate = NativeLibraryLoader.GetProcAddress<ResetDelegate>("mdbx_txn_reset");
            _renewDelegate = NativeLibraryLoader.GetProcAddress<RenewDelegate>("mdbx_txn_renew");
            _getTxnIdDelegate = NativeLibraryLoader.GetProcAddress<GetTxnIdDelegate>("mdbx_txn_id");
        }
    }
}
