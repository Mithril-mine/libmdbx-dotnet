using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Txn;

/// <summary>
/// Частичный класс для нативных методов транзакций libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Начинает транзакцию.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="parent">Родительская транзакция.</param>
    /// <param name="flags">Флаги транзакции.</param>
    /// <param name="txn">Указатель для транзакции.</param>
    /// <param name="size">Размер.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_begin_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnBeginEx(MDBX_env* env, MDBX_txn* parent, MDBX_txn_flags_t flags, MDBX_txn** txn, nuint size);

    /// <summary>
    /// Клонирует транзакцию.
    /// </summary>
    /// <param name="origin">Исходная транзакция.</param>
    /// <param name="inOutClone">Указатель для клона.</param>
    /// <param name="context">Пользовательский контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_clone", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnClone(MDBX_txn* origin, MDBX_txn** inOutClone, void* context);

    /// <summary>
    /// Устанавливает пользовательский контекст транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="ctx">Пользовательский контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_set_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnSetUserctx(MDBX_txn* txn, void* ctx);

    /// <summary>
    /// Получает пользовательский контекст транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Указатель на пользовательский контекст.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_get_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern void* MdbxTxnGetUserctx(MDBX_txn* txn);

    /// <summary>
    /// Получает информацию о транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="info">Структура для информации.</param>
    /// <param name="scanRlt">Сканировать читателей.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_info", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnInfo(MDBX_txn* txn, MDBX_txn_info* info, [MarshalAs(UnmanagedType.I1)] bool scanRlt);

    /// <summary>
    /// Получает окружение транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Указатель на окружение.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_env", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_env* MdbxTxnEnv(MDBX_txn* txn);

    /// <summary>
    /// Получает флаги транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Флаги транзакции.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_flags", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_txn_flags_t MdbxTxnFlags(MDBX_txn* txn);

    /// <summary>
    /// Получает ID транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>ID транзакции.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_id", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong MdbxTxnId(MDBX_txn* txn);

    /// <summary>
    /// Фиксирует транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="latency">Информация о задержках.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_commit_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnCommitEx(MDBX_txn* txn, MDBX_commit_latency* latency);

    /// <summary>
    /// Выполняет checkpoint транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="weakeningDurability">Флаги ослабления durability.</param>
    /// <param name="latency">Информация о задержках.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_checkpoint", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnCheckpoint(MDBX_txn* txn, MDBX_txn_flags_t weakeningDurability, MDBX_commit_latency* latency);

    /// <summary>
    /// Фиксирует транзакцию для embark read.
    /// </summary>
    /// <param name="ptxn">Указатель на транзакцию.</param>
    /// <param name="latency">Информация о задержках.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_commit_embark_read", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnCommitEmbarkRead(MDBX_txn** ptxn, MDBX_commit_latency* latency);

    /// <summary>
    /// Изменяет read-транзакцию в write-транзакцию.
    /// </summary>
    /// <param name="readTxn">Read-транзакция.</param>
    /// <param name="ptrWriteTxn">Указатель для write-транзакции.</param>
    /// <param name="flags">Флаги.</param>
    /// <param name="context">Пользовательский контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_amend", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnAmend(MDBX_txn* readTxn, MDBX_txn** ptrWriteTxn, MDBX_txn_flags_t flags, void* context);

    /// <summary>
    /// Откатывает транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_rollback", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnRollback(MDBX_txn* txn);

    /// <summary>
    /// Отменяет транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="latency">Информация о задержках.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_abort_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnAbortEx(MDBX_txn* txn, MDBX_commit_latency* latency);

    /// <summary>
    /// Ломает транзакцию (прерывает операцию).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_break", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnBreak(MDBX_txn* txn);

    /// <summary>
    /// Сбрасывает транзакцию (освобождает ресурсы, но не откатывает).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_reset", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnReset(MDBX_txn* txn);

    /// <summary>
    /// Паркует транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="autounpark">Автоматически unpark при доступе.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_park", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnPark(MDBX_txn* txn, [MarshalAs(UnmanagedType.I1)] bool autounpark);

    /// <summary>
    /// Отменяет парковку транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="restartIfOusted">Перезапустить если вытеснен.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_unpark", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnUnpark(MDBX_txn* txn, [MarshalAs(UnmanagedType.I1)] bool restartIfOusted);

    /// <summary>
    /// Обновляет (renew) транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_renew", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnRenew(MDBX_txn* txn);

    /// <summary>
    /// Обновляет (refresh) транзакцию.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_refresh", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnRefresh(MDBX_txn* txn);

    /// <summary>
    /// Освобождает все курсоры транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="unbind">Отвязать курсоры.</param>
    /// <param name="count">Количество освобожденных курсоров.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_release_all_cursors_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnReleaseAllCursorsEx(MDBX_txn* txn, [MarshalAs(UnmanagedType.I1)] bool unbind, nuint* count);

    /// <summary>
    /// Получает информацию о "straggler" транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="percent">Процент.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_straggler", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnStraggler(MDBX_txn* txn, int* percent);
}
