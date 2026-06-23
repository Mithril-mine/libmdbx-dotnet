namespace MDBX.Native.Types;

/// <summary>
/// Флаги копирования окружения.
/// </summary>
[Flags]
public enum MdbxCopyFlags : uint
{
    /// <summary>
    /// Копировать без сжатия.
    /// </summary>
    MDBX_CP_DEFAULT = 0x00000000,

    /// <summary>
    /// Сжимать данные при копировании.
    /// </summary>
    MDBX_CP_COMPACT = 0x00000001
}

/// <summary>
/// Режим удаления окружения.
/// </summary>
public enum MdbxEnvDeleteMode : uint
{
    /// <summary>
    /// Только удалить данные, но оставить файлы.
    /// </summary>
    MDBX_ENV_DELETE_DATA = 0,

    /// <summary>
    /// Удалить все файлы окружения.
    /// </summary>
    MDBX_ENV_DELETE_ALL = 1
}

/// <summary>
/// Флаги прогрева окружения.
/// </summary>
[Flags]
public enum MdbxWarmupFlags : uint
{
    /// <summary>
    /// Флаги по умолчанию.
    /// </summary>
    MDBX_WARMUP_DEFAULT = 0x00000000,

    /// <summary>
    /// Прогреть страницы данных.
    /// </summary>
    MDBX_WARMUP_DATA = 0x00000001,

    /// <summary>
    /// Прогреть страницы метаданных.
    /// </summary>
    MDBX_WARMUP_META = 0x00000002
}

/// <summary>
/// Параметры окружения.
/// </summary>
public enum MdbxOption : int
{
    /// <summary>
    /// Максимальное количество именованных таблиц для окружения.
    /// </summary>
    MDBX_OPT_MAX_DB = 0,

    /// <summary>
    /// Максимальное количество потоков/слотов читателей.
    /// </summary>
    MDBX_OPT_MAX_READERS = 1,

    /// <summary>
    /// Порог принудительного сброса буферов данных на диск.
    /// </summary>
    MDBX_OPT_SYNC_BYTES = 2,

    /// <summary>
    /// Относительный период с момента последней нестабильной фиксации для принудительного сброса.
    /// </summary>
    MDBX_OPT_SYNC_PERIOD = 3,

    /// <summary>
    /// Лимит наращивания списка освобожденных/рециклированных номеров страниц.
    /// </summary>
    MDBX_OPT_RP_AUGMENT_LIMIT = 4,

    /// <summary>
    /// Лимит кэша грязных страниц для повторного использования в текущей транзакции.
    /// </summary>
    MDBX_OPT_LOOSE_LIMIT = 5,

    /// <summary>
    /// Лимит предварительно выделенных элементов памяти для грязных страниц.
    /// </summary>
    MDBX_OPT_DUPLIMIT_LIMIT = 6,

    /// <summary>
    /// Включить/отключить mincore().
    /// </summary>
    MDBX_OPT_OP_MINCORE = 7,

    /// <summary>
    /// Включить prefault запись.
    /// </summary>
    MDBX_OPT_OP_PREFAULT_WRITE = 8,

    /// <summary>
    /// Включить метасинхронизацию.
    /// </summary>
    MDBX_OPT_OP_METASYNC = 9,

    /// <summary>
    /// Отключить файловую блокировку.
    /// </summary>
    MDBX_OPT_OP_NOLOCK = 10,

    /// <summary>
    /// Совместимость со старым интерфейсом.
    /// </summary>
    MDBX_OPT_OP_COMPAT = 11,

    /// <summary>
    /// Настройка планировщика.
    /// </summary>
    MDBX_OPT_OP_SCHEDULER = 12,

    /// <summary>
    /// Ограничение размера файла файловой системой.
    /// </summary>
    MDBX_OPT_OP_FSLIMIT = 13,

    /// <summary>
    /// Шаблон памяти.
    /// </summary>
    MDBX_OPT_OP_MEMTEMPLATE = 14,

    /// <summary>
    /// Количество таблиц.
    /// </summary>
    MDBX_OPT_NUMDBS = 15,

    /// <summary>
    /// Политика повторного использования читательских слотов.
    /// </summary>
    MDBX_OPT_OP_RP_REINVOKE = 16,

    /// <summary>
    /// Включить уникальные идентификаторы.
    /// </summary>
    MDBX_OPT_OP_UID = 17,

