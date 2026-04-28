using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    [Flags]
    public enum EnvironmentFlag : int
    {
        Unspecific = 0,

        /// <summary>
        /// By default, MDBX creates its environment in a directory whose
        /// pathname is given in path, and creates its data and lock files
        /// under that directory. With this option, path is used as-is for
        /// the database main data file. The database lock file is the path
        /// with "-lock" appended.
        /// </summary>
        NoSubDir = Constant.MDBX_NOSUBDIR,

        /// <summary>
        /// Open the environment in read-only mode. No write operations will
        /// be allowed. MDBX will still modify the lock file - except on
        /// read-only filesystems, where MDBX does not use locks.
        /// </summary>
        ReadOnly = Constant.MDBX_RDONLY,

        /// <summary>
        /// Use a writeable memory map unless MDBX_RDONLY is set. This uses fewer
        ///  mallocs but loses protection from application bugs like wild pointer
        ///  writes and other bad updates into the database.
        ///  This may be slightly faster for DBs that fit entirely in RAM,
        ///  but is slower for DBs larger than RAM.
        ///  Incompatible with nested transactions.
        ///  Do not mix processes with and without MDBX_WRITEMAP on the same
        ///  environment.  This can defeat durability (mdbx_env_sync etc).
        ///  with MDBX_WRITEMAP = all data will be mapped into memory in the read-write mode. This offers a significant performance benefit, since the data will be modified directly in mapped memory and then flushed to disk by single system call, without any memory management nor copying.
        ///  without MDBX_WRITEMAP = data will be mapped into memory in the read-only mode. This requires stocking all modified database pages in memory and then writing them to disk through file operations.
        /// </summary>
        WriteMap = Constant.MDBX_WRITEMAP,

        /// <summary>
        ///  Flush system buffers to disk only once per transaction, omit the
        ///  metadata flush. Defer that until the system flushes files to disk,
        ///  or next non-MDBX_RDONLY commit or mdbx_env_sync(). This optimization
        ///  maintains database integrity, but a system crash may undo the last
        ///  committed transaction. I.e. it preserves the ACI (atomicity,
        ///  consistency, isolation) but not D (durability) database property.
        ///  This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMetaSync = Constant.MDBX_NOMETASYNC,

        /// <summary>
        ///  Don't flush system buffers to disk when committing a transaction.
        ///  This optimization means a system crash can corrupt the database or
        ///  lose the last transactions if buffers are not yet flushed to disk.
        ///  The risk is governed by how often the system flushes dirty buffers
        ///  to disk and how often mdbx_env_sync() is called.  However, if the
        ///  filesystem preserves write order and the MDBX_WRITEMAP and/or
        ///  MDBX_LIFORECLAIM flags are not used, transactions exhibit ACI
        ///  (atomicity, consistency, isolation) properties and only lose D
        ///  (durability).  I.e. database integrity is maintained, but a system
        ///  crash may undo the final transactions.
        ///  
        ///  Note that (MDBX_NOSYNC | MDBX_WRITEMAP) leaves the system with no
        ///  hint for when to write transactions to disk.
        ///  Therefore the (MDBX_MAPASYNC | MDBX_WRITEMAP) may be preferable.
        ///  This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoSync = Constant.MDBX_NOSYNC,

        /// <summary>
        /// When using MDBX_WRITEMAP, use asynchronous flushes to disk. As with
        /// MDBX_NOSYNC, a system crash can then corrupt the database or lose
        /// the last transactions. Calling mdbx_env_sync() ensures on-disk
        /// database integrity until next commit. This flag may be changed at
        /// any time using mdbx_env_set_flags().
        /// 
        /// Obsolete
        /// Please use MDBX_SAFE_NOSYNC instead of MDBX_MAPASYNC.
        /// 
        /// Since version 0.9.x the MDBX_MAPASYNC is deprecated and has the same effect as MDBX_SAFE_NOSYNC with MDBX_WRITEMAP. 
        /// This just API simplification is for convenience and clarity.
        /// </summary>
        #pragma warning disable S1133
        [Obsolete("Please use MDBX_SAFE_NOSYNC instead of MDBX_MAPASYNC.")]
        #pragma warning restore S1133
        MapAsync = Constant.MDBX_MAPASYNC,

        /// <summary>
        /// Don't use Thread-Local Storage. Tie reader locktable slots to
        /// MDBX_txn objects instead of to threads. I.e. mdbx_txn_reset() keeps
        /// the slot reseved for the MDBX_txn object. A thread may use parallel
        /// read-only transactions. A read-only transaction may span threads if
        /// the user synchronizes its use. Applications that multiplex many
        /// user threads over individual OS threads need this option. Such an
        /// application must also serialize the write transactions in an OS
        /// thread, since MDBX's write locking is unaware of the user threads.
        /// </summary>
        NoTLS = Constant.MDBX_NOTLS,

        /// <summary>
        /// Turn off readahead. Most operating systems perform readahead on 
        /// read requests by default. This option turns it off if the OS
        /// supports it. Turning it off may help random read performance
        /// when the DB is larger than RAM and system RAM is full.
        /// </summary>
        NoReadAhead = Constant.MDBX_NORDAHEAD,

        /// <summary>
        /// Don't initialize malloc'd memory before writing to unused spaces
        /// in the data file. By default, memory for pages written to the data
        /// file is obtained using malloc. While these pages may be reused in
        /// subsequent transactions, freshly malloc'd pages will be initialized
        /// to zeroes before use.This avoids persisting leftover data from other
        /// code(that used the heap and subsequently freed the memory) into the
        /// data file.Note that many other system libraries may allocate and free
        /// memory from the heap for arbitrary uses.E.g., stdio may use the heap
        /// for file I/O buffers. This initialization step has a modest performance
        /// cost so some applications may want to disable it using this flag.This
        /// option can be a problem for applications which handle sensitive data
        /// like passwords, and it makes memory checkers like Valgrind noisy. This
        /// flag is not needed with MDBX_WRITEMAP, which writes directly to the
        /// mmap instead of using malloc for pages.The initialization is also
        /// skipped if MDBX_RESERVE is used; the caller is expected to overwrite
        /// all of the memory that was reserved in that case. This flag may be
        /// changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMemInit = Constant.MDBX_NOMEMINIT,

        /// <summary>
        /// Aim to coalesce records while reclaiming FreeDB. This flag may be
        /// changed at any time using mdbx_env_set_flags().
        /// </summary>
        Coalesce = Constant.MDBX_COALESCE,

        /// <summary>
        /// LIFO policy for reclaiming FreeDB records. This significantly reduce
        /// write IPOs in case MDBX_NOSYNC with periodically checkpoints.
        /// </summary>
        LifoReclaim = Constant.MDBX_LIFORECLAIM,

        /// <summary>
        /// Open environment in exclusive/monopolistic mode.
        /// MDBX_EXCLUSIVE flag can be used as a replacement for MDB_NOLOCK, 
        /// which don't supported by MDBX. 
        /// In this way, you can get the minimal overhead, but with the correct multi-process and multi-thread locking.
        /// 
        /// with MDBX_EXCLUSIVE = open environment in exclusive/monopolistic mode or return MDBX_BUSY if environment already used by other process. 
        /// The main feature of the exclusive mode is the ability to open the environment placed on a network share.
        /// 
        /// without MDBX_EXCLUSIVE = open environment in cooperative mode, i.e. for multi-process access/interaction/cooperation. The main requirements of the cooperative mode are:
        /// data files MUST be placed in the LOCAL file system, but NOT on a network share.
        /// environment MUST be opened only by LOCAL processes, but NOT over a network.
        /// OS kernel (i.e. file system and memory mapping implementation) and all processes that open the given environment MUST be running in the physically single RAM with cache-coherency.
        /// The only exception for cache-consistency requirement is Linux on MIPS architecture, but this case has not been tested for a long time).
        /// This flag affects only at environment opening but can't be changed after.
        /// 
        /// </summary>
        Exclusive = Constant.MDBX_EXCLUSIVE,

        /// <summary>
        /// Using database/environment which already opened by another process(es).
        /// 
        /// The MDBX_ACCEDE flag is useful to avoid MDBX_INCOMPATIBLE error while opening the database/environment which is already used by another process(es) with unknown mode/flags. In such cases, if there is a difference in the specified flags (MDBX_NOMETASYNC, MDBX_SAFE_NOSYNC, MDBX_UTTERLY_NOSYNC, MDBX_LIFORECLAIM and MDBX_NORDAHEAD), instead of returning an error, the database will be opened in a compatibility with the already used mode.
        /// MDBX_ACCEDE has no effect if the current process is the only one either opening the DB in read-only mode or other process(es) uses the DB in read-only mode.
        /// 
        /// </summary>
        Accede = Constant.MDBX_ACCEDE,

        /// <summary>
        /// Отвязывает транзакции от потоков/threads насколько это возможно.
        ///
        /// Опция предназначена для приложений, которые мультиплексируют множество
        /// пользовательских легковесных потоков выполнения по отдельным потокам
        /// операционной системы, например как это происходит в средах выполнения
        /// GoLang и Rust. Таким приложениям также рекомендуется сериализовать
        /// транзакции записи в одном потоке операционной системы, поскольку блокировка
        /// записи MDBX использует базовые системные примитивы синхронизации и ничего
        /// не знает о пользовательских потоках и/или легковесных потоков среды
        /// выполнения. Как минимум, обязательно требуется обеспечить завершение каждой
        /// пишущей транзакции строго в том же потоке операционной системы где она была
        /// запущена.
        ///
        /// \note Начиная с версии v0.13 опция `MDBX_NOSTICKYTHREADS` полностью
        /// заменяет опцию \ref MDBX_NOTLS.
        ///
        /// При использовании `MDBX_NOSTICKYTHREADS` транзакции становятся не
        /// ассоциированными с создавшими их потоками выполнения. Поэтому в функциях
        /// API не выполняется проверка соответствия транзакции и текущего потока
        /// выполнения. Большинство функций работающих с транзакциями и курсорами
        /// становится возможным вызывать из любых потоков выполнения. Однако, также
        /// становится невозможно обнаружить ошибки одновременного использования
        /// транзакций и/или курсоров в разных потоках.
        ///
        /// Использование `MDBX_NOSTICKYTHREADS` также сужает возможности по изменению
        /// размера БД, так как теряется возможность отслеживать работающие с БД потоки
        /// выполнения и приостанавливать их на время снятия отображения БД в ОЗУ. В
        /// частности, по этой причине на Windows уменьшение файла БД не возможно до
        /// закрытия БД последним работающим с ней процессом или до последующего
        /// открытия БД в режиме чтения-записи.
        ///
        /// \warning Вне зависимости от \ref MDBX_NOSTICKYTHREADS и \ref MDBX_NOTLS не
        /// допускается одновременно использование объектов API из разных потоков
        /// выполнения! Обеспечение всех мер для исключения одновременного
        /// использования объектов API из разных потоков выполнения целиком ложится на
        /// вас!
        ///
        /// \warning Транзакции записи могут быть завершены только в том же потоке
        /// выполнения где они были запущены. Это ограничение следует из требований
        /// большинства операционных систем о том, что захваченный примитив
        /// синхронизации (мьютекс, семафор, критическая секция) должен освобождаться
        /// только захватившим его потоком выполнения.
        ///
        /// \warning Создание курсора в контексте транзакции, привязка курсора к
        /// транзакции, отвязка курсора от транзакции и закрытие привязанного к
        /// транзакции курсора, являются операциями использующими как сам курсор так и
        /// соответствующую транзакцию. Аналогично, завершение или прерывание
        /// транзакции является операцией использующей как саму транзакцию, так и все
        /// привязанные к ней курсоры. Во избежание повреждения внутренних структур
        /// данных, непредсказуемого поведения, разрушение БД и потери данных следует
        /// не допускать возможности одновременного использования каких-либо курсора
        /// или транзакций из разных потоков выполнения.
        ///
        /// Читающие транзакции при использовании `MDBX_NOSTICKYTHREADS` перестают
        /// использовать TLS (Thread Local Storage), а слоты блокировок MVCC-снимков в
        /// таблице читателей привязываются только к транзакциям. Завершение каких-либо
        /// потоков не приводит к снятию блокировок MVCC-снимков до явного завершения
        /// транзакций, либо до завершения соответствующего процесса в целом.
        ///
        /// Для пишущих транзакций не выполняется проверка соответствия текущего потока
        /// выполнения и потока создавшего транзакцию. Однако, фиксация или прерывание
        /// пишущих транзакций должны выполняться строго в потоке запустившим
        /// транзакцию, так как эти операции связаны с захватом и освобождением
        /// примитивов синхронизации (мьютексов, критических секций), для которых
        /// большинство операционных систем требует освобождение только потоком
        /// захватившим ресурс.
        ///
        /// Этот флаг вступает в силу при открытии среды и не может быть изменен после.
        ///
        /// </summary>
        NoStickyThreads = Constant.MDBX_NOSTICKYTHREADS,

        /// <summary>
        /// Debugging option, fill/perturb released pages.
        /// </summary>
        PagePerTurb = Constant.MDBX_PAGEPERTURB,

        /// </summary>
        /// <summary>
        /// /* SYNC MODES****************************************************************/
        /// \defgroup sync_modes SYNC MODES
        /// 
        /// \attention Using any combination of \ref MDBX_SAFE_NOSYNC, \ref
        /// MDBX_NOMETASYNC and especially \ref MDBX_UTTERLY_NOSYNC is always a deal to
        /// reduce durability for gain write performance. You must know exactly what
        /// you are doing and what risks you are taking!
        /// 
        /// \note for LMDB users: \ref MDBX_SAFE_NOSYNC is NOT similar to LMDB_NOSYNC,
        /// but \ref MDBX_UTTERLY_NOSYNC is exactly match LMDB_NOSYNC. See details
        /// below.
        /// 
        /// THE SCENE:
        /// - The DAT-file contains several MVCC-snapshots of B-tree at same time,
        ///   each of those B-tree has its own root page.
        /// - Each of meta pages at the beginning of the DAT file contains a
        ///   pointer to the root page of B-tree which is the result of the particular
        ///   transaction, and a number of this transaction.
        /// - For data durability, MDBX must first write all MVCC-snapshot data
        ///   pages and ensure that are written to the disk, then update a meta page
        ///   with the new transaction number and a pointer to the corresponding new
        ///   root page, and flush any buffers yet again.
        /// - Thus during commit a I/O buffers should be flushed to the disk twice;
        ///   i.e. fdatasync(), FlushFileBuffers() or similar syscall should be
        ///   called twice for each commit. This is very expensive for performance,
        ///   but guaranteed durability even on unexpected system failure or power
        ///   outage. Of course, provided that the operating system and the
        ///   underlying hardware (e.g. disk) work correctly.
        /// 
        /// TRADE-OFF:
        /// By skipping some stages described above, you can significantly benefit in
        /// speed, while partially or completely losing in the guarantee of data
        /// durability and/or consistency in the event of system or power failure.
        /// Moreover, if for any reason disk write order is not preserved, then at
        /// moment of a system crash, a meta-page with a pointer to the new B-tree may
        /// be written to disk, while the itself B-tree not yet. In that case, the
        /// database will be corrupted!
        /// 
        /// \see MDBX_SYNC_DURABLE \see MDBX_NOMETASYNC \see MDBX_SAFE_NOSYNC
        /// \see MDBX_UTTERLY_NOSYNC
        /// 
        /// @* /
        /// </summary>

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
        SyncDurable = Constant.MDBX_SYNC_DURABLE,

        /// </summary>
        ////  
        ///  Don't sync anything but keep previous steady commits.
        ///  
        ///  Like \ref MDBX_UTTERLY_NOSYNC the `MDBX_SAFE_NOSYNC` flag disable similarly
        ///  flush system buffers to disk when committing a transaction. But there is a
        ///  huge difference in how are recycled the MVCC snapshots corresponding to
        ///  previous "steady" transactions (see below).
        ///  
        ///  With \ref MDBX_WRITEMAP the `MDBX_SAFE_NOSYNC` instructs MDBX to use
        ///  asynchronous mmap-flushes to disk. Asynchronous mmap-flushes means that
        ///  actually all writes will scheduled and performed by operation system on it
        ///  own manner, i.e. unordered. MDBX itself just notify operating system that
        ///  it would be nice to write data to disk, but no more.
        ///  
        ///  Depending on the platform and hardware, with `MDBX_SAFE_NOSYNC` you may get
        ///  a multiple increase of write performance, even 10 times or more.
        ///  
        ///  In contrast to \ref MDBX_UTTERLY_NOSYNC mode, with `MDBX_SAFE_NOSYNC` flag
        ///  MDBX will keeps untouched pages within B-tree of the last transaction
        ///  "steady" which was synced to disk completely. This has big implications for
        ///  both data durability and (unfortunately) performance:
        ///   - a system crash can't corrupt the database, but you will lose the last
        ///     transactions; because MDBX will rollback to last steady commit since it
        ///     kept explicitly.
        ///   - the last steady transaction makes an effect similar to "long-lived" read
        ///     transaction (see above in the \ref restrictions section) since prevents
        ///     reuse of pages freed by newer write transactions, thus the any data
        ///     changes will be placed in newly allocated pages.
        ///   - to avoid rapid database growth, the system will sync data and issue
        ///     a steady commit-point to resume reuse pages, each time there is
        ///     insufficient space and before increasing the size of the file on disk.
        ///  
        ///  In other words, with `MDBX_SAFE_NOSYNC` flag MDBX insures you from the
        ///  whole database corruption, at the cost increasing database size and/or
        ///  number of disk IOPs. So, `MDBX_SAFE_NOSYNC` flag could be used with
        ///  \ref mdbx_env_sync() as alternatively for batch committing or nested
        ///  transaction (in some cases). As well, auto-sync feature exposed by
        ///  \ref mdbx_env_set_syncbytes() and \ref mdbx_env_set_syncperiod() functions
        ///  could be very useful with `MDBX_SAFE_NOSYNC` flag.
        ///  
        ///  The number and volume of disk IOPs with MDBX_SAFE_NOSYNC flag will
        ///  exactly the as without any no-sync flags. However, you should expect a
        ///  larger process's [work set](https://bit.ly/2kA2tFX) and significantly worse
        ///  a [locality of reference](https://bit.ly/2mbYq2J), due to the more
        ///  intensive allocation of previously unused pages and increase the size of
        ///  the database.
        ///  
        ///  `MDBX_SAFE_NOSYNC` flag may be changed at any time using
        ///  \ref mdbx_env_set_flags() or by passing to \ref mdbx_txn_begin() for
        ///  particular write transaction. 
        /// </summary>
        SafeNoSync = Constant.MDBX_SAFE_NOSYNC,

        /// </summary>
        /// Don't sync anything and wipe previous steady commits.
        ///
        /// Don't flush system buffers to disk when committing a transaction. This
        /// optimization means a system crash can corrupt the database, if buffers are
        /// not yet flushed to disk. Depending on the platform and hardware, with
        /// `MDBX_UTTERLY_NOSYNC` you may get a multiple increase of write performance,
        /// even 100 times or more.
        ///
        /// If the filesystem preserves write order (which is rare and never provided
        /// unless explicitly noted) and the \ref MDBX_WRITEMAP and \ref
        /// MDBX_LIFORECLAIM flags are not used, then a system crash can't corrupt the
        /// database, but you can lose the last transactions, if at least one buffer is
        /// not yet flushed to disk. The risk is governed by how often the system
        /// flushes dirty buffers to disk and how often \ref mdbx_env_sync() is called.
        /// So, transactions exhibit ACI (atomicity, consistency, isolation) properties
        /// and only lose `D` (durability). I.e. database integrity is maintained, but
        /// a system crash may undo the final transactions.
        ///
        /// Otherwise, if the filesystem not preserves write order (which is
        /// typically) or \ref MDBX_WRITEMAP or \ref MDBX_LIFORECLAIM flags are used,
        /// you should expect the corrupted database after a system crash.
        ///
        /// So, most important thing about `MDBX_UTTERLY_NOSYNC`:
        ///  - a system crash immediately after commit the write transaction
        ///    high likely lead to database corruption.
        ///  - successful completion of mdbx_env_sync(force = true) after one or
        ///    more committed transactions guarantees consistency and durability.
        ///  - BUT by committing two or more transactions you back database into
        ///    a weak state, in which a system crash may lead to database corruption!
        ///    In case single transaction after mdbx_env_sync, you may lose transaction
        ///    itself, but not a whole database.
        ///
        /// Nevertheless, `MDBX_UTTERLY_NOSYNC` provides "weak" durability in case
        /// of an application crash (but no durability on system failure), and
        /// therefore may be very useful in scenarios where data durability is
        /// not required over a system failure (e.g for short-lived data), or if you
        /// can take such risk.
        ///
        /// `MDBX_UTTERLY_NOSYNC` flag may be changed at any time using
        /// \ref mdbx_env_set_flags(), but don't has effect if passed to
        /// \ref mdbx_txn_begin() for particular write transaction. \see sync_modes */
        /// </summary>
        UtterlyNoSync = Constant.MDBX_UTTERLY_NOSYNC,

    }
    
}
