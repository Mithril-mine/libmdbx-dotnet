using System;
using System.Runtime.InteropServices;

namespace Libmdbx.Interop
{
    /// <summary>
    /// All P/Invoke entry points into the native libmdbx shared library.
    /// </summary>
    internal static partial class NativeMethods
    {
        internal const string LibraryName = "mdbx";

        // ------------------------------------------------------------------ //
        // Initialisation
        // ------------------------------------------------------------------ //

        static NativeMethods() => LibraryResolver.EnsureRegistered();

        // ------------------------------------------------------------------ //
        // Error helpers
        // ------------------------------------------------------------------ //

        /// <summary>Return a string describing a given error code.</summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "mdbx_strerror")]
        internal static extern IntPtr mdbx_strerror(int errnum);

        // ------------------------------------------------------------------ //
        // Environment
        // ------------------------------------------------------------------ //

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_create(out IntPtr env);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        internal static extern int mdbx_env_open(
            IntPtr env,
            string path,
            uint flags,
            int mode);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_close(IntPtr env);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_close_ex(IntPtr env, int dont_sync);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_set_maxdbs(IntPtr env, uint dbs);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_set_maxreaders(IntPtr env, uint readers);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_get_maxreaders(IntPtr env, out uint readers);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_set_mapsize(IntPtr env, UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_set_flags(IntPtr env, uint flags, int onoff);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_get_flags(IntPtr env, out uint flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_sync(IntPtr env, int force);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_stat_ex(
            IntPtr env,
            IntPtr txn,     // pass IntPtr.Zero to read from last committed write txn
            ref MdbxStat stat,
            UIntPtr bytes);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_info_ex(
            IntPtr env,
            IntPtr txn,
            ref MdbxEnvInfo info,
            UIntPtr bytes);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_env_get_maxkeysize_ex(IntPtr env, uint flags);

        // ------------------------------------------------------------------ //
        // Transactions
        // ------------------------------------------------------------------ //

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_txn_begin(
            IntPtr env,
            IntPtr parent,
            uint flags,
            out IntPtr txn);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_txn_commit(IntPtr txn);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_txn_abort(IntPtr txn);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_txn_reset(IntPtr txn);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_txn_renew(IntPtr txn);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong mdbx_txn_id(IntPtr txn);

        // ------------------------------------------------------------------ //
        // Databases (DBI)
        // ------------------------------------------------------------------ //

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        internal static extern int mdbx_dbi_open(
            IntPtr txn,
            string? name,
            uint flags,
            out uint dbi);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_dbi_close(IntPtr env, uint dbi);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_drop(IntPtr txn, uint dbi, int del);

        // ------------------------------------------------------------------ //
        // Key/Value operations
        // ------------------------------------------------------------------ //

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_put(
            IntPtr txn,
            uint dbi,
            ref MdbxVal key,
            ref MdbxVal value,
            uint flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_get(
            IntPtr txn,
            uint dbi,
            ref MdbxVal key,
            out MdbxVal value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_del(
            IntPtr txn,
            uint dbi,
            ref MdbxVal key,
            IntPtr value);   // pass IntPtr.Zero to delete any matching key

        // ------------------------------------------------------------------ //
        // Cursors
        // ------------------------------------------------------------------ //

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_cursor_open(
            IntPtr txn,
            uint dbi,
            out IntPtr cursor);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void mdbx_cursor_close(IntPtr cursor);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_cursor_get(
            IntPtr cursor,
            ref MdbxVal key,
            ref MdbxVal value,
            int op);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_cursor_put(
            IntPtr cursor,
            ref MdbxVal key,
            ref MdbxVal value,
            uint flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_cursor_del(
            IntPtr cursor,
            uint flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdbx_cursor_count(
            IntPtr cursor,
            out UIntPtr count);

        // ------------------------------------------------------------------ //
        // Native struct definitions (must match the C layout exactly)
        // ------------------------------------------------------------------ //

        [StructLayout(LayoutKind.Sequential)]
        internal struct MdbxStat
        {
            public uint ms_psize;           // Size of a database page
            public uint ms_depth;           // Depth (height) of the B-tree
            public ulong ms_branch_pages;   // Number of internal (non-leaf) pages
            public ulong ms_leaf_pages;     // Number of leaf pages
            public ulong ms_overflow_pages; // Number of overflow pages
            public ulong ms_entries;        // Number of data items
            public ulong ms_mod_txnid;      // Transaction ID of last committed modification
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MdbxGeoInfo
        {
            public ulong lower;
            public ulong upper;
            public ulong current;
            public ulong shrink;
            public ulong grow;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MdbxEnvInfo
        {
            public MdbxGeoInfo mi_geo;
            public ulong mi_mapsize;            // Size of the data memory map
            public ulong mi_last_pgno;          // ID of the last used page
            public ulong mi_recent_txnid;       // ID of the last committed transaction
            public ulong mi_latter_reader_txnid;
            public ulong mi_self_latter_reader_txnid;
            public ulong mi_meta0_txnid;
            public ulong mi_meta0_sign;
            public ulong mi_meta1_txnid;
            public ulong mi_meta1_sign;
            public ulong mi_meta2_txnid;
            public ulong mi_meta2_sign;
            public uint mi_maxreaders;          // Max reader slots in the environment
            public uint mi_numreaders;          // Max reader slots used so far
            public uint mi_dxb_pagesize;        // Database page size
            public uint mi_sys_pagesize;        // System page size
            // Additional fields omitted for brevity
        }
    }
}
