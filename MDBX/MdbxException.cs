using System.Runtime.InteropServices;

namespace MDBX;

/// <summary>
/// Исключение, возникающее при ошибках libmdbx.
/// </summary>
public unsafe sealed class MdbxException : Exception
{
    /// <summary>
    /// Код ошибки libmdbx.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Инициализирует новое исключение с кодом ошибки.
    /// </summary>
    /// <param name="errorCode">Код ошибки libmdbx.</param>
    public MdbxException(int errorCode)
        : base(GetErrorMessage(errorCode))
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Инициализирует новое исключение с кодом ошибки и сообщением.
    /// </summary>
    /// <param name="errorCode">Код ошибки libmdbx.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    public MdbxException(int errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Возвращает строковое описание кода ошибки libmdbx.
    /// </summary>
    private static string GetErrorMessage(int errorCode)
    {
        byte* ptr = Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerror(errorCode);
        if (ptr == null)
            return $"MDBX error {errorCode}";
        return Marshal.PtrToStringUTF8((IntPtr)ptr)!;
    }
}
