using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_close(MDBX_env *env)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CloseDelegate(IntPtr env);

    private static CloseDelegate? _closeDelegate = null;

    internal static void Close(IntPtr env)
    {
        if (_closeDelegate is null)
            throw new InvalidOperationException("Env.Close called before Library.Load()");
        int err = _closeDelegate(env);
        if (err != 0)
            throw new MdbxException("mdbx_env_close", err);
    }
}
