namespace MDBX;

/// <summary>
/// Пара ключ-значение, прочитанная из таблицы через курсор.
/// Ключ и значение представлены в виде "сырых" байтов и копируются из нативной памяти.
/// </summary>
public sealed class MdbxEntry
{
    /// <summary>
    /// Ключ записи.
    /// </summary>
    public byte[] Key { get; }

    /// <summary>
    /// Значение записи.
    /// </summary>
    public byte[] Value { get; }

    /// <summary>
    /// Создает запись из ключа и значения.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    public MdbxEntry(byte[] key, byte[] value)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
