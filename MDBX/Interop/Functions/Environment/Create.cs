using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal partial class MdbxInteropEnvironment
{
    /// <summary>
    /// int mdbx_env_create(MDBX_env **penv)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CreateDelegate(out IntPtr env);

    private static CreateDelegate? _createDelegate = null;

    internal static IntPtr Create()
    {
        if (_createDelegate is null)
            throw new InvalidOperationException("Env.Create called before Library.Load()");
        IntPtr ptr;
        int err = _createDelegate(out ptr);
        if (err != 0)
            throw new MdbxException("mdbx_env_create", err);
        return ptr;
    }
}
