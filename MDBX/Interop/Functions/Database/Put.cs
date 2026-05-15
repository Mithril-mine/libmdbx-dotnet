using MDBX.Interop.Models;
using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class DataBase
{
    /// <summary>
    /// int mdbx_put(MDBX_txn *txn, MDBX_dbi dbi, const MDBX_val *key, const MDBX_val *val, mdbx_put_flags_t flags)
    /// </summary>
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
}
