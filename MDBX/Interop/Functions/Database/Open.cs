using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropDataBase
{
    /// <summary>
    /// int mdbx_dbi_open(MDBX_txn *txn, const char *name, mdbx_dbi_flags_t flags, MDBX_dbi *dbi)
    /// </summary>
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
}
