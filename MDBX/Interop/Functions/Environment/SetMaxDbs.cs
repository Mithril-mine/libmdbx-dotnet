using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_set_maxdbs(MDBX_env *env, mdbx_dbi_t dbs)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMaxDbsDelegate(IntPtr env, [MarshalAs(UnmanagedType.U4)] uint dbs);

    private static SetMaxDbsDelegate? _setMaxDbsDelegate = null;

    public static void SetMaxDBs(IntPtr env, uint dbs)
    {
        if (_setMaxDbsDelegate is null)
            throw new InvalidOperationException("Env.SetMaxDBs called before Library.Load()");
        int err = _setMaxDbsDelegate(env, dbs);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_maxdbs", err);
    }
}
