using Libmdbx.Interop;
using System.Runtime.InteropServices;

namespace Libmdbx
{
    /// <summary>
    /// Provides information about the MDBX environment map geometry.
    /// </summary>
    public sealed class EnvironmentGeoInfo
    {
        /// <summary>Lower size limit of the environment in bytes.</summary>
        public long Lower { get; }
        /// <summary>Upper size limit of the environment in bytes.</summary>
        public long Upper { get; }
        /// <summary>Current size of the environment in bytes.</summary>
        public long Current { get; }
        /// <summary>Shrink threshold in bytes.</summary>
        public long Shrink { get; }
        /// <summary>Growth step in bytes.</summary>
        public long Grow { get; }

        internal EnvironmentGeoInfo(NativeMethods.MdbxGeoInfo g)
        {
            Lower   = (long)g.lower;
            Upper   = (long)g.upper;
            Current = (long)g.current;
            Shrink  = (long)g.shrink;
            Grow    = (long)g.grow;
        }
    }

    /// <summary>Information about the MDBX environment.</summary>
    public sealed class EnvironmentInfo
    {
        /// <summary>Map geometry information.</summary>
        public EnvironmentGeoInfo Geo { get; }
        /// <summary>Size of the data memory map in bytes.</summary>
        public long MapSize { get; }
        /// <summary>ID of the last used page.</summary>
        public long LastPageNumber { get; }
        /// <summary>ID of the last committed transaction.</summary>
        public long LastTransactionId { get; }
        /// <summary>Max reader slots in the environment.</summary>
        public int MaxReaders { get; }
        /// <summary>Max reader slots used so far.</summary>
        public int NumReaders { get; }

        internal EnvironmentInfo(NativeMethods.MdbxEnvInfo i)
        {
            Geo               = new EnvironmentGeoInfo(i.mi_geo);
            MapSize           = (long)i.mi_mapsize;
            LastPageNumber    = (long)i.mi_last_pgno;
            LastTransactionId = (long)i.mi_recent_txnid;
            MaxReaders        = (int)i.mi_maxreaders;
            NumReaders        = (int)i.mi_numreaders;
        }
    }

    /// <summary>Statistics about the MDBX environment.</summary>
    public sealed class EnvironmentStat
    {
        /// <summary>Size of a database page in bytes.</summary>
        public int PageSize { get; }
        /// <summary>Depth (height) of the B-tree.</summary>
        public int TreeDepth { get; }
        /// <summary>Number of internal (non-leaf) pages.</summary>
        public long BranchPages { get; }
        /// <summary>Number of leaf pages.</summary>
        public long LeafPages { get; }
        /// <summary>Number of overflow pages.</summary>
        public long OverflowPages { get; }
        /// <summary>Number of data items stored.</summary>
        public long Entries { get; }

        internal EnvironmentStat(NativeMethods.MdbxStat s)
        {
            PageSize      = (int)s.ms_psize;
            TreeDepth     = (int)s.ms_depth;
            BranchPages   = (long)s.ms_branch_pages;
            LeafPages     = (long)s.ms_leaf_pages;
            OverflowPages = (long)s.ms_overflow_pages;
            Entries       = (long)s.ms_entries;
        }
    }

    /// <summary>
    /// Represents a libmdbx environment – the top-level handle that maps to a
    /// single on-disk database file (or directory).
    /// <para>
    /// Dispose the environment when you are done to ensure all resources are
    /// released cleanly.
    /// </para>
    /// </summary>
    public sealed class MdbxEnvironment : IDisposable
    {
        private IntPtr _env;
        private bool _disposed;

        /// <summary>
        /// Creates a new, uninitialised libmdbx environment.
        /// Call <see cref="Open"/> before using it.
        /// </summary>
        public MdbxEnvironment()
        {
            int rc = NativeMethods.mdbx_env_create(out _env);
            ThrowIfError(rc, "mdbx_env_create");
        }

        // ------------------------------------------------------------------ //
        // Configuration (must be called before Open)
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Set the maximum number of named databases.
        /// Must be called before <see cref="Open"/>.
        /// </summary>
        /// <param name="count">Maximum number of named databases.</param>
        /// <returns>This instance, to allow method chaining.</returns>
        public MdbxEnvironment SetMaxDatabases(uint count)
        {
            ThrowIfDisposed();
            ThrowIfError(NativeMethods.mdbx_env_set_maxdbs(_env, count),
                "mdbx_env_set_maxdbs");
            return this;
        }

        /// <summary>
        /// Set the maximum number of reader (read-only transaction) slots.
        /// Must be called before <see cref="Open"/>.
        /// </summary>
        /// <param name="count">Maximum number of reader slots.</param>
        /// <returns>This instance, to allow method chaining.</returns>
        public MdbxEnvironment SetMaxReaders(uint count)
        {
            ThrowIfDisposed();
            ThrowIfError(NativeMethods.mdbx_env_set_maxreaders(_env, count),
                "mdbx_env_set_maxreaders");
            return this;
        }

