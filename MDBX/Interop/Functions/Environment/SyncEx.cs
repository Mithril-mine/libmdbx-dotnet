using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks.Dataflow;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// Сброс буферов данных среды на диск.
    /// Если среда не была открыта с флагами отсутствия синхронизации (MDBX_NOMETASYNC, MDBX_SAFE_NOSYNC и MDBX_UTTERLY_NOSYNC), 
    /// то данные всегда записываются на диск и сбрасываются в него при вызове mdbx_txn_commit(). 
    /// В противном случае можно вызвать mdbx_env_sync(), чтобы вручную записать на диск несинхронизированные данные и сбросить их.
    /// 
    /// Кроме того, функция mdbx_env_sync_ex() с аргументом force=false может использоваться для обеспечения 
    /// режима опроса при отложенной/асинхронной синхронизации в сочетании с функциями mdbx_env_set_syncbytes() и/или mdbx_env_set_syncperiod().
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SyncExDelegate(IntPtr env, int force, int nonblock);

    private static SyncExDelegate? _syncExDelegate = null;

    public static void SyncEx(IntPtr env, bool force, bool nonblock)
    {
        if (_syncExDelegate is null) throw new InvalidOperationException("Env.Sync called before Library.Load()");
        
        int err = _syncExDelegate(env, force ? 1 : 0, nonblock ? 1 : 0);
       
        if (err != 0) throw new MdbxException("mdbx_env_sync_ex", err);
    }
}
