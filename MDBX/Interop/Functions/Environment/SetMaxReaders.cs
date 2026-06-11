using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_set_maxreaders(MDBX_env *env, mdbx_reader_t readers)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMaxReadersDelegate(IntPtr env, uint readers);

    private static SetMaxReadersDelegate? _setMaxReadersDelegate = null;

    public static void SetMaxReaders(IntPtr env, uint readers)
    {
        if (_setMaxReadersDelegate is null)
            throw new InvalidOperationException("Env.SetMaxReaders called before Library.Load()");
        int err = _setMaxReadersDelegate(env, readers);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_maxreaders", err);
    }
}
