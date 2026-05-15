using MDBX.Interop.Models;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class DataBase
{
    /// <summary>
    /// int mdbx_del(MDBX_txn *txn, MDBX_dbi dbi, const MDBX_val *key, MDBX_val *value)
    /// </summary>
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
}
