using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_info_ex(const MDBX_env *env, const MDBX_txn *txn, MDBX_envinfo *info, size_t bytes)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int InfoExDelegate(IntPtr env, IntPtr txn, ref EnvironmentInfo info, UIntPtr size_t);

    private static InfoExDelegate? _infoExDelegate = null;

    public static EnvironmentInfo InfoEx(IntPtr env, IntPtr txn)
    {
        if (_infoExDelegate is null) throw new InvalidOperationException("Env.InfoEx called before Library.Load()");

        EnvironmentInfo info = new();

        UIntPtr size_t = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(info));

        int err = _infoExDelegate(env, txn, ref info, size_t);

        if (err != 0) throw new MdbxException("mdbx_env_info_ex", err);

        return info;
    }
}
