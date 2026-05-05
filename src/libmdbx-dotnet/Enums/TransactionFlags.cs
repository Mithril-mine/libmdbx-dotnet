namespace Libmdbx
{
    /// <summary>
    /// Transaction flags for <see cref="MdbxEnvironment.BeginTransaction"/>.
    /// Maps to <c>MDBX_txn_flags_t</c> in the C header.
    /// </summary>
    [Flags]
    public enum TransactionFlags : uint
    {
        /// <summary>A read-write transaction.</summary>
        ReadWrite = 0,

        /// <summary>
        /// A read-only transaction (<c>MDBX_TXN_RDONLY</c>).
        /// </summary>
        ReadOnly = 0x20000,

        /// <summary>
        /// Don't block when a write transaction is already open; raise
        /// an exception instead (<c>MDBX_TXN_TRY</c>).
        /// </summary>
        Try = 0x10000000,
    }
}
