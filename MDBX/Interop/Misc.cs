using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Misc
    {
        /// <summary>
        /// Получает строковое описание ошибки в базе данных MDBX.
        /// </summary>
        /// <param name="err">Код ошибки для получения описания.</param>
        /// <returns>Описание ошибки или null.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr StringErrorDelegate(int err);

        private static StringErrorDelegate? _stringErrorDelegate = null;

        internal static string? StringError(int err)
        {
            IntPtr ptr = _stringErrorDelegate!(err);
            return Marshal.PtrToStringAnsi(ptr);
        }



        internal static void Bind()
        {
            _stringErrorDelegate = Library.GetProcAddress<StringErrorDelegate>("mdbx_strerror");
        }

    }
}
