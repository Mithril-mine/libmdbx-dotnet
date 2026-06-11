using MDBX.Interop.Functions;

namespace MDBX.Interop;

internal static partial class MdbxInteropMisc
{
    internal static void Bind()
    {
        _stringErrorDelegate = NativeLibraryLoader.GetProcAddress<StringErrorDelegate>(MdbxFunctions.Misc.StringError);
    }
}
