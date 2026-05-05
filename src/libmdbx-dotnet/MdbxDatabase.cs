using Libmdbx.Interop;
using System.Runtime.InteropServices;

namespace Libmdbx
{
    /// <summary>
    /// Represents an open named (or unnamed) libmdbx database within a
    /// transaction.
    /// </summary>
    public sealed class MdbxDatabase
    {
        private readonly MdbxEnvironment _env;
        private readonly MdbxTransaction _txn;
        internal readonly uint _dbi;

        internal MdbxDatabase(MdbxEnvironment env, MdbxTransaction txn, uint dbi)
        {
            _env = env;
            _txn = txn;
            _dbi = dbi;
        }

        // ------------------------------------------------------------------ //
        // Write operations
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Store a key/value pair.
        /// </summary>
        /// <param name="key">Key bytes (must not be empty).</param>
        /// <param name="value">Value bytes.</param>
        /// <param name="flags">Put behaviour flags.</param>
        public unsafe void Put(ReadOnlySpan<byte> key, ReadOnlySpan<byte> value,
            PutFlags flags = PutFlags.None)
        {
            fixed (byte* kp = key)
            fixed (byte* vp = value)
            {
                var kv = new MdbxVal((IntPtr)kp, key.Length);
                var vv = new MdbxVal((IntPtr)vp, value.Length);
                int rc = NativeMethods.mdbx_put(_txn.Handle, _dbi,
                    ref kv, ref vv, (uint)flags);
                MdbxEnvironment.ThrowIfError(rc, "mdbx_put");
            }
        }

        /// <summary>
        /// Store a key/value pair where key and value are byte arrays.
        /// </summary>
        public void Put(byte[] key, byte[] value, PutFlags flags = PutFlags.None)
            => Put(key.AsSpan(), value.AsSpan(), flags);

        /// <summary>
        /// Delete a key (and all of its duplicate data items).
        /// </summary>
        /// <param name="key">Key to delete.</param>
        /// <returns>
        /// <c>true</c> if the key was found and deleted;
        /// <c>false</c> if the key was not found.
        /// </returns>
        public unsafe bool Delete(ReadOnlySpan<byte> key)
        {
            fixed (byte* kp = key)
            {
                var kv = new MdbxVal((IntPtr)kp, key.Length);
                int rc = NativeMethods.mdbx_del(_txn.Handle, _dbi,
                    ref kv, IntPtr.Zero);
                if (rc == MdbxException.NotFound) return false;
                MdbxEnvironment.ThrowIfError(rc, "mdbx_del");
                return true;
            }
        }

        /// <summary>
        /// Delete a key (byte-array overload).
        /// </summary>
        public bool Delete(byte[] key) => Delete(key.AsSpan());

        // ------------------------------------------------------------------ //
        // Read operations
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Retrieve the value associated with <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Key to look up.</param>
        /// <returns>
        /// The value bytes, or <c>null</c> if the key was not found.
        /// </returns>
        public unsafe byte[]? Get(ReadOnlySpan<byte> key)
        {
            fixed (byte* kp = key)
            {
                var kv = new MdbxVal((IntPtr)kp, key.Length);
                int rc = NativeMethods.mdbx_get(_txn.Handle, _dbi,
                    ref kv, out MdbxVal vv);
                if (rc == MdbxException.NotFound) return null;
                MdbxEnvironment.ThrowIfError(rc, "mdbx_get");

                if (vv.iov_base == IntPtr.Zero || vv.Length == 0)
                    return [];

                byte[] result = new byte[vv.Length];
                Marshal.Copy(vv.iov_base, result, 0, result.Length);
                return result;
            }
        }

        /// <summary>
        /// Retrieve the value associated with <paramref name="key"/> (byte-array overload).
        /// </summary>
        public byte[]? Get(byte[] key) => Get(key.AsSpan());

        /// <summary>
        /// Check whether a key exists in the database.
        /// </summary>
        /// <param name="key">Key to look up.</param>
        /// <returns><c>true</c> if the key exists; otherwise <c>false</c>.</returns>
        public bool ContainsKey(ReadOnlySpan<byte> key)
            => Get(key) is not null;

        /// <summary>
        /// Check whether a key exists in the database (byte-array overload).
        /// </summary>
        public bool ContainsKey(byte[] key) => ContainsKey(key.AsSpan());

        // ------------------------------------------------------------------ //
        // Cursor
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Open a cursor for this database.
        /// </summary>
        /// <returns>A new <see cref="MdbxCursor"/>.</returns>
        public MdbxCursor OpenCursor()
        {
            int rc = NativeMethods.mdbx_cursor_open(_txn.Handle, _dbi,
                out IntPtr cursor);
            MdbxEnvironment.ThrowIfError(rc, "mdbx_cursor_open");
            return new MdbxCursor(cursor);
        }

        // ------------------------------------------------------------------ //
        // Maintenance
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Delete all key/data pairs from the database (truncate).
        /// </summary>
        public void Empty()
        {
            MdbxEnvironment.ThrowIfError(
                NativeMethods.mdbx_drop(_txn.Handle, _dbi, 0),
                "mdbx_drop");
        }

        /// <summary>
        /// Drop (delete) the entire database.
        /// </summary>
        public void Drop()
        {
            MdbxEnvironment.ThrowIfError(
                NativeMethods.mdbx_drop(_txn.Handle, _dbi, 1),
                "mdbx_drop");
        }

        /// <summary>
        /// Close the database handle.
        /// Usually not necessary – handles are closed automatically when the
        /// environment closes.
        /// </summary>
        public void Close()
        {
            MdbxEnvironment.ThrowIfError(
                NativeMethods.mdbx_dbi_close(_env.Handle, _dbi),
                "mdbx_dbi_close");
        }
    }
}
