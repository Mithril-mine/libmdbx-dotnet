using System.Runtime.InteropServices;

namespace MDBX.Interop;

internal static class Platform
{
    internal static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    internal static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
}