    /// <summary>
    /// Включить Unified Page Cache блоки.
    /// </summary>
    MDBX_OPT_OP_UPCBLK = 18,

    /// <summary>
    /// Включить блокировку IO.
    /// </summary>
    MDBX_OPT_OP_IOBLK = 19,

    /// <summary>
    /// Размер кэш-линии.
    /// </summary>
    MDBX_OPT_OP_CACHELINES = 20,

    /// <summary>
    /// Размер кэша в мегабайтах.
    /// </summary>
    MDBX_OPT_OP_CACHESIZE = 21,

    /// <summary>
    /// Размер кэша в байтах.
    /// </summary>
    MDBX_OPT_OP_CACHEBYTES = 22,

    /// <summary>
    /// Политика управления памятью.
    /// </summary>
    MDBX_OPT_OP_MEMPOLICY = 23,

    /// <summary>
    /// Политика потоков.
    /// </summary>
    MDBX_OPT_OP_THREADS = 24,

    /// <summary>
    /// Политика GC.
    /// </summary>
    MDBX_OPT_OP_GC = 25,

    /// <summary>
    /// Размер WAL.
    /// </summary>
    MDBX_OPT_OP_WALSIZE = 26,

    /// <summary>
    /// Политика WAL.
    /// </summary>
    MDBX_OPT_OP_WALPOLICY = 27,

    /// <summary>
    /// Количество элементов WAL.
    /// </summary>
    MDBX_OPT_OP_WALITEMS = 28,

    /// <summary>
    /// Количество страниц WAL.
    /// </summary>
    MDBX_OPT_OP_WALPAGES = 29,

    /// <summary>
    /// Политика роста WAL.
    /// </summary>
    MDBX_OPT_OP_WALGROW = 30,

    /// <summary>
    /// Политика сброса WAL.
    /// </summary>
    MDBX_OPT_OP_WALFLUSH = 31,

    /// <summary>
    /// Размер буфера WAL.
    /// </summary>
    MDBX_OPT_OP_WALBUF = 32,

    /// <summary>
    /// Политика буферизации WAL.
    /// </summary>
    MDBX_OPT_OP_WALBUFPOLICY = 33,

    /// <summary>
    /// Количество буферов WAL.
    /// </summary>
    MDBX_OPT_OP_WALBUFCOUNT = 34,

    /// <summary>
    /// Режим WAL.
    /// </summary>
    MDBX_OPT_OP_WALMODE = 35,

    /// <summary>
    /// Политика повторного использования WAL.
    /// </summary>
    MDBX_OPT_OP_WALREUSE = 36,

    /// <summary>
    /// Ограничение размера WAL.
    /// </summary>
    MDBX_OPT_OP_WALSIZE_LIMIT = 37,

    /// <summary>
    /// Сжатие WAL.
    /// </summary>
    MDBX_OPT_OP_WALCOMPRESS = 38,

    /// <summary>
    /// Размер страницы WAL.
    /// </summary>
    MDBX_OPT_OP_WALPAGESIZE = 39,

    /// <summary>
    /// Количество слотов WAL.
    /// </summary>
    MDBX_OPT_OP_WALSLOTS = 40,

    /// <summary>
    /// Политика освобождения WAL.
    /// </summary>
    MDBX_OPT_OP_WALFREE = 41,

    /// <summary>
    /// Политика фиксации WAL.
    /// </summary>
    MDBX_OPT_OP_WALCOMMIT = 42,

    /// <summary>
    /// Политика синхронизации WAL.
    /// </summary>
    MDBX_OPT_OP_WALSYNC = 43,

    /// <summary>
    /// Политика сброса WAL.
    /// </summary>
    MDBX_OPT_OP_WALRESET = 44,

    /// <summary>
    /// Политика переключения WAL.
    /// </summary>
    MDBX_OPT_OP_WALSWITCH = 45,

    /// <summary>
    /// Политика ротации WAL.
    /// </summary>
    MDBX_OPT_OP_WALROTATE = 46,

    /// <summary>
    /// Режим отладки.
    /// </summary>
    MDBX_OPT_OP_DEBUG = 75,

    /// <summary>
    /// Режим тестирования.
    /// </summary>
    MDBX_OPT_OP_TEST = 76,

    /// <summary>
    /// Количество параметров.
    /// </summary>
    MDBX_OPT_NUM = 77
}
