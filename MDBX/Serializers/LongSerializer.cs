namespace MDBX.Serializers;

/// <summary>
/// Сериализатор для длинных целых чисел (long).
/// </summary>
public class LongSerializer : ISerializer<long>
{
    /// <summary>
    /// Десериализует массив байтов в длинное целое число.
    /// </summary>
    /// <param name="buffer">Массив байтов для десериализации.</param>
    /// <returns>Десериализованное длинное число или 0, если buffer пуст.</returns>
    public long Deserialize(byte[] buffer) => buffer == null || buffer.Length == 0 ? 0 : BitConverter.ToInt64(buffer, 0);

    /// <summary>
    /// Сериализует длинное целое число в массив байтов.
    /// </summary>
    /// <param name="num">Длинное число для сериализации.</param>
    /// <returns>Массив байтов, представляющий число.</returns>
    public byte[] Serialize(long num) => BitConverter.GetBytes(num);
}
