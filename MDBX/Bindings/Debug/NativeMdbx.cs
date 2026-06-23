using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Debug;

/// <summary>
/// Частичный класс для нативных методов отладки libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Настраивает отладочное логирование libmdbx.
    /// </summary>
    /// <param name="logLevel">Уровень логирования.</param>
    /// <param name="debugFlags">Флаги отладки.</param>
    /// <param name="logger">Функция логирования.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_setup_debug", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxSetupDebug(MDBX_log_level_t logLevel, MDBX_debug_flags_t debugFlags, delegate* unmanaged[Cdecl]<MDBX_log_level_t, byte*, int, byte*, byte*, void> logger);

    /// <summary>
    /// Настраивает отладочное логирование без форматирования.
    /// </summary>
    /// <param name="logLevel">Уровень логирования.</param>
    /// <param name="debugFlags">Флаги отладки.</param>
    /// <param name="logger">Функция логирования без форматирования.</param>
    /// <param name="loggerBuffer">Буфер для форматирования сообщений.</param>
    /// <param name="loggerBufferSize">Размер буфера.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_setup_debug_nofmt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxSetupDebugNofmt(MDBX_log_level_t logLevel, MDBX_debug_flags_t debugFlags, delegate* unmanaged[Cdecl]<MDBX_log_level_t, byte*, int, byte*, void> logger, byte* loggerBuffer, nuint loggerBufferSize);

    /// <summary>
    /// Устанавливает структуру-маяк (canary) для окружения.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="canary">Структура-маяк.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_canary_put", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCanaryPut(MDBX_txn* txn, MDBX_canary* canary);

    /// <summary>
    /// Получает структуру-маяк (canary) окружения.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="canary">Структура-маяк для заполнения.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_canary_get", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCanaryGet(MDBX_txn* txn, MDBX_canary* canary);

    /// <summary>
    /// Форматирует значение MDBX_val в строку.
    /// </summary>
    /// <param name="key">Ключ для форматирования.</param>
    /// <param name="buf">Буфер для результата.</param>
    /// <param name="bufsize">Размер буфера.</param>
    /// <returns>Указатель на строку.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dump_val", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxDumpVal(MDBX_val* key, byte* buf, nuint bufsize);
}
