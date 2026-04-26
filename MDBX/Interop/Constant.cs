using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace MDBX.Interop
{
    internal class Constant
    {
        /* no environment directory */
        public const int MDBX_NOSUBDIR = 0x4000;
        /* don't fsync after commit */
        public const int MDBX_NOSYNC = 0x10000;
        /* read only */
        public const int MDBX_RDONLY = 0x20000;
        /* don't fsync metapage after commit */
        public const int MDBX_NOMETASYNC = 0x40000;
        /* use writable mmap */
        public const int MDBX_WRITEMAP = 0x80000;
        /* use asynchronous msync when MDBX_WRITEMAP is used */
        public const int MDBX_MAPASYNC = 0x100000;
        /* tie reader locktable slots to MDBX_txn objects instead of to threads */
        public const int MDBX_NOTLS = 0x200000;
        /* open DB in exclusive/monopolistic mode. */
        public const int MDBX_EXCLUSIVE = 0x400000;
        /* don't do readahead */
        public const int MDBX_NORDAHEAD = 0x800000;
        /* don't initialize malloc'd memory before writing to datafile */
        public const int MDBX_NOMEMINIT = 0x1000000;
        /* aim to coalesce FreeDB records */
        public const int MDBX_COALESCE = 0x2000000;
        /* LIFO policy for reclaiming FreeDB records */
        public const int MDBX_LIFORECLAIM = 0x4000000;
        /* make a steady-sync only on close and explicit env-sync */
        public const int MDBX_UTTERLY_NOSYNC = (MDBX_NOSYNC | MDBX_MAPASYNC);
        /* debuging option; fill/perturb released pages */
        public const int MDBX_PAGEPERTURB = 0x8000000;
        /* Do not block when starting a write transaction */
        public const int MDBX_TRYTXN = 0x10000000;

        /* use reverse string keys */
        public const int MDBX_REVERSEKEY = 0x02;
        /* use sorted duplicates */
        public const int MDBX_DUPSORT = 0x04;
        /* numeric keys in native byte order, either uint32_t or uint64_t.
         * The keys must all be of the same size. */
        public const int MDBX_INTEGERKEY = 0x08;
        /* with MDBX_DUPSORT, sorted dup items have fixed size */
        public const int MDBX_DUPFIXED = 0x10;
        /* with MDBX_DUPSORT, dups are MDBX_INTEGERKEY-style integers */
        public const int MDBX_INTEGERDUP = 0x20;
        /* with MDBX_DUPSORT, use reverse string dups */
        public const int MDBX_REVERSEDUP = 0x40;
        /* create DB if not already existing */
        public const int MDBX_CREATE = 0x40000;


        /* For put: Don't write if the key already exists. */
        public const int MDBX_NOOVERWRITE = 0x10;
        /* Only for MDBX_DUPSORT
         * For put: don't write if the key and data pair already exist.
         * For mdbx_cursor_del: remove all duplicate data items. */
        public const int MDBX_NODUPDATA = 0x20;
        /* For mdbx_cursor_put: overwrite the current key/data pair
         * MDBX allows this flag for mdbx_put() for explicit overwrite/update without
         * insertion. */
        public const int MDBX_CURRENT = 0x40;
        /* For put: Just reserve space for data, don't copy it. Return a
         * pointer to the reserved space. */
        public const int MDBX_RESERVE = 0x10000;
        /* Data is being appended, don't split full pages. */
        public const int MDBX_APPEND = 0x20000;
        /* Duplicate data is being appended, don't split full pages. */
        public const int MDBX_APPENDDUP = 0x40000;
        /* Store multiple data items in one call. Only for MDBX_DUPFIXED. */
        public const int MDBX_MULTIPLE = 0x80000;

        /// <summary>
        /// Using database/environment which already opened by another process(es).
        /// </summary>
        public const int MDBX_ACCEDE = 0x40000000;

        /// <summary>
        /// Отвязывает транзакции от потоков/threads насколько это возможно.
        /// </summary>
        public const int MDBX_NOSTICKYTHREADS = 0x200000;


        /// <summary>
        /// /* SYNC MODES****************************************************************/
        /** \defgroup sync_modes SYNC MODES
         *
         * \attention Using any combination of \ref MDBX_SAFE_NOSYNC, \ref
         * MDBX_NOMETASYNC and especially \ref MDBX_UTTERLY_NOSYNC is always a deal to
         * reduce durability for gain write performance. You must know exactly what
         * you are doing and what risks you are taking!
         *
         * \note for LMDB users: \ref MDBX_SAFE_NOSYNC is NOT similar to LMDB_NOSYNC,
         * but \ref MDBX_UTTERLY_NOSYNC is exactly match LMDB_NOSYNC. See details
         * below.
         *
         * THE SCENE:
         * - The DAT-file contains several MVCC-snapshots of B-tree at same time,
         *   each of those B-tree has its own root page.
         * - Each of meta pages at the beginning of the DAT file contains a
         *   pointer to the root page of B-tree which is the result of the particular
         *   transaction, and a number of this transaction.
         * - For data durability, MDBX must first write all MVCC-snapshot data
         *   pages and ensure that are written to the disk, then update a meta page
         *   with the new transaction number and a pointer to the corresponding new
         *   root page, and flush any buffers yet again.
         * - Thus during commit a I/O buffers should be flushed to the disk twice;
         *   i.e. fdatasync(), FlushFileBuffers() or similar syscall should be
         *   called twice for each commit. This is very expensive for performance,
         *   but guaranteed durability even on unexpected system failure or power
         *   outage. Of course, provided that the operating system and the
         *   underlying hardware (e.g. disk) work correctly.
         *
         * TRADE-OFF:
         * By skipping some stages described above, you can significantly benefit in
         * speed, while partially or completely losing in the guarantee of data
         * durability and/or consistency in the event of system or power failure.
         * Moreover, if for any reason disk write order is not preserved, then at
         * moment of a system crash, a meta-page with a pointer to the new B-tree may
         * be written to disk, while the itself B-tree not yet. In that case, the
         * database will be corrupted!
         *
         * \see MDBX_SYNC_DURABLE \see MDBX_NOMETASYNC \see MDBX_SAFE_NOSYNC
         * \see MDBX_UTTERLY_NOSYNC
         *
         * @{ */

        /** Default robust and durable sync mode.
         *
         * Metadata is written and flushed to disk after a data is written and
         * flushed, which guarantees the integrity of the database in the event
         * of a crash at any time.
         *
         * \attention Please do not use other modes until you have studied all the
         * details and are sure. Otherwise, you may lose your users' data, as happens
         * in [Miranda NG](https://www.miranda-ng.org/) messenger. */
        /// </summary>
        public const int MDBX_SYNC_DURABLE = 0;

        /// <summary>
        /// Start read-write transaction.
        ///
        /// Only one write transaction may be active at a time.Writes are fully
        /// serialized, which guarantees that writers can never deadlock. */
        /// </summary>
        public const int MDBX_TXN_READWRITE = 0;

        /** 
 * Don't sync anything but keep previous steady commits.
 *
 * Like \ref MDBX_UTTERLY_NOSYNC the `MDBX_SAFE_NOSYNC` flag disable similarly
 * flush system buffers to disk when committing a transaction. But there is a
 * huge difference in how are recycled the MVCC snapshots corresponding to
 * previous "steady" transactions (see below).
 *
 * With \ref MDBX_WRITEMAP the `MDBX_SAFE_NOSYNC` instructs MDBX to use
 * asynchronous mmap-flushes to disk. Asynchronous mmap-flushes means that
 * actually all writes will scheduled and performed by operation system on it
 * own manner, i.e. unordered. MDBX itself just notify operating system that
 * it would be nice to write data to disk, but no more.
 *
 * Depending on the platform and hardware, with `MDBX_SAFE_NOSYNC` you may get
 * a multiple increase of write performance, even 10 times or more.
 *
 * In contrast to \ref MDBX_UTTERLY_NOSYNC mode, with `MDBX_SAFE_NOSYNC` flag
 * MDBX will keeps untouched pages within B-tree of the last transaction
 * "steady" which was synced to disk completely. This has big implications for
 * both data durability and (unfortunately) performance:
 *  - a system crash can't corrupt the database, but you will lose the last
 *    transactions; because MDBX will rollback to last steady commit since it
 *    kept explicitly.
 *  - the last steady transaction makes an effect similar to "long-lived" read
 *    transaction (see above in the \ref restrictions section) since prevents
 *    reuse of pages freed by newer write transactions, thus the any data
 *    changes will be placed in newly allocated pages.
 *  - to avoid rapid database growth, the system will sync data and issue
 *    a steady commit-point to resume reuse pages, each time there is
 *    insufficient space and before increasing the size of the file on disk.
 *
 * In other words, with `MDBX_SAFE_NOSYNC` flag MDBX insures you from the
 * whole database corruption, at the cost increasing database size and/or
 * number of disk IOPs. So, `MDBX_SAFE_NOSYNC` flag could be used with
 * \ref mdbx_env_sync() as alternatively for batch committing or nested
 * transaction (in some cases). As well, auto-sync feature exposed by
 * \ref mdbx_env_set_syncbytes() and \ref mdbx_env_set_syncperiod() functions
 * could be very useful with `MDBX_SAFE_NOSYNC` flag.
 *
 * The number and volume of disk IOPs with MDBX_SAFE_NOSYNC flag will
 * exactly the as without any no-sync flags. However, you should expect a
 * larger process's [work set](https://bit.ly/2kA2tFX) and significantly worse
 * a [locality of reference](https://bit.ly/2mbYq2J), due to the more
 * intensive allocation of previously unused pages and increase the size of
 * the database.
 *
 * `MDBX_SAFE_NOSYNC` flag may be changed at any time using
 * \ref mdbx_env_set_flags() or by passing to \ref mdbx_txn_begin() for
 * particular write transaction. 
*/
        public const int MDBX_SAFE_NOSYNC = 0x10000;

        /// <summary>
        /// Prepare but not start read-only transaction.
        // Transaction will not be started immediately, but created transaction handle will be ready for use with mdbx_txn_renew().
        // This flag allows to preallocate memory and assign a reader slot, thus avoiding these operations at the next start of the transaction.
        /// </summary>
        public const int MDBX_TXN_RDONLY_PREPARE = MDBX_RDONLY | MDBX_NOMEMINIT;
    }
}
