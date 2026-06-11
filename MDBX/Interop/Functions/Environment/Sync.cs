using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_sync(MDBX_env *env, int force)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SyncDelegate(IntPtr env, int force);

    private static SyncDelegate? _syncDelegate = null;

    public static void Sync(IntPtr env, bool force)
    {
        if (_syncDelegate is null)
            throw new InvalidOperationException("Env.Sync called before Library.Load()");
        int err = _syncDelegate(env, force ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_sync", err);
    }
}
