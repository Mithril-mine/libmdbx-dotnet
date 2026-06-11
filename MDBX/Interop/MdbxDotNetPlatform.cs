using System.Runtime.InteropServices;

namespace MDBX.Interop;

internal enum MdbxDotNetSupportedPlatform
{
    Unknown = -1,
    Windows = 0,
    Linux = 1
}

internal static class MdbxDotNetPlatform
{
    internal static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    internal static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    internal static MdbxDotNetSupportedPlatform GetPlatform()
    {
        if (IsLinux) return MdbxDotNetSupportedPlatform.Linux;

        if (IsWindows) return MdbxDotNetSupportedPlatform.Windows;

        return MdbxDotNetSupportedPlatform.Unknown;
    }

    internal static readonly string ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();
}
