using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Settings;

/// <summary>
/// Частичный класс для нативных методов настроек libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Устанавливает параметр окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="option">Параметр.</param>
    /// <param name="value">Значение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_set_option", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSetOption(MDBX_env* env, MDBX_option_t option, ulong value);

    /// <summary>
    /// Получает параметр окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="option">Параметр.</param>
    /// <param name="pValue">Указатель для значения.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_option", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetOption(MDBX_env* env, MDBX_option_t option, ulong* pValue);

    /// <summary>
    /// Устанавливает флаги окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Флаги.</param>
    /// <param name="onoff">Включить или выключить.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_set_flags", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSetFlags(MDBX_env* env, MDBX_env_flags_t flags, [MarshalAs(UnmanagedType.I1)] bool onoff);

    /// <summary>
    /// Получает флаги окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Указатель для флагов.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_flags", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetFlags(MDBX_env* env, uint* flags);

    /// <summary>
    /// Устанавливает геометрию окружения (размеры файлов).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="sizeLower">Нижний предел размера.</param>
    /// <param name="sizeNow">Текущий размер.</param>
    /// <param name="sizeUpper">Верхний предел размера.</param>
    /// <param name="growthStep">Шаг роста.</param>
    /// <param name="shrinkThreshold">Порог сжатия.</param>
    /// <param name="pagesize">Размер страницы.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_set_geometry", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSetGeometry(MDBX_env* env, nint sizeLower, nint sizeNow, nint sizeUpper, nint growthStep, nint shrinkThreshold, nint pagesize);

    /// <summary>
    /// Устанавливает колбэк Handle-Slow-Readers.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="hsrCallback">Функция колбэка.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_set_hsr", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSetHsr(MDBX_env* env, delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int> hsrCallback);

    /// <summary>
    /// Получает колбэк Handle-Slow-Readers.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Указатель на функцию колбэка.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_hsr", CallingConvention = CallingConvention.Cdecl)]
    public static extern delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int> MdbxEnvGetHsr(MDBX_env* env);

    /// <summary>
    /// Устанавливает пользовательский контекст окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="ctx">Пользовательский контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_set_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvSetUserctx(MDBX_env* env, void* ctx);

    /// <summary>
    /// Получает пользовательский контекст окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Указатель на пользовательский контекст.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern void* MdbxEnvGetUserctx(MDBX_env* env);

    /// <summary>
    /// Получает путь к окружению.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dest">Буфер для пути.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_path", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetPath(MDBX_env* env, byte** dest);

    /// <summary>
    /// Получает путь к окружению (wide version).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dest">Буфер для пути.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_pathW", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetPathW(MDBX_env* env, char** dest);

    /// <summary>
    /// Получает дескриптор файла окружения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="fd">Указатель для дескриптора.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_fd", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetFd(MDBX_env* env, IntPtr* fd);

    /// <summary>
    /// Получает максимальный размер ключа.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер ключа.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_maxkeysize_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetMaxkeysizeEx(MDBX_env* env, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер ключа (устаревшая версия без флагов).
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Максимальный размер ключа.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_maxkeysize", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetMaxkeysize(MDBX_env* env);

    /// <summary>
    /// Получает максимальный размер значения.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер значения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_maxvalsize_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetMaxvalsizeEx(MDBX_env* env, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер пары ключ-значение для страницы.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер пары.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_pairsize4page_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetPairsize4pageMax(MDBX_env* env, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер значения для страницы.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер значения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_get_valsize4page_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvGetValsize4pageMax(MDBX_env* env, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает размер страницы по умолчанию.
    /// </summary>
    /// <returns>Размер страницы в байтах.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_default_pagesize", CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint MdbxDefaultPagesize();

    /// <summary>
    /// Получает информацию о системной RAM.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="totalPages">Общее количество страниц.</param>
    /// <param name="availPages">Доступное количество страниц.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get_sysraminfo", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxGetSysraminfo(nint* pageSize, nint* totalPages, nint* availPages);

    /// <summary>
    /// Проверяет, разумно ли использовать readahead для заданного объема.
    /// </summary>
    /// <param name="volume">Объем данных.</param>
    /// <param name="redundancy">Избыточность.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_is_readahead_reasonable", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxIsReadaheadReasonable(nuint volume, nint redundancy);

    /// <summary>
    /// Получает минимальный размер базы данных.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <returns>Минимальный размер.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_dbsize_min", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsDbsizeMin(nint pagesize);

    /// <summary>
    /// Получает максимальный размер базы данных.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <returns>Максимальный размер.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_dbsize_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsDbsizeMax(nint pagesize);

    /// <summary>
    /// Получает минимальный размер ключа.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Минимальный размер ключа.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_keysize_min", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsKeysizeMin(MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер ключа.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер ключа.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_keysize_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsKeysizeMax(nint pagesize, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает минимальный размер значения.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Минимальный размер значения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_valsize_min", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsValsizeMin(MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер значения.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер значения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_valsize_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsValsizeMax(nint pagesize, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер пары ключ-значение для страницы.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер пары.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_pairsize4page_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsPairsize4pageMax(nint pagesize, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер значения для страницы.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Максимальный размер значения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_valsize4page_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsValsize4pageMax(nint pagesize, MDBX_db_flags_t flags);

    /// <summary>
    /// Получает максимальный размер транзакции.
    /// </summary>
    /// <param name="pagesize">Размер страницы.</param>
    /// <returns>Максимальный размер транзакции.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_limits_txnsize_max", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint MdbxLimitsTxnsizeMax(nint pagesize);

    /// <summary>
    /// Преобразует соотношение в строку с заданной точностью.
    /// </summary>
    /// <param name="numerator">Числитель.</param>
    /// <param name="denominator">Знаменатель.</param>
    /// <param name="precision">Точность.</param>
    /// <param name="buffer">Буфер для результата.</param>
    /// <param name="bufferSize">Размер буфера.</param>
    /// <returns>Указатель на строку.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_ratio2digits", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxRatio2Digits(ulong numerator, ulong denominator, int precision, byte* buffer, nuint bufferSize);

    /// <summary>
    /// Преобразует соотношение в проценты.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <param name="whole">Целое.</param>
    /// <param name="buffer">Буфер для результата.</param>
    /// <param name="bufferSize">Размер буфера.</param>
    /// <returns>Указатель на строку.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_ratio2percents", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxRatio2Percents(ulong value, ulong whole, byte* buffer, nuint bufferSize);
}
