using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_set_flags(MDBX_env *env, mdbx_env_flags_t flags, int onoff)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetFlagsDelegate(IntPtr env, uint flags, int onoff);

    private static SetFlagsDelegate? _setFlagsDelegate = null;

    public static void SetFlags(IntPtr env, EnvironmentFlag flags, bool onoff)
    {
        if (_setFlagsDelegate is null)
            throw new InvalidOperationException("Env.SetFlags called before Library.Load()");
        int err = _setFlagsDelegate(env, (uint)flags, onoff ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_flags", err);
    }
}
