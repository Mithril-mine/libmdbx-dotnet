/// <summary>
/// Константы MDBX.
/// </summary>
internal static class Constant
{
    /// <summary>
    /// Среда не создаст поддиректорию для данных.
    /// </summary>
    public const int MDBX_NOSUBDIR = 0x4000;

    /// <summary>
    /// Не выполнять fsync после фиксации транзакции.
    /// </summary>
    public const int MDBX_NOSYNC = 0x10000;

    /// <summary>
    /// Открыть среду только для чтения.
    /// </summary>
    public const int MDBX_RDONLY = 0x20000;

    /// <summary>
    /// Не выполнять fsync мета-страниц после фиксации транзакции.
    /// </summary>
    public const int MDBX_NOMETASYNC = 0x40000;

    /// <summary>
    /// Использовать изменяемое отображение в память (writable mmap).
    /// </summary>
    public const int MDBX_WRITEMAP = 0x80000;

    /// <summary>
    /// Использовать асинхронный msync при использовании MDBX_WRITEMAP.
    /// </summary>
    public const int MDBX_MAPASYNC = 0x100000;

    /// <summary>
    /// Не использовать локальное хранилище потоков (Thread-Local Storage).
    /// Связывать слоты блокировок читателей с объектами MDBX_txn вместо потоков.
    /// </summary>
    public const int MDBX_NOTLS = 0x200000;

    /// <summary>
    /// Открыть базу данных/среду в эксклюзивном/монопольном режиме.
    /// </summary>
    public const int MDBX_EXCLUSIVE = 0x400000;

    /// <summary>
    /// Отключить readahead (предвыборку данных).
    /// </summary>
    public const int MDBX_NORDAHEAD = 0x800000;

    /// <summary>
    /// Не инициализировать память, выделенную через malloc, перед записью в файл данных.
    /// </summary>
    public const int MDBX_NOMEMINIT = 0x1000000;

    /// <summary>
    /// Стремиться объединять (coalesce) записи в FreeDB при освобождении.
    /// </summary>
    public const int MDBX_COALESCE = 0x2000000;

    /// <summary>
    /// Политика LIFO (Last In, First Out) для освобождения записей в FreeDB.
    /// </summary>
    public const int MDBX_LIFORECLAIM = 0x4000000;

    /// <summary>
    /// Выполнять steady-sync только при закрытии и явном env-sync.
    /// Комбинация MDBX_NOSYNC | MDBX_MAPASYNC.
    /// </summary>
    public const int MDBX_UTTERLY_NOSYNC = (MDBX_NOSYNC | MDBX_MAPASYNC);

    /// <summary>
    /// Опция отладки; заполнять/возмущать освобождаемые страницы памяти.
    /// </summary>
    public const int MDBX_PAGEPERTURB = 0x8000000;

    /// <summary>
    /// Не блокироваться при запуске пишущей транзакции.
    /// </summary>
    public const int MDBX_TRYTXN = 0x10000000;

    /// <summary>
    /// Использовать обратные (реверсивные) строковые ключи.
    /// </summary>
    public const int MDBX_REVERSEKEY = 0x02;

    /// <summary>
    /// Использовать отсортированные дубликаты ключей.
    /// </summary>
    public const int MDBX_DUPSORT = 0x04;

    /// <summary>
    /// Ключи - это двоичные целые числа в нативном порядке байт (uint32_t или uint64_t).
    /// Все ключи должны быть одинакового размера.
    /// </summary>
    public const int MDBX_INTEGERKEY = 0x08;

    /// <summary>
    /// С MDBX_DUPSORT: элементы дубликатов имеют фиксированный размер.
    /// </summary>
    public const int MDBX_DUPFIXED = 0x10;

    /// <summary>
    /// С MDBX_DUPSORT: дубликаты - это целые числа в стиле MDBX_INTEGERKEY.
    /// </summary>
    public const int MDBX_INTEGERDUP = 0x20;

    /// <summary>
    /// С MDBX_DUPSORT: использовать обратные строковые дубликаты.
    /// </summary>
    public const int MDBX_REVERSEDUP = 0x40;

