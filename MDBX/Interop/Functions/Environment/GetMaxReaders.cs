using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_get_maxreaders(MDBX_env *env, mdbx_reader_t *readers)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetMaxReadersDelegate(IntPtr env, out uint readers);

    private static GetMaxReadersDelegate? _getMaxReadersDelegate = null;

    public static int GetMaxReaders(IntPtr env)
    {
        if (_getMaxReadersDelegate is null)
            throw new InvalidOperationException("Env.GetMaxReaders called before Library.Load()");
        uint readers;
        int err = _getMaxReadersDelegate(env, out readers);
        if (err != 0)
            throw new MdbxException("mdbx_env_get_maxreaders", err);
        return (int)readers;
    }
}
