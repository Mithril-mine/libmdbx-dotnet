using MDBX.Interop.Functions;

namespace MDBX.Interop;

internal static partial class MdbxInteropEnvironment
{
    internal static void Bind()
    {
        _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>(MdbxFunctions.Environment.Close);
        _closeExDelegate = NativeLibraryLoader.GetProcAddress<CloseExDelegate>(MdbxFunctions.Environment.CloseEx);
        _createDelegate = NativeLibraryLoader.GetProcAddress<CreateDelegate>(MdbxFunctions.Environment.Create);
        _getFlagsDelegate = NativeLibraryLoader.GetProcAddress<GetFlagsDelegate>(MdbxFunctions.Environment.GetFlags);
        _getMaxKeySizeDelegate = NativeLibraryLoader.GetProcAddress<GetMaxKeySizeDelegate>(MdbxFunctions.Environment.GetMaxKeySize);
        _getMaxReadersDelegate = NativeLibraryLoader.GetProcAddress<GetMaxReadersDelegate>(MdbxFunctions.Environment.GetMaxReaders);
        _infoExDelegate = NativeLibraryLoader.GetProcAddress<InfoExDelegate>(MdbxFunctions.Environment.InfoEx);
        _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>(MdbxFunctions.Environment.Open);
        _setFlagsDelegate = NativeLibraryLoader.GetProcAddress<SetFlagsDelegate>(MdbxFunctions.Environment.SetFlags);
        _setMapSizeDelegate = NativeLibraryLoader.GetProcAddress<SetMapSizeDelegate>(MdbxFunctions.Environment.SetMapSize);
        _setMaxDbsDelegate = NativeLibraryLoader.GetProcAddress<SetMaxDbsDelegate>(MdbxFunctions.Environment.SetMaxDbs);
        _setMaxReadersDelegate = NativeLibraryLoader.GetProcAddress<SetMaxReadersDelegate>(MdbxFunctions.Environment.SetMaxReaders);
        _statDelegate = NativeLibraryLoader.GetProcAddress<StatDelegate>(MdbxFunctions.Environment.StatEx);
        _syncDelegate = NativeLibraryLoader.GetProcAddress<SyncDelegate>(MdbxFunctions.Environment.Sync);
        _syncExDelegate = NativeLibraryLoader.GetProcAddress<SyncExDelegate>(MdbxFunctions.Environment.SyncEx);
    }
}
