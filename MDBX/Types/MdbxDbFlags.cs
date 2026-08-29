namespace MDBX.Native.Types;

/// <summary>
/// Флаги базы данных (таблицы).
/// </summary>
[Flags]
public enum MdbxDbFlags : uint
{
    /// <summary>
    /// Флаги по умолчанию.
    /// </summary>
    MDBX_DB_DEFAULTS = 0x00000000,

    /// <summary>
    /// Использовать обратный ключ (ключи хранятся в обратном порядке).
    /// </summary>
    MDBX_REVERSEKEY = 0x00000002,

    /// <summary>
    /// Разрешить дубликаты ключей.
    /// </summary>
    MDBX_DUPSORT = 0x00000004,

    /// <summary>
    /// Цифровые ключи (для оптимизации сравнения).
    /// </summary>
    MDBX_INTEGERKEY = 0x00000008,

    /// <summary>
    /// Цифровые значения (для оптимизации сравнения).
    /// </summary>
    MDBX_INTEGERDUP = 0x00000020,

    /// <summary>
    /// С фиксированным размером дубликаты (для оптимизации сравнения).
    /// </summary>
    MDBX_DUPFIXED = 0x00000010,

    /// <summary>
    /// Обратное сравнение строковых значений дубликатов.
    /// </summary>
    MDBX_REVERSEDUP = 0x00000040,

    /// <summary>
    /// Создать базу данных, если она не существует.
    /// </summary>
    MDBX_CREATE = 0x00040000,

    /// <summary>
    /// Открыть существующую таблицу, созданную с неизвестными флагами.
    /// </summary>
    MDBX_DB_ACCEDE = 0x40000000
}
