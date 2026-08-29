using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Env;

/// <summary>
/// Частичный класс для нативных методов окружения libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Создает окружение MDBX.
    /// </summary>
    /// <param name="penv">Указатель для хранения окружения.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_create", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvCreate(MDBX_env** penv);

    /// <summary>
    /// Открывает окружение MDBX (ANSI версия).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="flags">Флаги окружения.</param>
    /// <param name="mode">Режим доступа.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxEnvOpen(MDBX_env* env, byte* pathname, MDBX_env_flags_t flags, ushort mode);

    /// <summary>
    /// Открывает окружение MDBX (Unicode версия).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="flags">Флаги окружения.</param>
    /// <param name="mode">Режим доступа.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_openW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxEnvOpenW(MDBX_env* env, char* pathname, MDBX_env_flags_t flags, ushort mode);

    /// <summary>
    /// Закрывает окружение MDBX.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_close", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvClose(MDBX_env* env);

    /// <summary>
    /// Закрывает окружение MDBX с дополнительными опциями.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dontSync">Не синхронизировать при закрытии.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_close_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvCloseEx(MDBX_env* env, [MarshalAs(UnmanagedType.I1)] bool dontSync);

    /// <summary>
    /// Удаляет окружение MDBX (ANSI версия).
    /// </summary>
    /// <param name="pathname">Путь к окружению.</param>
    /// <param name="mode">Режим удаления.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_delete", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxEnvDelete(byte* pathname, MDBX_env_delete_mode_t mode);

    /// <summary>
    /// Удаляет окружение MDBX (Unicode версия).
    /// </summary>
    /// <param name="pathname">Путь к окружению.</param>
    /// <param name="mode">Режим удаления.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_deleteW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxEnvDeleteW(char* pathname, MDBX_env_delete_mode_t mode);

    /// <summary>
    /// Копирует окружение MDBX.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dest">Путь назначения.</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxEnvCopy(MDBX_env* env, byte* dest, MDBX_copy_flags_t flags);

    /// <summary>
    /// Копирует окружение MDBX (Unicode версия).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dest">Путь назначения.</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_copyW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxEnvCopyW(MDBX_env* env, char* dest, MDBX_copy_flags_t flags);

    /// <summary>
    /// Копирует окружение MDBX в файловый дескриптор.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="fd">Файловый дескриптор (mdbx_filehandle_t, кроссплатформенный).</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_copy2fd", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvCopy2Fd(MDBX_env* env, IntPtr fd, MDBX_copy_flags_t flags);

    /// <summary>
    /// Копирует транзакцию в путь назначения (ANSI версия).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dest">Путь назначения.</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_copy2pathname", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxTxnCopy2Pathname(MDBX_txn* txn, byte* dest, MDBX_copy_flags_t flags);

    /// <summary>
    /// Копирует транзакцию в путь назначения (Unicode версия, Windows только).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dest">Путь назначения.</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_copy2pathnameW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxTxnCopy2PathnameW(MDBX_txn* txn, char* dest, MDBX_copy_flags_t flags);

    /// <summary>
    /// Копирует транзакцию в файловый дескриптор.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="fd">Файловый дескриптор (mdbx_filehandle_t, кроссплатформенный).</param>
    /// <param name="flags">Флаги копирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_copy2fd", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnCopy2Fd(MDBX_txn* txn, IntPtr fd, MDBX_copy_flags_t flags);

    /// <summary>
    /// Открывает окружение для восстановления (ANSI версия).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="targetMeta">Целевая мета-страница.</param>
    /// <param name="writeable">Доступно для записи.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_open_for_recovery", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxEnvOpenForRecovery(MDBX_env* env, byte* pathname, uint targetMeta, [MarshalAs(UnmanagedType.I1)] bool writeable);

    /// <summary>
    /// Открывает окружение для восстановления (Unicode версия).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="targetMeta">Целевая мета-страница.</param>
    /// <param name="writeable">Доступно для записи.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_open_for_recoveryW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxEnvOpenForRecoveryW(MDBX_env* env, char* pathname, uint targetMeta, [MarshalAs(UnmanagedType.I1)] bool writeable);

    /// <summary>
    /// Переключает окружение для восстановления.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="targetMeta">Целевая мета-страница.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_turn_for_recovery", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvTurnForRecovery(MDBX_env* env, uint targetMeta);

    /// <summary>
    /// Восстанавливает окружение после fork.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_resurrect_after_fork", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvResurrectAfterFork(MDBX_env* env);

    /// <summary>
    /// Прогревает окружение (предзагружает страницы в память).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="txn">Транзакция.</param>
    /// <param name="flags">Флаги прогрева.</param>
    /// <param name="timeoutSeconds16Dot16">Таймаут в 1/65536 сек (0 = без ограничения).</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_warmup", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvWarmup(MDBX_env* env, MDBX_txn* txn, MDBX_warmup_flags_t flags, uint timeoutSeconds16Dot16);

    /// <summary>
    /// Фрагментирует окружение (defrag).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="defragAtleast">Минимальное количество страниц для дефрагментации.</param>
    /// <param name="timeAtleastDot16">Минимальное время (1/65536 сек).</param>
    /// <param name="defragEnough">Достаточное количество страниц.</param>
    /// <param name="timeLimitDot16">Лимит времени (1/65536 сек).</param>
    /// <param name="acceptableBacklash">Допустимый откат.</param>
    /// <param name="preferredBatch">Предпочтительный размер пакета.</param>
    /// <param name="progressCallback">Функция обратного вызова для уведомления о прогрессе.</param>
    /// <param name="ctx">Контекст для колбэка.</param>
    /// <param name="result">Структура результата дефрагментации.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_defrag", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvDefrag(MDBX_env* env, nuint defragAtleast, nuint timeAtleastDot16, nuint defragEnough, nuint timeLimitDot16, nint acceptableBacklash, nint preferredBatch, delegate* unmanaged[Cdecl]<void*, MDBX_defrag_result_t*, int> progressCallback, void* ctx, MDBX_defrag_result_t* result);

    /// <summary>
    /// Открывает снапшот информации окружения (ANSI версия).
    /// </summary>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="info">Структура информации.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_preopen_snapinfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxPreopenSnapinfo(byte* pathname, MDBX_envinfo* info, nuint bytes);

    /// <summary>
    /// Открывает снапшот информации окружения (Unicode версия).
    /// </summary>
    /// <param name="pathname">Путь к файлу.</param>
    /// <param name="info">Структура информации.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_preopen_snapinfoW", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MdbxPreopenSnapinfoW(char* pathname, MDBX_envinfo* info, nuint bytes);
}
