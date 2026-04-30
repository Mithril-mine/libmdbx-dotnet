using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    /// <summary>
    /// Вспомогательные функции для взаимодействия с библиотекой MDBX.
    /// </summary>
    internal static class Misc
    {
        /// <summary>
        /// Получает строковое описание ошибки MDBX по её коду.
        /// </summary>
        /// <param name="err">Код ошибки.</param>
        /// <returns>Строка с описанием ошибки или null.</returns>
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

        /// <summary>
        /// Привязывает делегат для функции mdbx_strerror.
        /// </summary>
        internal static void Bind()
        {
            _stringErrorDelegate = NativeLibraryLoader.GetProcAddress<StringErrorDelegate>("mdbx_strerror");
        }
    }
}