    /// <summary>
    /// Создать базу данных, если она не существует.
    /// </summary>
    public const int MDBX_CREATE = 0x40000;

    /// <summary>
    /// Для mdbx_put: не записывать, если ключ уже существует.
    /// </summary>
    public const int MDBX_NOOVERWRITE = 0x10;

    /// <summary>
    /// Для mdbx_put с MDBX_DUPSORT: не записывать, если пара ключ/данные уже существует.
    /// Для mdbx_cursor_del: удалить все элементы дубликатов.
    /// </summary>
    public const int MDBX_NODUPDATA = 0x20;

    /// <summary>
    /// Для mdbx_cursor_put: перезаписать текущую пару ключ/данные.
    /// </summary>
    public const int MDBX_CURRENT = 0x40;

    /// <summary>
    /// Для put: только зарезервировать место для данных, не копировать их.
    /// Вернуть указатель на зарезервированное пространство.
    /// </summary>
    public const int MDBX_RESERVE = 0x10000;

    /// <summary>
    /// Данные добавляются в конец, не разбивать полные страницы.
    /// </summary>
    public const int MDBX_APPEND = 0x20000;

    /// <summary>
    /// Дублирующие данные добавляются в конец, не разбивать полные страницы.
    /// </summary>
    public const int MDBX_APPENDDUP = 0x40000;

    /// <summary>
    /// Хранить несколько элементов данных в одном вызове. Только для MDBX_DUPFIXED.
    /// </summary>
    public const int MDBX_MULTIPLE = 0x80000;

    /// <summary>
    /// Использовать базу данных/среду, которая уже открыта другим(и) процессом(ами).
    /// </summary>
    public const int MDBX_ACCEDE = 0x40000000;

    /// <summary>
    /// Отвязывать транзакции от потоков насколько это возможно.
    /// </summary>
    public const int MDBX_NOSTICKYTHREADS = 0x200000;

    /// <summary>
    /// Режим синхронизации по умолчанию: надёжный и долговечный.
    /// Метаданные записываются и сбрасываются на диск после записи и сброса данных,
    /// что гарантирует целостность базы данных при сбое в любое время.
    ///
    /// Внимание: не используйте другие режимы, пока вы не изучили все детали
    /// и не уверены. В противном случае вы можете потерять данные пользователей,
    /// как это произошло в мессенджере Miranda NG.
    /// </summary>
    public const int MDBX_SYNC_DURABLE = 0;

    /// <summary>
    /// Начать транзакцию чтения-записи.
    /// Только одна транзакция записи может быть активна в один момент времени.
    /// </summary>
    public const int MDBX_TXN_READWRITE = 0;

    /// <summary>
    /// Не синхронизировать ничего, но сохранять предыдущие steady-коммиты.
    /// Флагsafe nosync отключает сброс буферов при фиксации, но сохраняет последний steady-коммит.
    /// </summary>
    public const int MDBX_SAFE_NOSYNC = 0x10000;

    /// <summary>
    /// Подготовить, но не начать транзакцию только для чтения.
    /// Транзакция не будет запущена сразу, но дескриптор будет готов для mdbx_txn_renew().
    /// </summary>
    public const int MDBX_TXN_RDONLY_PREPARE = MDBX_RDONLY | MDBX_NOMEMINIT;

    /// <summary>
    /// Дополнительная валидация структуры БД и содержимого страниц.
    /// Включает безопасный режим работы с повреждённой или ненадёжной БД.
    /// </summary>
    public const int MDBX_VALIDATION = 0x00002000;

    /// <summary>
    /// Транзакция только на чтение.
    /// </summary>
    public const int MDBX_TXN_RDONLY = MDBX_RDONLY;

    /// <summary>
    /// Не блокировать при запуске транзакции записи.
    /// </summary>
    public const int MDBX_TXN_TRY = 0x10000000;

    /// <summary>
    /// Не снижать надежность транзакции, а использовать режим работы среды.
    /// </summary>
    public const int MDBX_TXN_NOWEAKING = 0;
}