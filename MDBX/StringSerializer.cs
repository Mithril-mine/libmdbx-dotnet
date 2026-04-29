using System.Text;

namespace MDBX
{
    /// <summary>
    /// Сериализатор для строк UTF-8.
    /// </summary>
    public class StringSerializer : ISerializer<string>
    {
        /// <summary>
        /// Десериализует массив байтов в строку.
        /// </summary>
        /// <param name="buffer">Массив байтов для десериализации.</param>
        /// <returns>Десериализованная строка или пустая строка, если buffer равен null.</returns>
        public string Deserialize(byte[]? buffer)
        {
            if (buffer == null)
                return string.Empty;

            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Сериализует строку в массив байтов.
        /// </summary>
        /// <param name="text">Строка для сериализации.</param>
        /// <returns>Массив байтов, представляющий строку.</returns>
        public byte[] Serialize(string text)
        {
            if (text == null)
                return new byte[0];

            return Encoding.UTF8.GetBytes(text);
        }
    }
}
