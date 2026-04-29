namespace MDBX
{
    /// <summary>
    /// Интерфейс для сериализации и десериализации объектов.
    /// </summary>
    /// <typeparam name="T">Тип объекта для сериализации.</typeparam>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Сериализует объект в массив байтов.
        /// </summary>
        /// <param name="t">Объект для сериализации.</param>
        /// <returns>Массив байтов, представляющий объект.</returns>
        byte[] Serialize(T t);

        /// <summary>
        /// Десериализует массив байтов в объект.
        /// </summary>
        /// <param name="buffer">Массив байтов для десериализации.</param>
        /// <returns>Десериализованный объект.</returns>
        T Deserialize(byte[] buffer);
    }
}
