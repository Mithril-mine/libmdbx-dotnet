using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop;

internal static class Environment
{
    /// <summary>
    /// int mdbx_env_create(MDBX_env **penv)
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CreateDelegate(out IntPtr env);

    private static CreateDelegate? _createDelegate = null;

    internal static IntPtr Create()
    {
        if (_createDelegate is null)
            throw new InvalidOperationException("Env.Create called before Library.Load()");
        IntPtr ptr;
        int err = _createDelegate(out ptr);
        if (err != 0)
            throw new MdbxException("mdbx_env_create", err);
        return ptr;
    }

    /// <summary>
    /// int mdbx_env_close(MDBX_env *env)
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CloseDelegate(IntPtr env);

    private static CloseDelegate? _closeDelegate = null;

    internal static void Close(IntPtr env)
    {
        if (_closeDelegate is null)
            throw new InvalidOperationException("Env.Close called before Library.Load()");
        int err = _closeDelegate(env);
        if (err != 0)
            throw new MdbxException("mdbx_env_close", err);
    }


    /// <summary>
    /// int mdbx_env_close_ex(MDBX_env *env, int dont_sync);
    /// </summary>
    /// <param name="env"></param>
    /// <param name="dont_sync"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CloseExDelegate(IntPtr env, [MarshalAs(UnmanagedType.I4)] int dont_sync);

    private static CloseExDelegate? _closeExDelegate = null;

