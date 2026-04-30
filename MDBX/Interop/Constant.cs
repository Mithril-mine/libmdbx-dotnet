
namespace MDBX.Interop;

/// <summary>
/// Constants.
/// </summary>
internal static class Constant
{
    /// <summary>
    /// Отсутствие директории для среды.
    /// </summary>
    public const int MDBX_NOSUBDIR = 0x4000;

    /// <summary>
    /// Не выполнять fsync после фиксации.
    /// </summary>
    public const int MDBX_NOSYNC = 0x10000;

    /// <summary>
    /// Только для чтения.
    /// </summary>
    public const int MDBX_RDONLY = 0x20000;

    /// <summary>
    /// Не выполнять fsync метастраниц после фиксации.
    /// </summary>
    public const int MDBX_NOMETASYNC = 0x40000;

    /// <summary>
    /// Использовать writable mmap.
    /// </summary>
    public const int MDBX_WRITEMAP = 0x80000;

    /// <summary>
    /// Использовать асинхронный msync когда MDBX_WRITEMAP используется.
    /// </summary>
    public const int MDBX_MAPASYNC = 0x100000;

    /// <summary>
    /// Привязывать слоты таблицы блокировок читателей к объектам MDBX_txn вместо потоков.
    /// </summary>
    public const int MDBX_NOTLS = 0x200000;

    /// <summary>
    /// Открыть БД в эксклюзивном/монопольном режиме.
    /// </summary>
    public const int MDBX_EXCLUSIVE = 0x400000;

    /// <summary>
    /// Не выполнять readahead.
    /// </summary>
    public const int MDBX_NORDAHEAD = 0x800000;

    /// <summary>
    /// Не инициализировать память, выделенную через malloc, перед записью в файл данных.
    /// </summary>
    public const int MDBX_NOMEMINIT = 0x1000000;

    /// <summary>
    /// Стремиться coalesce записи в FreeDB.
    /// </summary>
    public const int MDBX_COALESCE = 0x2000000;

    /// <summary>
    /// Политика LIFO для освобождения записей в FreeDB.
    /// </summary>
    public const int MDBX_LIFORECLAIM = 0x4000000;

    /// <summary>
    /// Выполнять steady-sync только при закрытии и явном env-sync.
    /// </summary>
    public const int MDBX_UTTERLY_NOSYNC = (MDBX_NOSYNC | MDBX_MAPASYNC);

    /// <summary>
    /// Опция отладки; заполнять/возмущать освобождаемые страницы.
    /// </summary>
    public const int MDBX_PAGEPERTURB = 0x8000000;

    /// <summary>
    /// Не блокироваться при запуске пишущей транзакции.
    /// </summary>
    public const int MDBX_TRYTXN = 0x10000000;

    /// <summary>
    /// Использовать обратные строковые ключи.
    /// </summary>
    public const int MDBX_REVERSEKEY = 0x02;

    /// <summary>
    /// Использовать отсортированные дубликаты.
    /// </summary>
    public const int MDBX_DUPSORT = 0x04;

    /// <summary>
    /// Числовые ключи в нативном порядке байт, либо uint32_t либо uint64_t.
    /// Все ключи должны быть одинакового размера.
    /// </summary>
    public const int MDBX_INTEGERKEY = 0x08;

    /// <summary>
    /// С MDBX_DUPSORT, отсортированные элементы дубликатов имеют фиксированный размер.
    /// </summary>
    public const int MDBX_DUPFIXED = 0x10;

    /// <summary>
    /// С MDBX_DUPSORT, дубликаты - это целые числа в стиле MDBX_INTEGERKEY.
    /// </summary>
    public const int MDBX_INTEGERDUP = 0x20;

    /// <summary>
    /// С MDBX_DUPSORT, использовать обратные строковые дубликаты.
    /// </summary>
    public const int MDBX_REVERSEDUP = 0x40;

    /// <summary>
    /// Создать БД если она не существует.
    /// </summary>
    public const int MDBX_CREATE = 0x40000;


    /// <summary>
    /// Для put: не записывать если ключ уже существует.
    /// </summary>
    public const int MDBX_NOOVERWRITE = 0x10;

    /// <summary>
    /// Только для MDBX_DUPSORT. Для put: не записывать если пара ключ/данные уже существует.
    /// Для mdbx_cursor_del: удалить все элементы дубликатов.
    /// </summary>
    public const int MDBX_NODUPDATA = 0x20;

    /// <summary>
    /// Для mdbx_cursor_put: перезаписать текущую пару ключ/данные.
    /// MDBX позволяет этот флаг для mdbx_put() для явного перезаписи/обновления без вставки.
    /// </summary>
    public const int MDBX_CURRENT = 0x40;

