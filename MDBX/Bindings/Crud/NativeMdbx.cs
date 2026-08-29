using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Crud;

/// <summary>
/// Частичный класс для нативных методов CRUD операций libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Получает значение по ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxGet(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data);

    /// <summary>
    /// Получает значение по ключу с расширенными опциями.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="valuesCount">Количество значений.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxGetEx(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data, nuint* valuesCount);

    /// <summary>
    /// Записывает значение.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="flags">Флаги.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_put", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxPut(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data, MDBX_put_flags_t flags);

    /// <summary>
    /// Заменяет значение.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="newData">Новое значение.</param>
    /// <param name="oldData">Старое значение.</param>
    /// <param name="flags">Флаги.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_replace", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxReplace(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* newData, MDBX_val* oldData, MDBX_put_flags_t flags);

    /// <summary>
    /// Заменяет значение с расширенными опциями.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="newData">Новое значение.</param>
    /// <param name="oldData">Старое значение.</param>
    /// <param name="flags">Флаги.</param>
    /// <param name="preserver">Функция сохранения данных.</param>
    /// <param name="preserver_context">Контекст для функции сохранения.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_replace_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxReplaceEx(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* newData, MDBX_val* oldData, MDBX_put_flags_t flags, MDBX.Native.Types.MdbxPreserveFunc? preserver, void* preserver_context);

    /// <summary>
    /// Удаляет элемент.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение (опционально).</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_del", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDel(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data);

    /// <summary>
    /// Получает значение по ключу или ближайному большему ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ (на вход: искомый ключ, на выходе: найденный ключ).</param>
    /// <param name="data">Значение.</param>
    /// <returns>Код ошибки (0 при успехе, MDBX_NOTFOUND если не найдено).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get_equal_or_great", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxGetEqualOrGreat(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data);
}
