namespace MDBX
{
    using Interop;

    /// <summary>
    /// TransactionOption.
    /// </summary>
    [Flags]
#pragma warning disable S4070
#pragma warning disable S2342
    public enum TransactionOption : int
#pragma warning restore S2342
#pragma warning restore S4070
    {


        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Нет специальных опций.
        /// </summary>
        Unspecific = None,

        /// <summary>
        /// Start read-write transaction.
        ///
        /// Only one write transaction may be active at a time.Writes are fully
        /// serialized, which guarantees that writers can never deadlock. */
        /// </summary>
        ReadWrite = Constant.MDBX_TXN_READWRITE,

        /// <summary>
        /// Flush system buffers to disk only once per transaction, omit the metadata flush.
        /// Defer that until the system flushes files to disk,
        /// or next non-MDBX_RDONLY commit or mdbx_env_sync().
        /// 
        /// This optimization maintains database integrity, 
        /// but a system crash may undo the last committed transaction.
        /// I.e. it preserves the ACI (atomicity,consistency, isolation) but not D (durability) database property.
        /// This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMetaSync = Constant.MDBX_NOMETASYNC,

        /// <summary>
        /// Don't flush system buffers to disk when committing a transaction.
        /// This optimization means a system crash can corrupt the database or 
        /// lose the last transactions if buffers are not yet flushed to disk.
        /// 
        /// The risk is governed by how often the system flushes dirty buffers
        /// to disk and how often mdbx_env_sync() is called.  However, if the
        /// filesystem preserves write order and the MDBX_WRITEMAP and/or
        /// LIFORECLAIM flags are not used, transactions exhibit ACI(atomicity, consistency, isolation)
        /// properties and only lose D(durability) 
        /// I.e. database integrity is maintained, but a system crash may undo the final transactions.
        /// 
        /// Note that (MDBX_NOSYNC | MDBX_WRITEMAP) leaves the system with no hint for when to write transactions to disk.
        /// Therefore the (MDBX_MAPASYNC | MDBX_WRITEMAP) may be preferable.
        /// This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoSync = Constant.MDBX_NOSYNC,

        /// <summary>
        /// This transaction will not perform any write operations.
        /// </summary>
        ReadOnly = Constant.MDBX_RDONLY,

        /// <summary>
        /// Prepare but not start read-only transaction.
        /// Transaction will not be started immediately, but created transaction handle will be ready for use with mdbx_txn_renew().
        /// This flag allows to preallocate memory and assign a reader slot, thus avoiding these operations at the next start of the transaction.
        /// </summary>
        ReadonlyPrepare = Constant.MDBX_TXN_RDONLY_PREPARE,

        /// <summary>
        /// Do not block when starting a write transaction
        /// </summary>
        Try = Constant.MDBX_TRYTXN,
    }
}