    /// <summary>
    /// Для put: только зарезервировать место для данных, не копировать их.
    /// Вернуть указатель на зарезервированное пространство.
    /// </summary>
    public const int MDBX_RESERVE = 0x10000;

    /// <summary>
    /// Данные добавляются, не разбивать полные страницы.
    /// </summary>
    public const int MDBX_APPEND = 0x20000;

    /// <summary>
    /// Дублирующие данные добавляются, не разбивать полные страницы.
    /// </summary>
    public const int MDBX_APPENDDUP = 0x40000;

    /// <summary>
    /// Хранить несколько элементов данных в одном вызове. Только для MDBX_DUPFIXED.
    /// </summary>
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
    /// /* РЕЖИМЫ СИНХРОНИЗАЦИ****************************************************************/
    ///* \defgroup sync_modes РЕЖИМЫ СИНХРОНИЗАЦИИ (SYNC MODES)
    ///
    /// \attention Использование любой комбинации \ref MDBX_SAFE_NOSYNC, \ref
    /// MDBX_NOMETASYNC и особенно \ref MDBX_UTTERLY_NOSYNC всегда является сделкой
    /// для уменьшения долговечности ради увеличения производительности записи.
    /// Вы должны точно знать, что делаете и какие риски принимаете!
    ///
    /// \note для пользователей LMDB: \ref MDBX_SAFE_NOSYNC НЕ аналогичен LMDB_NOSYNC,
    /// но \ref MDBX_UTTERLY_NOSYNC точно соответствует LMDB_NOSYNC. Смотрите детали
    /// ниже.
    ///
    /// СЦЕНАРИЙ:
    /// - DAT-файл содержит несколько MVCC-снимков B-дерева одновременно,
    ///   каждое из этих B-деревьев имеет свою корневую страницу.
    /// - Каждая из мета-страниц в начале DAT-файла содержит
    ///   указатель на корневую страницу B-дерева, которое является результатом конкретной
    ///   транзакции, и номер этой транзакции.
    /// - Для долговечности данных, MDBX сначала должна записать все страницы данных
    ///   MVCC-снимка и убедиться, что они записаны на диск, затем обновить мета-страницу
    ///   новым номером транзакции и указателем на соответствующее новое
    ///   корневое дерево, и снова сбросить все буферы.
    /// - Таким образом, при фиксации буферы ввода-вывода должны быть сброшены на диск дважды;
    ///   т.е. fdatasync(), FlushFileBuffers() или аналогичный системный вызов должен быть
    ///   вызван дважды для каждой фиксации. Это очень дорого для производительности,
    ///   но гарантирует долговечность даже при неожиданном сбое системы или отключении
    ///   питания. Конечно, при условии, что операционная система и
    ///   базовое оборудование (например, диск) работают корректно.
    ///
    /// КОМПРОМИСС:
    /// Пропуская некоторые этапы, описанные выше, вы можете значительно выиграть в
    /// скорости, при этом частично или полностью теряя гарантию долговечности
    /// данных и/или согласованности при сбое системы или питания.
    /// Более того, если по какой-либо причине порядок записи на диск не сохраняется, то в
    /// момент сбоя системы, мета-страница с указателем на новое B-дерево может
    /// быть записана на диск, а само B-дерево еще нет. В этом случае база
    /// данных будет повреждена!
    ///
    /// \see MDBX_SYNC_DURABLE \see MDBX_NOMETASYNC \see MDBX_SAFE_NOSYNC
    /// \see MDBX_UTTERLY_NOSYNC
    ///
    /// @{ */
    ///
    ///* РЕЖИМ ПО УМОЛЧАНИЮ: надёжный и долговечный режим синхронизации.
    ///
    /// Метаданные записываются и сбрасываются на диск после записи и сброса
    /// данных, что гарантирует целостность базы данных при
    /// сбое в любое время.
    ///
    /// \attention Пожалуйста, не используйте другие режимы, пока вы не изучили все
    /// детали и не уверены. В противном случае вы можете потерять данные ваших пользователей, как это
    /// произошло в мессенджере [Miranda NG](https://www.miranda-ng.org/). */
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
    /// Transaction will not be started immediately, but created transaction handle will be ready for use with mdbx_txn_renew().
    /// This flag allows to preallocate memory and assign a reader slot, thus avoiding these operations at the next start of the transaction.
    /// </summary>
    public const int MDBX_TXN_RDONLY_PREPARE = MDBX_RDONLY | MDBX_NOMEMINIT;


    /** 
      * Extra validation of DB structure and pages content.
      *
      * The `MDBX_VALIDATION` enabled the simple safe/careful mode for working
      * with damaged or untrusted DB. However, a notable performance
      * degradation should be expected. 
      */
    public const int MDBX_VALIDATION = 0x00002000;
}
