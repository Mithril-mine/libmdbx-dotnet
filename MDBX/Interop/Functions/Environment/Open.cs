using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_open(MDBX_env *env, const char *path, mdbx_env_flags_t flags, mdbx_mode_t mode)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int OpenDelegate(
        IntPtr env,
        [MarshalAs(UnmanagedType.LPStr)] string path,
        [MarshalAs(UnmanagedType.U4)] int flags,
        [MarshalAs(UnmanagedType.I4)] int mode);

    private static OpenDelegate? _openDelegate = null;

    public static void Open(IntPtr env, string path, EnvironmentFlag flags, int mode)
    {
        if (_openDelegate is null)
            throw new InvalidOperationException("Env.Open called before Library.Load()");
        int err = _openDelegate(env, path, (int)flags, mode);
        if (err != 0)
            throw new MdbxException("mdbx_env_open", err);
    }
}
