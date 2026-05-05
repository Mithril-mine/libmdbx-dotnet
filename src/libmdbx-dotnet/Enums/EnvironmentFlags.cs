namespace Libmdbx
{
    /// <summary>
    /// Flags for <see cref="MdbxEnvironment.Open"/>.
    /// Maps to the <c>MDBX_env_flags_t</c> enum in the C header.
    /// </summary>
    [Flags]
    public enum EnvironmentFlags : uint
    {
        /// <summary>No flags.</summary>
        None = 0,

        /// <summary>
        /// Use path as a file, not a directory.  The lock file will be
        /// <i>path</i> with <c>-lck</c> appended.
        /// </summary>
        NoSubDir = 0x4000,

        /// <summary>Open in read-only mode.</summary>
        ReadOnly = 0x20000,

        /// <summary>
        /// Use a writeable memory map (fewer allocations but less crash-safety).
        /// </summary>
        WriteMap = 0x80000,

        /// <summary>
        /// Flush system buffers to disk only once per transaction (omits the
        /// metadata flush).  Preserves ACI but not D.
        /// </summary>
        NoMetaSync = 0x40000,

        /// <summary>
        /// Don't flush system buffers on commit.  Risk of data loss on crash.
        /// </summary>
        NoSync = 0x10000,

        /// <summary>
        /// When using <see cref="WriteMap"/>, use asynchronous flushes.
        /// </summary>
        MapAsync = 0x100000,

        /// <summary>
        /// Don't use Thread-Local Storage; tie reader slots to transaction
        /// objects instead of threads.
        /// </summary>
        NoTLS = 0x200000,

        /// <summary>Disable readahead.</summary>
        NoReadAhead = 0x800000,

        /// <summary>Don't initialise malloc'd memory before use.</summary>
        NoMemInit = 0x1000000,

        /// <summary>LIFO policy for reclaiming free-list records.</summary>
        LifoReclaim = 0x400000,

        /// <summary>Coalesce records when reclaiming the free-list.</summary>
        Coalesce = 0x2000000,

        /// <summary>Exclusive access mode.</summary>
        Exclusive = 0x40000000u,
    }
}