    internal static void Close(IntPtr env, bool dontSync)
    {
        if (_closeExDelegate is null)
            throw new InvalidOperationException("Env.Close called before Library.Load()");
        int err = _closeExDelegate(env, dontSync ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_close_ex", err);
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int OpenDelegate(IntPtr env
        , [MarshalAs(UnmanagedType.LPStr)] string path
        , [MarshalAs(UnmanagedType.U4)] int flags
        , [MarshalAs(UnmanagedType.I4)] int mode
        );
    private static OpenDelegate? _openDelegate = null;

    public static void Open(IntPtr env, string path, EnvironmentFlag flags, int mode)
    {
        if (_openDelegate is null)
            throw new InvalidOperationException("Env.Open called before Library.Load()");
        int err = _openDelegate(env, path, (int)flags, mode);
        if (err != 0)
            throw new MdbxException("mdbx_env_open", err);
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMaxDbsDelegate(IntPtr env
        , [MarshalAs(UnmanagedType.U4)] uint dbs
        );
    private static SetMaxDbsDelegate? _setMaxDbsDelegate = null;

    public static void SetMaxDBs(IntPtr env, uint dbs)
    {
        if (_setMaxDbsDelegate is null)
            throw new InvalidOperationException("Env.SetMaxDBs called before Library.Load()");
        int err = _setMaxDbsDelegate(env, dbs);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_maxdbs", err);
    }




    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int StatDelegate(IntPtr env
        , ref EnvironmentStat stat
        , UIntPtr bytes
        );
    private static StatDelegate? _statDelegate = null;

    public static EnvironmentStat Stat(IntPtr env)
    {
        if (_statDelegate is null)
            throw new InvalidOperationException("Env.Stat called before Library.Load()");
        EnvironmentStat stat = new EnvironmentStat();
        UIntPtr bytes = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(stat));
        int err = _statDelegate(env, ref stat, bytes);
        if (err != 0)
            throw new MdbxException("mdbx_env_stat_ex", err);
        return stat;
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int InfoDelegate(IntPtr env
        , ref EnvironmentInfo info
        , UIntPtr bytes
        );
    private static InfoDelegate? _infoDelegate = null;

    public static EnvironmentInfo Info(IntPtr env)
    {
        if (_infoDelegate is null)
            throw new InvalidOperationException("Env.Info called before Library.Load()");
        EnvironmentInfo info = new EnvironmentInfo();
        UIntPtr bytes = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(info));
        int err = _infoDelegate(env, ref info, bytes);
        if (err != 0)
            throw new MdbxException("mdbx_env_info", err);
        return info;
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SyncDelegate(IntPtr env, int force);
    private static SyncDelegate? _syncDelegate = null;

    public static void Sync(IntPtr env, bool force)
    {
        if (_syncDelegate is null)
            throw new InvalidOperationException("Env.Sync called before Library.Load()");
        int err = _syncDelegate(env, force ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_sync", err);
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetFlagsDelegate(IntPtr env, uint flags, int onoff);
    private static SetFlagsDelegate? _setFlagsDelegate = null;

    public static void SetFlags(IntPtr env, EnvironmentFlag flags, bool onoff)
    {
        if (_setFlagsDelegate is null)
            throw new InvalidOperationException("Env.SetFlags called before Library.Load()");
        int err = _setFlagsDelegate(env, (uint)flags, onoff ? 1 : 0);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_flags", err);
    }



    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetFlagsDelegate(IntPtr env, out uint flags);
    private static GetFlagsDelegate? _getFlagsDelegate = null;

    public static EnvironmentFlag GetFlags(IntPtr env)
    {
        if (_getFlagsDelegate is null)
            throw new InvalidOperationException("Env.GetFlags called before Library.Load()");
        uint flags;
        int err = _getFlagsDelegate(env, out flags);
        if (err != 0)
            throw new MdbxException("mdbx_env_get_flags", err);
        return (EnvironmentFlag)flags;
    }


    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMapSizeDelegate(IntPtr env, UIntPtr size);
    private static SetMapSizeDelegate? _setMapSizeDelegate = null;

    public static void SetMapSize(IntPtr env, uint size)
    {
        if (_setMapSizeDelegate is null)
            throw new InvalidOperationException("Env.SetMapSize called before Library.Load()");
        int err = _setMapSizeDelegate(env, UIntPtr.Add(UIntPtr.Zero, (int)size));
        if (err != 0)
            throw new MdbxException("mdbx_env_set_mapsize", err);
    }


    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetMaxReadersDelegate(IntPtr env, uint readers);
    private static SetMaxReadersDelegate? _setMaxReadersDelegate = null;

    public static void SetMaxReaders(IntPtr env, uint readers)
    {
        if (_setMaxReadersDelegate is null)
            throw new InvalidOperationException("Env.SetMaxReaders called before Library.Load()");
        int err = _setMaxReadersDelegate(env, readers);
        if (err != 0)
            throw new MdbxException("mdbx_env_set_maxreaders", err);
    }

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetMaxReadersDelegate(IntPtr env, out uint readers);
    private static GetMaxReadersDelegate? _getMaxReadersDelegate = null;

    public static int GetMaxReaders(IntPtr env)
    {
        if (_getMaxReadersDelegate is null)
            throw new InvalidOperationException("Env.GetMaxReaders called before Library.Load()");
        uint readers;
        int err = _getMaxReadersDelegate(env, out readers);
        if (err != 0)
            throw new MdbxException("mdbx_env_get_maxreaders", err);
        return (int)readers;
    }


    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetMaxKeySizeDelegate(IntPtr env);
    private static GetMaxKeySizeDelegate? _getMaxKeySizeDelegate = null;

    public static int GetMaxKeySize(IntPtr env)
    {
        if (_getMaxKeySizeDelegate is null)
            throw new InvalidOperationException("Env.GetMaxKeySize called before Library.Load()");
        int result = _getMaxKeySizeDelegate(env);
        if (result < 0)
        {
            throw new MdbxException("mdbx_env_get_maxkeysize", MdbxCode.MDBX_INVALID);
        }
        return result;
    }


    internal static void Bind()
    {
        _createDelegate = NativeLibraryLoader.GetProcAddress<CreateDelegate>(MdbxFunctions.Environment.Create);
        _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>(MdbxFunctions.Environment.Close);
        _closeExDelegate = NativeLibraryLoader.GetProcAddress<CloseExDelegate>(MdbxFunctions.Environment.CloseEx);
        _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>(MdbxFunctions.Environment.Open);
        _statDelegate = NativeLibraryLoader.GetProcAddress<StatDelegate>(MdbxFunctions.Environment.Stat);
        _infoDelegate = NativeLibraryLoader.GetProcAddress<InfoDelegate>(MdbxFunctions.Environment.Info);
        _syncDelegate = NativeLibraryLoader.GetProcAddress<SyncDelegate>(MdbxFunctions.Environment.Sync);
        _setMaxDbsDelegate = NativeLibraryLoader.GetProcAddress<SetMaxDbsDelegate>(MdbxFunctions.Environment.SetMaxDbs);
        _setFlagsDelegate = NativeLibraryLoader.GetProcAddress<SetFlagsDelegate>(MdbxFunctions.Environment.SetFlags);
        _getFlagsDelegate = NativeLibraryLoader.GetProcAddress<GetFlagsDelegate>(MdbxFunctions.Environment.GetFlags);
        _setMapSizeDelegate = NativeLibraryLoader.GetProcAddress<SetMapSizeDelegate>(MdbxFunctions.Environment.SetMapSize);
        _setMaxReadersDelegate = NativeLibraryLoader.GetProcAddress<SetMaxReadersDelegate>(MdbxFunctions.Environment.SetMaxReaders);
        _getMaxReadersDelegate = NativeLibraryLoader.GetProcAddress<GetMaxReadersDelegate>(MdbxFunctions.Environment.GetMaxReaders);
        _getMaxKeySizeDelegate = NativeLibraryLoader.GetProcAddress<GetMaxKeySizeDelegate>(MdbxFunctions.Environment.GetMaxKeySize);
    }

}
