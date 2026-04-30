using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

/// <summary>
/// Взаимодействие с нативными функциями MDBX для операций с базой данных (DBI).
/// </summary>
internal static class DataBase
{
    /// <summary>
    /// Делегат для функции mdbx_dbi_open.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="name">Имя базы данных.</param>
    /// <param name="flags">Флаги базы данных.</param>
    /// <param name="dbi">Возвращаемый дескриптор базы данных.</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int OpenDelegate(IntPtr txn
        , [MarshalAs(UnmanagedType.LPStr)] string name
        , [MarshalAs(UnmanagedType.U4)] int flags
        , out uint dbi);

    private static OpenDelegate? _openDelegate = null;

    /// <summary>
    /// Открывает (создаёт) базу данных в транзакции.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="name">Имя базы данных (пустая строка для безымянной БД).</param>
    /// <param name="options">Опции базы данных.</param>
    /// <returns>Дескриптор базы данных.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при открытии базы данных.</exception>
    internal static uint Open(IntPtr txn, string name, DatabaseOption options)
    {
        if (_openDelegate is null)
            throw new InvalidOperationException("Dbi.Open called before Library.Load()");
        uint dbi;
        int err = _openDelegate(txn, name, (int)options, out dbi);
        if (err != 0)
            throw new MdbxException("mdbx_dbi_open", err);
        return dbi;
    }

    /// <summary>
    /// Делегат для функции mdbx_dbi_close.
    /// </summary>
    /// <param name="env">Указатель на среду.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CloseDelegate(IntPtr env, uint dbi);

    private static CloseDelegate? _closeDelegate = null;

    /// <summary>
    /// Закрывает дескриптор базы данных.
    /// </summary>
    /// <param name="env">Указатель на среду.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при закрытии базы данных.</exception>
    internal static void Close(IntPtr env, uint dbi)
    {
        if (_closeDelegate is null)
            throw new InvalidOperationException("Dbi.Close called before Library.Load()");
        int err = _closeDelegate(env, dbi);
        if (err != 0)
            throw new MdbxException("mdbx_dbi_close", err);
    }

    /// <summary>
    /// Делегат для функции mdbx_put.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="flags">Флаги операции.</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PutDelegate(IntPtr txn
        , uint dbi
        , ref DataBaseValue key
        , ref DataBaseValue value
        , [MarshalAs(UnmanagedType.U4)] uint flags);

    private static PutDelegate? _putDelegate = null;

    /// <summary>
    /// Добавляет или обновляет запись в базе данных.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="options">Опции операции.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции put.</exception>
    internal static void Put(IntPtr txn, uint dbi, DataBaseValue key, DataBaseValue value, PutOption options)
    {
        if (_putDelegate is null)
            throw new InvalidOperationException("Dbi.Put called before Library.Load()");
        int err = _putDelegate(txn, dbi, ref key, ref value, (uint)options);
        if (err != 0)
            throw new MdbxException("mdbx_put", err);
    }

    /// <summary>
    /// Делегат для функции mdbx_del.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение (не используется, должно быть null).</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DelDelegate(IntPtr txn
        , uint dbi
        , ref DataBaseValue key
        , IntPtr value);

    private static DelDelegate? _delDelegate = null;

    /// <summary>
    /// Удаляет запись из базы данных по ключу.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение (не используется).</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции delete.</exception>
    internal static void Del(IntPtr txn, uint dbi, DataBaseValue key, IntPtr value)
    {
        if (_delDelegate is null)
            throw new InvalidOperationException("Dbi.Del called before Library.Load()");
        int err = _delDelegate(txn, dbi, ref key, value);
        if (err != 0)
            throw new MdbxException("mdbx_del", err);
    }

    /// <summary>
    /// Делегат для функции mdbx_get.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Возвращаемое значение.</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetDelegate(IntPtr txn
        , uint dbi
        , ref DataBaseValue key
        , ref DataBaseValue value);

    private static GetDelegate? _getDelegate = null;

    /// <summary>
    /// Получает значение из базы данных по ключу.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="key">Ключ.</param>
    /// <returns>Структура с данными.</returns>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции get.</exception>
    internal static DataBaseValue Get(IntPtr txn, uint dbi, DataBaseValue key)
    {
        if (_getDelegate is null)
            throw new InvalidOperationException("Dbi.Get called before Library.Load()");
        DataBaseValue value = new DataBaseValue();
        int err = _getDelegate(txn, dbi, ref key, ref value);
        if (err != 0)
            throw new MdbxException("mdbx_get", err);
        return value;
    }

    /// <summary>
    /// Делегат для функции mdbx_drop.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="del">Удалить запись полностью.</param>
    /// <returns>Код ошибки или 0 при успехе.</returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DropDelegate(IntPtr txn, uint dbi, int del);

    private static DropDelegate? _dropDelegate = null;

    /// <summary>
    /// Удаляет базу данных или очищает её содержимое.
    /// </summary>
    /// <param name="txn">Указатель на транзакцию.</param>
    /// <param name="dbi">Дескриптор базы данных.</param>
    /// <param name="del">Если true - удаляет базу полностью, иначе только очищает.</param>
    /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
    /// <exception cref="MdbxException">Ошибка при операции drop.</exception>
    internal static void Drop(IntPtr txn, uint dbi, bool del)
    {
        if (_dropDelegate is null)
            throw new InvalidOperationException("Dbi.Drop called before Library.Load()");
        int err = _dropDelegate(txn, dbi, del ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_drop", err);
    }

    /// <summary>
    /// Привязывает все делегаты DBI к нативным функциям библиотеки MDBX.
    /// </summary>
    internal static void Bind()
    {
        _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>("mdbx_dbi_open");
        _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>("mdbx_dbi_close");
        _putDelegate = NativeLibraryLoader.GetProcAddress<PutDelegate>("mdbx_put");
        _getDelegate = NativeLibraryLoader.GetProcAddress<GetDelegate>("mdbx_get");
        _delDelegate = NativeLibraryLoader.GetProcAddress<DelDelegate>("mdbx_del");
        _dropDelegate = NativeLibraryLoader.GetProcAddress<DropDelegate>("mdbx_drop");
    }
}
