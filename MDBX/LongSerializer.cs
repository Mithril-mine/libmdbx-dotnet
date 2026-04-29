namespace MDBX
{
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
        public long Deserialize(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return 0;

            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Сериализует длинное целое число в массив байтов.
        /// </summary>
        /// <param name="num">Длинное число для сериализации.</param>
        /// <returns>Массив байтов, представляющий число.</returns>
        public byte[] Serialize(long num)
        {
            return BitConverter.GetBytes(num);
        }
    }
}
