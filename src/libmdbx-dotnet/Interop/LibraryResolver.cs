using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Libmdbx.Interop
{
    /// <summary>
    /// Registers a custom <see cref="DllImportResolver"/> so that the
    /// <c>libmdbx</c> native library bundled alongside this assembly is
    /// loaded instead of whatever the OS default search would find.
    /// </summary>
    internal static class LibraryResolver
    {
        private static bool _registered;
        private static readonly object _lock = new();

        internal static void EnsureRegistered()
        {
            if (_registered) return;
            lock (_lock)
            {
                if (_registered) return;
                NativeLibrary.SetDllImportResolver(
                    typeof(LibraryResolver).Assembly,
                    Resolve);
                _registered = true;
            }
        }

        private static IntPtr Resolve(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            if (!string.Equals(libraryName, NativeMethods.LibraryName,
                    StringComparison.OrdinalIgnoreCase))
                return IntPtr.Zero;

            // Build the runtime-specific relative path, e.g.:
            //   runtimes/linux-x64/native/libmdbx.so
            string rid = GetRuntimeId();
            string nativeFileName = GetNativeFileName();

            string assemblyDir = Path.GetDirectoryName(assembly.Location) ?? ".";

            string candidatePath = Path.Combine(
                assemblyDir, "runtimes", rid, "native", nativeFileName);

            if (File.Exists(candidatePath)
                && NativeLibrary.TryLoad(candidatePath, out IntPtr handle))
                return handle;

            // Fallback: let the runtime use its default search
            if (NativeLibrary.TryLoad(libraryName, assembly, searchPath, out handle))
                return handle;

            throw new DllNotFoundException(
                $"Cannot find the native libmdbx library. " +
                $"Tried: {candidatePath}");
        }

        private static string GetRuntimeId()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.X86 => "win-x86",
                    Architecture.Arm64 => "win-arm64",
                    _ => throw new PlatformNotSupportedException()
                };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "linux-x64",
                    Architecture.Arm64 => "linux-arm64",
                    Architecture.Arm => "linux-arm",
                    _ => throw new PlatformNotSupportedException()
                };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "osx-x64",
                    Architecture.Arm64 => "osx-arm64",
                    _ => throw new PlatformNotSupportedException()
                };

            throw new PlatformNotSupportedException(
                $"Unsupported platform: {RuntimeInformation.OSDescription}");
        }

        private static string GetNativeFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "mdbx.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "libmdbx.dylib";
            return "libmdbx.so";
        }
    }
}
