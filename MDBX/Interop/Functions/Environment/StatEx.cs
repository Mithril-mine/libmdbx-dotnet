using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_stat_ex(MDBX_env *env, mdbx_stat_t *stat, size_t stat_size)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int StatDelegate(IntPtr env, IntPtr txn, ref EnvironmentStat stat, UIntPtr bytes);

    private static StatDelegate? _statDelegate = null;

    public static EnvironmentStat Stat(IntPtr env, IntPtr txn)
    {
        if (_statDelegate is null)
            throw new InvalidOperationException("Env.Stat called before Library.Load()");
        EnvironmentStat stat = new EnvironmentStat();
        UIntPtr bytes = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(stat));
        int err = _statDelegate(env, txn, ref stat, bytes);
        if (err != 0)
            throw new MdbxException("mdbx_env_stat_ex", err);
        return stat;
    }
}
