namespace MDBX.Interop.Functions;

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
        internal static readonly string SyncEx = "mdbx_env_sync_ex";
        internal static readonly string SetMaxDbs = "mdbx_env_set_maxdbs";
        internal static readonly string SetFlags = "mdbx_env_set_flags";
        internal static readonly string GetFlags = "mdbx_env_get_flags";
        internal static readonly string SetMapSize = "mdbx_env_set_mapsize";
        internal static readonly string SetMaxReaders = "mdbx_env_set_maxreaders";
        internal static readonly string GetMaxReaders = "mdbx_env_get_maxreaders";
        internal static readonly string GetMaxKeySize = "mdbx_env_get_maxkeysize";
    }

    /// <summary>
    /// Функции для работы с базами данных libmdbx.
    /// </summary>
    internal static class Database
    {
        internal static readonly string Open = "mdbx_dbi_open";
        internal static readonly string Close = "mdbx_dbi_close";
        internal static readonly string Put = "mdbx_put";
        internal static readonly string Get = "mdbx_get";
        internal static readonly string Del = "mdbx_del";
        internal static readonly string Drop = "mdbx_drop";
    }

    /// <summary>
    /// Вспомогательные функции libmdbx.
    /// </summary>
    internal static class Misc
    {
        internal static readonly string StringError = "mdbx_strerror";
    }

    /// <summary>
    /// Функции для работы с транзакциями libmdbx.
    /// </summary>
    internal static class Transaction
    {
        internal static readonly string Begin = "mdbx_txn_begin";
        internal static readonly string Commit = "mdbx_txn_commit";
        internal static readonly string Abort = "mdbx_txn_abort";
        internal static readonly string Reset = "mdbx_txn_reset";
        internal static readonly string Renew = "mdbx_txn_renew";
        internal static readonly string GetID = "mdbx_txn_id";
    }

    /// <summary>
    /// Функции для работы с курсорами libmdbx.
    /// </summary>
    internal static class Cursor
    {
        internal static readonly string Open = "mdbx_cursor_open";
        internal static readonly string Close = "mdbx_cursor_close";
        internal static readonly string Get = "mdbx_cursor_get";
        internal static readonly string Put = "mdbx_cursor_put";
        internal static readonly string Del = "mdbx_cursor_del";
        internal static readonly string Count = "mdbx_cursor_count";
    }
}
