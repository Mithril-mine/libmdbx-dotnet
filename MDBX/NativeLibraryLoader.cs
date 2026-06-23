using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MDBX.Native;

/// <summary>
/// Загрузчик нативных библиотек libmdbx.
/// </summary>
/// <remarks>
/// Обеспечивает кроссплатформенную загрузку библиотеки mdbx.dll (Windows)
/// или libmdbx.so (Linux).
/// </remarks>
public static class NativeLibraryLoader
{
    /// <summary>
    /// Имя нативной библиотеки для DllImport.
    /// </summary>
    public const string LibraryName = "mdbx";

    /// <summary>
    /// Инициализирует загрузчик нативных библиотек.
    /// </summary>
    /// <remarks>
    /// Устанавливает резолвер для DllImport, который будет искать библиотеку
    /// в стандартных местах для текущей платформы.
    /// Безопасен для многократного вызова — второй и последующие вызовы игнорируются.
    /// </remarks>
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        NativeLibrary.SetDllImportResolver(typeof(NativeLibraryLoader).Assembly, ResolveNativeLibrary);
        _initialized = true;
    }

    /// <summary>
    /// Флаг, указывающий, что резолвер уже зарегистрирован.
    /// </summary>
    private static bool _initialized;

    /// <summary>
    /// Автоматически вызывается при загрузке модуля, чтобы гарантировать
    /// регистрацию резолвера нативных библиотек до первого вызова P/Invoke.
    /// </summary>
    [ModuleInitializer]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Initialization", "CA2255:Module initializer", Justification = "Required to ensure native library resolver is registered before any P/Invoke call")]
    internal static void ModuleInit()
    {
        Initialize();
    }

    /// <summary>
    /// Резолвер для загрузки нативных библиотек.
    /// </summary>
    private static IntPtr ResolveNativeLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        // Проверяем, запрашивается ли наша библиотека
        if (!string.Equals(libraryName, LibraryName, StringComparison.OrdinalIgnoreCase))
        {
            return IntPtr.Zero;
        }

        // Пытаемся загрузить библиотеку стандартным способом
        IntPtr handle = NativeLibrary.Load(libraryName, assembly, searchPath);
        if (handle != IntPtr.Zero)
        {
            return handle;
        }

        // Если стандартная загрузка не удалась, пытаемся загрузить по явному пути
        string baseDir = AppContext.BaseDirectory;
        string? nativeLibPath = GetNativeLibraryPath(baseDir);
        if (nativeLibPath != null && File.Exists(nativeLibPath))
        {
            return NativeLibrary.Load(nativeLibPath);
        }

        return handle;
    }

    /// <summary>
    /// Возвращает путь к нативной библиотеке для текущей платформы.
    /// </summary>
    private static string? GetNativeLibraryPath(string baseDir)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Combine(baseDir, "native", "windows", "x64", "mdbx.dll");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Path.Combine(baseDir, "native", "linux", "x64", "libmdbx.so");
        }

        return null;
    }
}
