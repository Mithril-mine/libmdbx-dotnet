using Libmdbx.Interop;
using System.Runtime.InteropServices;

namespace Libmdbx
{
    /// <summary>
    /// Represents a libmdbx cursor.
    /// <para>
    /// A cursor is bound to a specific transaction and database.  It must be
    /// closed (disposed) before the transaction ends.
    /// </para>
    /// </summary>
    public sealed class MdbxCursor : IDisposable
    {
        private IntPtr _cursor;
        private bool _disposed;

        internal MdbxCursor(IntPtr cursor)
        {
            _cursor = cursor;
        }

        // ------------------------------------------------------------------ //
        // Navigation
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Position the cursor and retrieve the key/value at that position.
        /// </summary>
        /// <param name="key">
        /// On input: the search key (required by some operations such as
        /// <see cref="CursorOp.Set"/>).
        /// On output: the key at the new cursor position.
        /// </param>
        /// <param name="value">
        /// On output: the value at the new cursor position.
        /// </param>
        /// <param name="op">The positioning operation.</param>
        /// <returns>
        /// <c>true</c> if the operation found a record;
        /// <c>false</c> when the operation returns
        /// <c>MDBX_NOTFOUND</c> (e.g. end of database reached).
        /// </returns>
        public unsafe bool Get(
            ref byte[]? key,
            ref byte[]? value,
            CursorOp op)
        {
            ThrowIfDisposed();

            // Allocate a temporary native buffer for the key we send in
            IntPtr keyBuf = IntPtr.Zero;
            try
            {
                MdbxVal kv = default;
                MdbxVal vv = default;

                if (key?.Length > 0)
                {
                    keyBuf = Marshal.AllocHGlobal(key.Length);
                    Marshal.Copy(key, 0, keyBuf, key.Length);
                    kv = new MdbxVal(keyBuf, key.Length);
                }

                int rc = NativeMethods.mdbx_cursor_get(_cursor, ref kv, ref vv, (int)op);
                if (rc == MdbxException.NotFound) return false;
                MdbxEnvironment.ThrowIfError(rc, "mdbx_cursor_get");

                // Copy back key
                if (kv.iov_base != IntPtr.Zero && kv.Length >= 0)
                {
                    key = new byte[kv.Length];
                    if (kv.Length > 0)
                        Marshal.Copy(kv.iov_base, key, 0, kv.Length);
                }
                else
                {
                    key = null;
                }

                // Copy back value
                if (vv.iov_base != IntPtr.Zero && vv.Length >= 0)
                {
                    value = new byte[vv.Length];
                    if (vv.Length > 0)
                        Marshal.Copy(vv.iov_base, value, 0, vv.Length);
                }
                else
                {
                    value = null;
                }

                return true;
            }
            finally
            {
                if (keyBuf != IntPtr.Zero)
                    Marshal.FreeHGlobal(keyBuf);
            }
        }

        /// <summary>
        /// Store a key/value pair via the cursor.
        /// The cursor is positioned at the new item on success.
        /// </summary>
        public unsafe void Put(
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> value,
            PutFlags flags = PutFlags.None)
        {
            ThrowIfDisposed();
            fixed (byte* kp = key)
            fixed (byte* vp = value)
            {
                var kv = new MdbxVal((IntPtr)kp, key.Length);
                var vv = new MdbxVal((IntPtr)vp, value.Length);
                int rc = NativeMethods.mdbx_cursor_put(_cursor,
                    ref kv, ref vv, (uint)flags);
                MdbxEnvironment.ThrowIfError(rc, "mdbx_cursor_put");
            }
        }

        /// <summary>
        /// Delete the key/data pair at the current cursor position.
        /// </summary>
        /// <param name="deleteAllDups">
        /// When <c>true</c> and the database uses <c>DUPSORT</c>, all
        /// duplicate data items for the current key are deleted.
        /// </param>
        public void Delete(bool deleteAllDups = false)
        {
            ThrowIfDisposed();
            uint flags = deleteAllDups ? 0x40u : 0u; // MDBX_NODUPDATA = 0x20; no-flag = 0
            MdbxEnvironment.ThrowIfError(
                NativeMethods.mdbx_cursor_del(_cursor, flags),
                "mdbx_cursor_del");
        }

        /// <summary>
        /// Return the count of duplicate data items for the current key.
        /// Only valid for databases opened with <see cref="DatabaseFlags.DupSort"/>.
        /// </summary>
        public int CountDuplicates()
        {
            ThrowIfDisposed();
            MdbxEnvironment.ThrowIfError(
                NativeMethods.mdbx_cursor_count(_cursor, out UIntPtr count),
                "mdbx_cursor_count");
            return (int)(ulong)count;
        }

        // ------------------------------------------------------------------ //
        // Helpers
        // ------------------------------------------------------------------ //

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        // ------------------------------------------------------------------ //
        // IDisposable
        // ------------------------------------------------------------------ //

        /// <summary>Close and free the cursor.</summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_cursor != IntPtr.Zero)
            {
                NativeMethods.mdbx_cursor_close(_cursor);
                _cursor = IntPtr.Zero;
            }
        }
    }
}
