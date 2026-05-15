using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_set_mapsize(MDBX_env *env, mdbx_mmap_t size)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMapSizeDelegate(IntPtr env, UIntPtr size);

    private static SetMapSizeDelegate? _setMapSizeDelegate = null;

    public static void SetMapSize(IntPtr env, uint size)
    {
        if (_setMapSizeDelegate is null)
            throw new InvalidOperationException("Env.SetMapSize called before Library.Load()");
        int err = _setMapSizeDelegate(env, UIntPtr.Add(UIntPtr.Zero, (int)size));
        if (err != 0)
            throw new MdbxException("mdbx_env_set_mapsize", err);
    }
}
