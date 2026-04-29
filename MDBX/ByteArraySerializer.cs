namespace MDBX
{
    /// <summary>
    /// Сериализатор для массивов байтов (идентичное преобразование).
    /// </summary>
    public class ByteArraySerializer : ISerializer<byte[]>
    {
        /// <summary>
        /// Возвращает массив байтов как есть.
        /// </summary>
        /// <param name="buffer">Исходный массив байтов.</param>
        /// <returns>Тот же массив байтов.</returns>
        public byte[] Deserialize(byte[] buffer)
        {
            return buffer;
        }

        /// <summary>
        /// Возвращает массив байтов как есть.
        /// </summary>
        /// <param name="buffer">Исходный массив байтов.</param>
        /// <returns>Тот же массив байтов.</returns>
        public byte[] Serialize(byte[] buffer)
        {
            return buffer;
        }
    }
}
