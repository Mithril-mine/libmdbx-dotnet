using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropDataBase
{
    /// <summary>
    /// int mdbx_dbi_close(MDBX_env *env, MDBX_dbi dbi)
    /// </summary>
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
}
