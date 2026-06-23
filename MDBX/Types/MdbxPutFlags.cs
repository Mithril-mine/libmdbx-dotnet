namespace MDBX.Native.Types;

/// <summary>
/// Флаги операции вставки/обновления.
/// </summary>
[Flags]
public enum MdbxPutFlags : uint
{
    /// <summary>
    /// Вставить или обновить (UPSERT) — поведение по умолчанию.
    /// </summary>
    MDBX_UPSERT = 0,

    /// <summary>
    /// Вставить только если ключа нет.
    /// </summary>
    MDBX_NOOVERWRITE = 0x10,

    /// <summary>
    /// Не записывать, если пара ключ-значение уже существует.
    /// </summary>
    MDBX_NODUPDATA = 0x20,

    /// <summary>
    /// Текущее значение (обновить существующий ключ).
    /// </summary>
    MDBX_CURRENT = 0x40,

    /// <summary>
    /// Все дубликаты (заменить все значения для ключа).
    /// </summary>
    MDBX_ALLDUPS = 0x80,

    /// <summary>
    /// Только зарезервировать место для данных, не копировать их.
    /// </summary>
    MDBX_RESERVE = 0x10000,

    /// <summary>
    /// Добавить элемент в начало (для неуникальных ключей).
    /// </summary>
    MDBX_APPEND = 0x20000,

    /// <summary>
    /// Добавить дубликат в начало.
    /// </summary>
    MDBX_APPENDDUP = 0x40000,

    /// <summary>
    /// Сохранить существующее значение при неудачной вставке.
    /// </summary>
    MDBX_MULTIPLE = 0x80000
}
