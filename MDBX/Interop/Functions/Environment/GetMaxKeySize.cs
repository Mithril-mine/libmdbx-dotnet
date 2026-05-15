using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Environment
{
    /// <summary>
    /// int mdbx_env_get_maxkeysize(MDBX_env *env)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetMaxKeySizeDelegate(IntPtr env);

    private static GetMaxKeySizeDelegate? _getMaxKeySizeDelegate = null;

    public static int GetMaxKeySize(IntPtr env)
    {
        if (_getMaxKeySizeDelegate is null)
            throw new InvalidOperationException("Env.GetMaxKeySize called before Library.Load()");
        int result = _getMaxKeySizeDelegate(env);
        if (result < 0)
        {
            throw new MdbxException("mdbx_env_get_maxkeysize", Constants.MDBX_INVALID);
        }
        return result;
    }
}
