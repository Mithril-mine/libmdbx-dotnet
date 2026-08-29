using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Stat;

/// <summary>
/// Частичный класс для нативных методов статистики libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Получает статистику окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="txn">Транзакция (может быть null).</param>
    /// <param name="stat">Структура для статистики.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_stat_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvStatEx(MDBX_env* env, MDBX_txn* txn, MDBX_stat* stat, nuint bytes);

    /// <summary>
    /// Получает информацию об окружении.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="txn">Транзакция (может быть null).</param>
    /// <param name="info">Структура для информации.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_info_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvInfoEx(MDBX_env* env, MDBX_txn* txn, MDBX_envinfo* info, nuint bytes);

    /// <summary>
    /// Синхронизирует окружение.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="force">Принудительная синхронизация.</param>
    /// <param name="nonblock">Неблокирующая.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_sync_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSyncEx(MDBX_env* env, [MarshalAs(UnmanagedType.I1)] bool force, [MarshalAs(UnmanagedType.I1)] bool nonblock);
}
