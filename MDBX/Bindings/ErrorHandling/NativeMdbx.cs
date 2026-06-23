using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.ErrorHandling;

/// <summary>
/// Частичный класс для нативных методов обработки ошибок libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    static NativeMdbx()
    {
        NativeLibraryLoader.Initialize();
    }

    /// <summary>
    /// Возвращает строковое описание кода ошибки libmdbx.
    /// </summary>
    /// <param name="errnum">Код ошибки.</param>
    /// <returns>Указатель на статическую строку с описанием ошибки.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_strerror", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxStrerror(int errnum);

    /// <summary>
    /// Копирует строковое описание кода ошибки в предоставленный буфер.
    /// </summary>
    /// <param name="errnum">Код ошибки.</param>
    /// <param name="buf">Буфер для копирования строки.</param>
    /// <param name="buflen">Размер буфера в байтах.</param>
    /// <returns>Указатель на буфер с строкой описания ошибки.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_strerror_r", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxStrerrorR(int errnum, byte* buf, nuint buflen);

    /// <summary>
    /// Возвращает строковое описание кода ошибки libmdbx (версия только для ошибок libmdbx).
    /// </summary>
    /// <param name="errnum">Код ошибки.</param>
    /// <returns>Указатель на статическую строку с описанием ошибки.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_liberr2str", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxLiberr2Str(int errnum);

    /// <summary>
    /// Возвращает строковое описание кода ошибки в кодировке ANSI/OEM (Windows).
    /// </summary>
    /// <param name="errnum">Код ошибки.</param>
    /// <returns>Указатель на статическую строку с описанием ошибки.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_strerror_ANSI2OEM", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxStrerrorAnsi2Oem(int errnum);

    /// <summary>
    /// Копирует строковое описание кода ошибки в предоставленный буфер (ANSI/OEM версия).
    /// </summary>
    /// <param name="errnum">Код ошибки.</param>
    /// <param name="buf">Буфер для копирования строки.</param>
    /// <param name="buflen">Размер буфера в байтах.</param>
    /// <returns>Указатель на буфер с строкой описания ошибки.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_strerror_r_ANSI2OEM", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxStrerrorRAnsi2Oem(int errnum, byte* buf, nuint buflen);

    /// <summary>
    /// Функция аварийного завершения при ошибке (assert).
    /// </summary>
    /// <param name="msg">Сообщение об ошибке.</param>
    /// <param name="func">Имя функции, в которой произошла ошибка.</param>
    /// <param name="line">Номер строки.</param>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_assert_fail", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MdbxAssertFail(byte* msg, byte* func, uint line);

    /// <summary>
    /// Устанавливает функцию обработки паники (критических ошибок).
    /// </summary>
    /// <param name="func">Указатель на функцию обработки паники.</param>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_set_panic", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MdbxSetPanic(delegate* unmanaged[Cdecl]<byte*, byte*, uint, void*, void> func);
}
