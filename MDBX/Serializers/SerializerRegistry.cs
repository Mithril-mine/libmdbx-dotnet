namespace MDBX.Serializers;

/// <summary>
/// Реестр для регистрации и получения сериализаторов типов данных.
/// </summary>
public static class SerializerRegistry
{
    private static readonly Dictionary<Type, object> _dic = new Dictionary<Type, object>()
    {
        { typeof(string), new StringSerializer() },
        { typeof(int), new IntSerializer() },
        { typeof(long), new LongSerializer() },
        { typeof(byte[]), new ByteArraySerializer() },
    };

    /// <summary>
    /// Регистрирует сериализатор для указанного типа T.
    /// </summary>
    /// <typeparam name="T">Тип, для которого регистрируется сериализатор.</typeparam>
    /// <param name="serializer">Экземпляр сериализатора.</param>
    public static void Register<T>(ISerializer<T> serializer)
    {
        _dic[typeof(T)] = serializer;
    }

    /// <summary>
    /// Получает сериализатор для указанного типа T.
    /// </summary>
    /// <typeparam name="T">Тип, для которого нужен сериализатор.</typeparam>
    /// <returns>Экземпляр сериализатора.</returns>
    /// <exception cref="KeyNotFoundException">Если сериализатор для типа не найден.</exception>
    internal static ISerializer<T> Get<T>()
    {

        _dic.TryGetValue(typeof(T), out object? obj);

        if (obj is not ISerializer<T> serializer)
        {
            throw new KeyNotFoundException($"Unable to find serializer of {typeof(T).Name}, please use `SerializerRegistry.Register` to register.");
        }

        return serializer;
    }
}
