using Libmdbx.Interop;

namespace Libmdbx
{
    /// <summary>
    /// Represents a libmdbx transaction.
    /// <para>
    /// A transaction is either committed with <see cref="Commit"/> or
    /// rolled back.  Disposing a transaction without committing it aborts it
    /// automatically.
    /// </para>
    /// </summary>
    public sealed class MdbxTransaction : IDisposable
    {
        private IntPtr _txn;
        private bool _finished;   // committed or aborted
        private readonly MdbxEnvironment _env;

        internal MdbxTransaction(MdbxEnvironment env, IntPtr txn)
        {
            _env = env;
            _txn = txn;
        }

        // ------------------------------------------------------------------ //
        // Open databases
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Open a database handle within this transaction.
        /// </summary>
        /// <param name="name">
        /// Name of the database, or <c>null</c> for the default (unnamed)
        /// database.
        /// </param>
        /// <param name="flags">Database flags.</param>
        /// <returns>A <see cref="MdbxDatabase"/> handle.</returns>
        public MdbxDatabase OpenDatabase(
            string? name = null,
            DatabaseFlags flags = DatabaseFlags.None)
        {
            ThrowIfFinished();
            int rc = NativeMethods.mdbx_dbi_open(_txn, name, (uint)flags, out uint dbi);
            MdbxEnvironment.ThrowIfError(rc, "mdbx_dbi_open");
            return new MdbxDatabase(_env, this, dbi);
        }

        // ------------------------------------------------------------------ //
        // Commit / abort
        // ------------------------------------------------------------------ //

        /// <summary>Commit the transaction, writing all changes to the database.</summary>
        /// <exception cref="InvalidOperationException">
        /// If the transaction has already been committed or aborted.
        /// </exception>
        public void Commit()
        {
            ThrowIfFinished();
            _finished = true;
            MdbxEnvironment.ThrowIfError(NativeMethods.mdbx_txn_commit(_txn),
                "mdbx_txn_commit");
        }

        /// <summary>Abort the transaction, discarding all uncommitted changes.</summary>
        public void Abort()
        {
            if (_finished || _txn == IntPtr.Zero) return;
            _finished = true;
            NativeMethods.mdbx_txn_abort(_txn);
        }

        /// <summary>
        /// Reset a read-only transaction, releasing the reader lock while
        /// keeping the handle alive.  Call <see cref="Renew"/> to re-acquire.
        /// </summary>
        public void Reset()
        {
            ThrowIfFinished();
            MdbxEnvironment.ThrowIfError(NativeMethods.mdbx_txn_reset(_txn),
                "mdbx_txn_reset");
        }

        /// <summary>
        /// Re-acquire a reader lock for a previously <see cref="Reset"/> transaction.
        /// </summary>
        public void Renew()
        {
            ThrowIfFinished();
            MdbxEnvironment.ThrowIfError(NativeMethods.mdbx_txn_renew(_txn),
                "mdbx_txn_renew");
        }

        // ------------------------------------------------------------------ //
        // Query
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Return the transaction ID.  For read-only transactions this is the
        /// snapshot sequence number.
        /// </summary>
        public ulong Id()
        {
            ThrowIfFinished();
            return NativeMethods.mdbx_txn_id(_txn);
        }

        // ------------------------------------------------------------------ //
        // Internal helpers
        // ------------------------------------------------------------------ //

        internal IntPtr Handle
        {
            get
            {
                ThrowIfFinished();
                return _txn;
            }
        }

        private void ThrowIfFinished()
        {
            if (_finished || _txn == IntPtr.Zero)
                throw new InvalidOperationException(
                    "The transaction has already been committed or aborted.");
        }

        // ------------------------------------------------------------------ //
        // IDisposable – auto-abort if not committed
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Dispose the transaction.  If it has not been committed, it is
        /// aborted automatically.
        /// </summary>
        public void Dispose() => Abort();
    }
}
