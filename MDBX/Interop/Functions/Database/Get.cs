using MDBX.Interop.Models;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropDataBase
{
    /// <summary>
    /// int mdbx_get(MDBX_txn *txn, MDBX_dbi dbi, const MDBX_val *key, MDBX_val *data)
    /// </summary>
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
}