        /// <summary>
        /// Set the size of the memory map.
        /// The map size is also the upper bound on the database size.
        /// </summary>
        /// <param name="bytes">Map size in bytes.</param>
        /// <returns>This instance, to allow method chaining.</returns>
        public MdbxEnvironment SetMapSize(long bytes)
        {
            ThrowIfDisposed();
            ThrowIfError(
                NativeMethods.mdbx_env_set_mapsize(_env, (UIntPtr)bytes),
                "mdbx_env_set_mapsize");
            return this;
        }

        // ------------------------------------------------------------------ //
        // Lifecycle
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Open the environment at the given filesystem path.
        /// </summary>
        /// <param name="path">
        /// Path to the database directory (or file when
        /// <see cref="EnvironmentFlags.NoSubDir"/> is set).
        /// </param>
        /// <param name="flags">Environment-open flags.</param>
        /// <param name="mode">Unix file-creation permissions (e.g. 0644).</param>
        public void Open(string path, EnvironmentFlags flags = EnvironmentFlags.None,
            int mode = 0x1B6 /* 0666 */)
        {
            ThrowIfDisposed();
            ThrowIfError(NativeMethods.mdbx_env_open(_env, path, (uint)flags, mode),
                "mdbx_env_open");
        }

        /// <summary>
        /// Flush all data buffers to disk.
        /// </summary>
        /// <param name="force">
        /// When <c>true</c>, force a synchronous flush even if the environment
        /// was opened with <see cref="EnvironmentFlags.NoSync"/>.
        /// </param>
        public void Sync(bool force = true)
        {
            ThrowIfDisposed();
            ThrowIfError(NativeMethods.mdbx_env_sync(_env, force ? 1 : 0),
                "mdbx_env_sync");
        }

        // ------------------------------------------------------------------ //
        // Transactions
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Begin a new transaction.
        /// </summary>
        /// <param name="flags">Transaction flags (default: read-write).</param>
        /// <returns>A new <see cref="MdbxTransaction"/> handle.</returns>
        public MdbxTransaction BeginTransaction(
            TransactionFlags flags = TransactionFlags.ReadWrite)
        {
            ThrowIfDisposed();
            int rc = NativeMethods.mdbx_txn_begin(_env, IntPtr.Zero, (uint)flags,
                out IntPtr txn);
            ThrowIfError(rc, "mdbx_txn_begin");
            return new MdbxTransaction(this, txn);
        }

        // ------------------------------------------------------------------ //
        // Query
        // ------------------------------------------------------------------ //

        /// <summary>Return statistics about the environment.</summary>
        public EnvironmentStat Stat()
        {
            ThrowIfDisposed();
            var stat = new NativeMethods.MdbxStat();
            ThrowIfError(
                NativeMethods.mdbx_env_stat_ex(_env, IntPtr.Zero, ref stat,
                    (UIntPtr)Marshal.SizeOf<NativeMethods.MdbxStat>()),
                "mdbx_env_stat_ex");
            return new EnvironmentStat(stat);
        }

        /// <summary>Return information about the environment.</summary>
        public EnvironmentInfo Info()
        {
            ThrowIfDisposed();
            var info = new NativeMethods.MdbxEnvInfo();
            ThrowIfError(
                NativeMethods.mdbx_env_info_ex(_env, IntPtr.Zero, ref info,
                    (UIntPtr)Marshal.SizeOf<NativeMethods.MdbxEnvInfo>()),
                "mdbx_env_info_ex");
            return new EnvironmentInfo(info);
        }

        /// <summary>
        /// Get the maximum number of reader slots.
        /// </summary>
        public int GetMaxReaders()
        {
            ThrowIfDisposed();
            ThrowIfError(NativeMethods.mdbx_env_get_maxreaders(_env, out uint readers),
                "mdbx_env_get_maxreaders");
            return (int)readers;
        }

        // ------------------------------------------------------------------ //
        // Internal helpers
        // ------------------------------------------------------------------ //

        internal IntPtr Handle
        {
            get
            {
                ThrowIfDisposed();
                return _env;
            }
        }

        internal static void ThrowIfError(int rc, string functionName)
        {
            if (rc != 0)
                throw new MdbxException(functionName, rc);
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        // ------------------------------------------------------------------ //
        // IDisposable
        // ------------------------------------------------------------------ //

        /// <summary>Close the environment, releasing all resources.</summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_env != IntPtr.Zero)
            {
                NativeMethods.mdbx_env_close_ex(_env, 0 /* do sync */);
                _env = IntPtr.Zero;
            }
        }
    }
}
