using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_close_ex(MDBX_env *env, int dont_sync)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CloseExDelegate(IntPtr env, [MarshalAs(UnmanagedType.I4)] int dont_sync);

    private static CloseExDelegate? _closeExDelegate = null;

    internal static void Close(IntPtr env, bool dontSync)
    {
        if (_closeExDelegate is null)
            throw new InvalidOperationException("Env.Close called before Library.Load()");
        int err = _closeExDelegate(env, dontSync ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_close_ex", err);
    }
}
