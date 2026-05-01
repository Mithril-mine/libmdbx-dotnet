using System.Runtime.InteropServices;

namespace MDBX.Interop;

internal class LibraryLoadInfo
{
    internal required string FileName {get;set;}
    internal required string Platform { get; set; }
}

/// <summary>
/// Загрузка и привязка нативной библиотеки MDBX.
/// </summary>
internal static partial class NativeLibraryLoader
{
    /// <summary>
    /// Функция dlopen для Linux.
    /// </summary>
    /// <param name="filename">Имя файла библиотеки.</param>
    /// <param name="flags">Флаги загрузки.</param>
    /// <returns>Указатель на загруженную библиотеку.</returns>
    [DllImport("libdl.so")]
    private static extern IntPtr dlopen(string filename, int flags);

    /// <summary>
    /// Функция dlsym для Linux.
    /// </summary>
    /// <param name="handle">Дескриптор библиотеки.</param>
    /// <param name="symbol">Имя символа.</param>
    /// <returns>Указатель на символ.</returns>
    [DllImport("libdl.so")]
    private static extern IntPtr dlsym(IntPtr handle, string symbol);

    /// <summary>
    /// Константа флага RTLD_NOW для dlopen.
    /// </summary>
    private const int RTLD_NOW = 2;

    /// <summary>
    /// Функция LoadLibrary для Windows.
    /// </summary>
    /// <param name="lpFileName">Имя файла библиотеки.</param>
    /// <returns>Указатель на загруженную библиотеку.</returns>
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
    private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

    /// <summary>
    /// Функция GetProcAddress для Windows.
    /// </summary>
    /// <param name="hModule">Дескриптор модуля.</param>
    /// <param name="procName">Имя процедуры.</param>
    /// <returns>Указатель на процедуру.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    /// <summary>
    /// Функция SetDefaultDllDirectories для Windows.
    /// </summary>
    /// <param name="DirectoryFlags">Флаги директорий.</param>
    /// <returns>true в случае успеха.</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetDefaultDllDirectories(int DirectoryFlags);

    /// <summary>
    /// Константа флага LOAD_LIBRARY_SEARCH_DEFAULT_DIRS.
    /// </summary>
    private const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

    /// <summary>
    /// Указатель на загруженную библиотеку.
    /// </summary>
    private static IntPtr _libPtr = IntPtr.Zero;

    /// <summary>
    /// Статический конструктор. Инициализирует безопасность загрузки библиотек на Windows.
    /// </summary>
    static NativeLibraryLoader()
    {
        try
        {
            SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
        }
        catch
        {
            // Игнорировать, если не поддерживается ОС
        }
    }

    /// <summary>
    /// Получает адрес函数ы из загруженной библиотеки.
    /// </summary>
    /// <typeparam name="T">Тип делегата.</typeparam>
    /// <param name="procName">Имя функции.</param>
    /// <returns>Делегат для native-функции.</returns>
    /// <exception cref="BadImageFormatException">Не удалось найти функцию.</exception>
    internal static T GetProcAddress<T>(string procName) where T : Delegate
    {
        IntPtr ptr = IntPtr.Zero;

        if (MdbxDotNetPlatform.IsWindows) ptr = GetProcAddress(_libPtr, procName);

        if (MdbxDotNetPlatform.IsLinux) ptr = dlsym(_libPtr, procName);

        if (!ptr.Equals(IntPtr.Zero)) return Marshal.GetDelegateForFunctionPointer<T>(ptr);

        throw new BadImageFormatException($"MDBX failed to bind '{procName}' function.");
    }

    /// <summary>
    /// Получить информацию о платформе и библиотеке libmdbx.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    private static LibraryLoadInfo GetLibraryLoadInfo()
    {
        switch (MdbxDotNetPlatform.GetPlatform())
        {
            case MdbxDotNetSupportedPlatform.Windows:
                return new LibraryLoadInfo() { FileName = "mdbx.dll", Platform = "windows" };
            case MdbxDotNetSupportedPlatform.Linux:
                return new LibraryLoadInfo() { FileName = "libmdbx.so", Platform = "linux" };
            default: throw new PlatformNotSupportedException($"Unsupported OS platform : {RuntimeInformation.OSDescription}");
        }
    }

    private static string GetLibraryFullPath(LibraryLoadInfo info)
    {
        var filePath =  Path.Combine(
          AppContext.BaseDirectory
          , "native"
          , info.Platform.ToLowerInvariant()
          , MdbxDotNetPlatform.ProcessArchitecture
          , info.FileName
        );

        if (filePath == null || filePath == string.Empty) throw new FileNotFoundException("MDBX-DOTNET File path unspecified");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"MDBX cannot find the library at {filePath}", filePath);

        return filePath;
    }

    /// <summary>
    /// Загружает нативную библиотеку MDBX для текущей платформы.
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">Платформа не поддерживается.</exception>
    /// <exception cref="FileNotFoundException">Библиотека не найдена или не загружена.</exception>
    internal static void Load()
    {
        var info = GetLibraryLoadInfo();

        string filepath = GetLibraryFullPath(info);

        switch (MdbxDotNetPlatform.GetPlatform())
        {
            case MdbxDotNetSupportedPlatform.Windows:
                _libPtr = LoadLibrary(filepath);
                break;
            case MdbxDotNetSupportedPlatform.Linux:
                _libPtr = dlopen(filepath, RTLD_NOW);
                break;
            default: throw new FileNotFoundException($"MDBX failed to load library at {filepath}", filepath);
        }

        Misc.Bind();
        Environment.Bind();
        Transaction.Bind();
        DataBase.Bind();
        Cursor.Bind();
    }
}
