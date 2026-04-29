namespace MDBX
{
    /// <summary>
    /// Сериализатор для целых чисел (int).
    /// </summary>
    public class IntSerializer : ISerializer<int>
    {
        /// <summary>
        /// Десериализует массив байтов в целое число.
        /// </summary>
        /// <param name="buffer">Массив байтов для десериализации.</param>
        /// <returns>Десериализованное целое число или 0, если buffer пуст.</returns>
        public int Deserialize(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return 0;

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Сериализует целое число в массив байтов.
        /// </summary>
        /// <param name="num">Целое число для сериализации.</param>
        /// <returns>Массив байтов, представляющий число.</returns>
        public byte[] Serialize(int num)
        {
            return BitConverter.GetBytes(num);
        }
    }
}
