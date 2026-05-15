/// <summary>
/// Константы MDBX.
/// </summary>
internal static class Constants
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


    /// <summary>
    /// Success.
    /// </summary>
    public const int MDBX_SUCCESS = 0;
    /// <summary>
    /// False result.
    /// </summary>
    public const int MDBX_RESULT_FALSE = 0;

    /// <summary>
    /// True result.
    /// </summary>
    public const int MDBX_RESULT_TRUE = -1;

    /* key/data pair already exists */
    /// <summary>
    /// Ключ/значение пара уже существует.
    /// </summary>
    public const int MDBX_KEYEXIST = -30799;

    /* key/data pair not found (EOF) */
    /// <summary>
    /// Ключ/значение пара не найдена (EOF).
    /// </summary>
    public const int MDBX_NOTFOUND = -30798;

    /* Requested page not found - this usually indicates corruption */
    /// <summary>
    /// Требуемая страница не найдена - обычно указывает на повреждение.
    /// </summary>
    public const int MDBX_PAGE_NOTFOUND = -30797;

    /* Located page was wrong type */
    /// <summary>
    /// Найденная страница имеет неправильный тип.
    /// </summary>
    public const int MDBX_CORRUPTED = -30796;

    /* Update of meta page failed or environment had fatal error */
    /// <summary>
    /// Обновление метаинформации не удалось или произошла критическая ошибка.
    /// </summary>
    public const int MDBX_PANIC = -30795;

    /* DB file version mismatch with libmdbx */
    /// <summary>
    /// Несоответствие версии файла БД с libmdbx.
    /// </summary>
    public const int MDBX_VERSION_MISMATCH = -30794;

    /* File is not a valid MDBX file */
    /// <summary>
    /// Файл не является действительным файлом MDBX.
    /// </summary>
    public const int MDBX_INVALID = -30793;
    /// <summary>
    /// Достигнут максимальный размер отображения среды.
    /// </summary>
    public const int MDBX_MAP_FULL = -30792;
    /// <summary>
    /// Достигнуто максимальное количество баз данных в среде.
    /// </summary>
    public const int MDBX_DBS_FULL = -30791;
    /// <summary>
    /// Достигнуто максимальное количество слотов для читателей.
    /// </summary>
    public const int MDBX_READERS_FULL = -30790;
    /// <summary>
    /// В транзакции слишком много грязных страниц.
    /// </summary>
    public const int MDBX_TXN_FULL = -30788;
    /// <summary>
    /// Стек курсора слишком глубок - внутренняя ошибка.
    /// </summary>
    public const int MDBX_CURSOR_FULL = -30787;
    /// <summary>
    /// В странице недостаточно места - внутренняя ошибка.
    /// </summary>
    public const int MDBX_PAGE_FULL = -30786;
    /// <summary>
    /// Содержимое базы данных превысило размер отображения среды.
    /// </summary>
    public const int MDBX_MAP_RESIZED = -30785;
    /// <summary>
    /// Операция и база данных несовместимы, или тип базы данных изменился. Это может означать:
    ///  - Операция ожидает базу данных с флагом MDBX_DUPSORT / MDBX_DUPFIXED.
    ///  - Открытие именованной базы, когда безымянная база имеет флаги MDBX_DUPSORT/MDBX_INTEGERKEY.
    ///  - Доступ к записи данных как к базе данных или наоборот.
    ///  - База данных была удалена и пересоздана с другими флагами. */
    /// </summary>
    public const int MDBX_INCOMPATIBLE = -30784;
    /// <summary>
    /// Некорректное повторное использование слота таблицы блокировок читателя.
    /// </summary>
    public const int MDBX_BAD_RSLOT = -30783;
    /// <summary>
    /// Транзакция должна быть прервана, имеет дочернюю транзакцию или является некорректной.
    /// </summary>
    public const int MDBX_BAD_TXN = -30782;
    /// <summary>
    /// Некорректный размер ключа/имени БД/данных или неверный размер DUPFIXED.
    /// </summary>
    public const int MDBX_BAD_VALSIZE = -30781;
    /// <summary>
    /// Указанный DBI был неожиданно изменён.
    /// </summary>
    public const int MDBX_BAD_DBI = -30780;
    /// <summary>
    /// Непредвиденная проблема - транзакция должна быть прервана.
    /// </summary>
    public const int MDBX_PROBLEM = -30779;
    /// <summary>
    /// Другая пишущая транзакция уже выполняется.
    /// </summary>
    public const int MDBX_BUSY = -30778;
    /// <summary>
    /// Последний определённый код ошибки.
    /// </summary>
    public const int MDBX_LAST_ERRCODE = -30778;

    /// <summary>
    /// mdbx_put() или mdbx_replace() был вызван для ключа,
    /// который имеет более одного связанного значения. */
    /// </summary>
    public const int MDBX_EMULTIVAL = -30421;

    /// <summary>
    /// Плохая сигнатура рантайм-объекта(ов), это может означать:
    ///  - повреждение памяти или двойное освобождение;
    ///  - несоответствие ABI версий (редкий случай); */
    /// </summary>
    public const int MDBX_EBADSIGN = -30420;

    /// <summary>
    /// База данных должна быть восстановлена, но это НЕ может быть сделано автоматически
    /// прямо сейчас (например, в режиме только для чтения и т.п.). */
    /// </summary>
    public const int MDBX_WANNA_RECOVERY = -30419;

    /// <summary>
    /// Значение данного ключа не соответствует текущей позиции курсора,
    /// когда mdbx_cursor_put() вызывается с опцией MDBX_CURRENT. */
    /// </summary>
    public const int MDBX_EKEYMISMATCH = -30418;

    /// <summary>
    /// База данных слишком большая для текущей системы,
    /// например, не может быть отображена в оперативную память. */
    /// </summary>
    public const int MDBX_TOO_LARGE = -30417;

    /// <summary>
    /// Поток попытался использовать объект, которому он не принадлежит,
    /// например, транзакцию, запущенную другим потоком. */
    /// </summary>
    public const int MDBX_THREAD_MISMATCH = -30416;
}