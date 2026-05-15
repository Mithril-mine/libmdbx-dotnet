using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_get_flags(MDBX_env *env, mdbx_env_flags_t *flags)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetFlagsDelegate(IntPtr env, out uint flags);

    private static GetFlagsDelegate? _getFlagsDelegate = null;

    public static EnvironmentFlag GetFlags(IntPtr env)
    {
        if (_getFlagsDelegate is null)
            throw new InvalidOperationException("Env.GetFlags called before Library.Load()");
        uint flags;
        int err = _getFlagsDelegate(env, out flags);
        if (err != 0)
            throw new MdbxException("mdbx_env_get_flags", err);
        return (EnvironmentFlag)flags;
    }
}
