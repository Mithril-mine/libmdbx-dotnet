namespace MDBX.Interop.Mdbx;

internal static class MdbxFunctions
{
    internal static class Environment
    {
        internal static readonly string Create = "mdbx_env_create";
        internal static readonly string Close = "mdbx_env_close";
        internal static readonly string CloseEx = "mdbx_env_close_ex";
        internal static readonly string Open = Platform.IsWindows ? "mdbx_env_openW": "mdbx_env_open";
        internal static readonly string Stat = "mdbx_env_stat";
        internal static readonly string Info = "mdbx_env_info";
        internal static readonly string Sync = "mdbx_env_info";
        internal static readonly string SetMaxDbs = "mdbx_env_set_maxdbs";
        internal static readonly string SetFlags = "mdbx_env_set_flags";
        internal static readonly string GetFlags = "mdbx_env_get_flags";
        internal static readonly string SetMapSize = "mdbx_env_set_mapsize";
        internal static readonly string SetMaxReaders = "mdbx_env_set_maxreaders";
        internal static readonly string GetMaxReaders = "mdbx_env_get_maxreaders";
        internal static readonly string GetMaxKeySize = "mdbx_env_get_maxkeysize";
    }
}
