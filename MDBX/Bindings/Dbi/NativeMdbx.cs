using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Dbi;

/// <summary>
/// Частичный класс для нативных методов таблиц (DBI) libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Открывает таблицу по имени.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="name">Имя таблицы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <param name="dbi">Указатель для DBI.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxDbiOpen(MDBX_txn* txn, byte* name, MDBX_db_flags_t flags, uint* dbi);

    /// <summary>
    /// Открывает таблицу по значению.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="name">Имя таблицы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <param name="dbi">Указатель для DBI.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_open2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiOpen2(MDBX_txn* txn, MDBX_val* name, MDBX_db_flags_t flags, uint* dbi);

    /// <summary>
    /// Открывает таблицу по имени с пользовательскими функциями сравнения (устаревшая версия).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="name">Имя таблицы (null-terminated).</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <param name="dbi">Указатель для DBI.</param>
    /// <param name="keycmp">Функция сравнения ключей (nullptr = использовать по умолчанию).</param>
    /// <param name="datacmp">Функция сравнения значений (nullptr = использовать по умолчанию).</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_open_ex", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxDbiOpenEx(MDBX_txn* txn, byte* name, MDBX_db_flags_t flags, uint* dbi,
        delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> keycmp, delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> datacmp);

    /// <summary>
    /// Открывает таблицу по значению с пользовательскими функциями сравнения (устаревшая версия).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="name">Имя таблицы (MDBX_val).</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <param name="dbi">Указатель для DBI.</param>
    /// <param name="keycmp">Функция сравнения ключей (nullptr = использовать по умолчанию).</param>
    /// <param name="datacmp">Функция сравнения значений (nullptr = использовать по умолчанию).</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_open_ex2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiOpenEx2(MDBX_txn* txn, MDBX_val* name, MDBX_db_flags_t flags, uint* dbi,
        delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> keycmp, delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> datacmp);

    /// <summary>
    /// Закрывает таблицу.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dbi">DBI.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_close", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiClose(MDBX_env* env, uint dbi);

    /// <summary>
    /// Переименовывает таблицу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="name">Новое имя.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_rename", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int MdbxDbiRename(MDBX_txn* txn, uint dbi, byte* name);

    /// <summary>
    /// Переименовывает таблицу по значению.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="name">Новое имя.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_rename2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiRename2(MDBX_txn* txn, uint dbi, MDBX_val* name);

    /// <summary>
    /// Получает статистику таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="stat">Структура для статистики.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_stat", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiStat(MDBX_txn* txn, uint dbi, MDBX_stat* stat, nuint bytes);

    /// <summary>
    /// Получает маску глубины дубликатов.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="mask">Указатель для маски.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_dupsort_depthmask", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiDupsortDepthmask(MDBX_txn* txn, uint dbi, uint* mask);

    /// <summary>
    /// Получает флаги и состояние таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="flags">Указатель для флагов.</param>
    /// <param name="state">Указатель для состояния.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_flags_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiFlagsEx(MDBX_txn* txn, uint dbi, uint* flags, uint* state);

    /// <summary>
    /// Получает/устанавливает последовательность таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="result">Указатель для результата.</param>
    /// <param name="increment">Инкремент.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_sequence", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiSequence(MDBX_txn* txn, uint dbi, ulong* result, ulong increment);

    /// <summary>
    /// Удаляет таблицу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="del">Удалить данные.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_drop", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDrop(MDBX_txn* txn, uint dbi, [MarshalAs(UnmanagedType.I1)] bool del);
}
