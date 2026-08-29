namespace MDBX.Native.Types;

/// <summary>
/// Флаги окружения.
/// </summary>
[Flags]
public enum MdbxEnvFlags : uint
{
    /// <summary>
    /// Флаги по умолчанию.
    /// </summary>
    MDBX_ENV_DEFAULTS = 0x00000000,

    /// <summary>
    /// Дополнительная валидация структуры БД и содержимого страниц.
    /// </summary>
    MDBX_VALIDATION = 0x00002000,

    /// <summary>
    /// Отсутствие каталога окружения.
    /// </summary>
    MDBX_NOSUBDIR = 0x00004000,

    /// <summary>
    /// Режим только для чтения.
    /// </summary>
    MDBX_RDONLY = 0x00020000,

    /// <summary>
    /// Исключительный/монопольный режим открытия окружения.
    /// </summary>
    MDBX_EXCLUSIVE = 0x00400000,

    /// <summary>
    /// Использовать окружение/БД, уже открытые другим процессом(ами).
    /// </summary>
    MDBX_ACCEDE = 0x40000000,

    /// <summary>
    /// Отображать данные в память с правами записи.
    /// </summary>
    MDBX_WRITEMAP = 0x00080000,

    /// <summary>
    /// Отвязать транзакции от потоков, насколько это возможно.
    /// </summary>
    MDBX_NOSTICKYTHREADS = 0x00200000,

    /// <summary>
    /// Отключить предварительное чтение.
    /// </summary>
    MDBX_NORDAHEAD = 0x00800000,

    /// <summary>
    /// Не инициализировать malloc'ed память перед записью в файл данных.
    /// </summary>
    MDBX_NOMEMINIT = 0x01000000,

    /// <summary>
    /// Использовать LRU-стратегию возврата страниц читателями.
    /// </summary>
    MDBX_LIFORECLAIM = 0x04000000,

    /// <summary>
    /// Отладка: заполнять/возмущать освобождаемые страницы.
    /// </summary>
    MDBX_PAGEPERTURB = 0x08000000,

    /// <summary>
    /// Не сбрасывать метаданные на диск при коммите.
    /// </summary>
    MDBX_NOMETASYNC = 0x00040000,

    /// <summary>
    /// Безопасный режим без синхронизации (данные сбрасываются периодически).
    /// </summary>
    MDBX_SAFE_NOSYNC = 0x00010000,

    /// <summary>
    /// Полностью отключить синхронизацию (опасно для данных).
    /// </summary>
    MDBX_UTTERLY_NOSYNC = 0x00110000
}
