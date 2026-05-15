using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

partial class Misc
{
    /// <summary>
    /// const char *mdbx_strerror(int errnum)
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr StringErrorDelegate(int err);

    private static StringErrorDelegate? _stringErrorDelegate = null;

    /// <summary>
    /// Возвращает текстовое описание кода ошибки MDBX.
    /// </summary>
    /// <param name="err">Код ошибки.</param>
    /// <returns>Строка с описанием ошибки или null.</returns>
    internal static string? StringError(int err)
    {
        IntPtr ptr = _stringErrorDelegate!(err);
        return Marshal.PtrToStringAnsi(ptr);
    }
}
