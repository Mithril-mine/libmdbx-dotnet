using MDBX.Interop.Functions;

namespace MDBX.Interop;

internal static partial class MdbxInteropDataBase
{
    internal static void Bind()
    {
        _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>(MdbxFunctions.Database.Open);
        _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>(MdbxFunctions.Database.Close);
        _putDelegate = NativeLibraryLoader.GetProcAddress<PutDelegate>(MdbxFunctions.Database.Put);
        _getDelegate = NativeLibraryLoader.GetProcAddress<GetDelegate>(MdbxFunctions.Database.Get);
        _delDelegate = NativeLibraryLoader.GetProcAddress<DelDelegate>(MdbxFunctions.Database.Del);
        _dropDelegate = NativeLibraryLoader.GetProcAddress<DropDelegate>(MdbxFunctions.Database.Drop);
    }
}
