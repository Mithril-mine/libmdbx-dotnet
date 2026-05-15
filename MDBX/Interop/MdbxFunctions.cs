namespace MDBX.Interop;

/// <summary>
/// Функции libmdbx C API.
/// </summary>
internal static class MdbxFunctions
{
    /// <summary>
    /// Функции для работы с окружением libmdbx.
    /// </summary>
    internal static class Environment
    {
        internal static readonly string Create = "mdbx_env_create";
        internal static readonly string Close = "mdbx_env_close";
        internal static readonly string CloseEx = "mdbx_env_close_ex";
        /// <summary>
        /// Open an environment instance.
        /// On Windows the mdbx_env_openW() is recommended to use.
        /// </summary>
        internal static readonly string Open = MdbxDotNetPlatform.IsWindows ? "mdbx_env_openW": "mdbx_env_open";

#pragma warning disable S1133
        [Obsolete("Please use mdbx_env_stat_ex instead of mdbx_env_stat.")]
#pragma warning restore S1133
        internal static readonly string Stat = "mdbx_env_stat";
        internal static readonly string StatEx = "mdbx_env_stat_ex";
        /// <summary>
        /// Возвращает информацию о среде MDBX.
        /// </summary>
        #pragma warning disable S1133
        [Obsolete("Please use mdbx_env_info_ex instead of mdbx_env_info.")]
        #pragma warning restore S1133
        internal static readonly string Info = "mdbx_env_info";
        /// <summary>
        /// Возвращает информацию о среде MDBX.
        /// </summary>
        internal static readonly string InfoEx = "mdbx_env_info_ex";
        internal static readonly string Sync = "mdbx_env_sync";
        internal static readonly string SetMaxDbs = "mdbx_env_set_maxdbs";
        internal static readonly string SetFlags = "mdbx_env_set_flags";
        internal static readonly string GetFlags = "mdbx_env_get_flags";
        internal static readonly string SetMapSize = "mdbx_env_set_mapsize";
        internal static readonly string SetMaxReaders = "mdbx_env_set_maxreaders";
        internal static readonly string GetMaxReaders = "mdbx_env_get_maxreaders";
        internal static readonly string GetMaxKeySize = "mdbx_env_get_maxkeysize";
        
    }
}
