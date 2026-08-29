using System.Collections.Generic;

namespace MDBX;

/// <summary>
/// Методы расширения для <see cref="MdbxQuery"/>, упрощающие проекцию записей
/// и построение LINQ-запросов, где параметр предиката — это значение или ключ.
/// </summary>
public static class MdbxQueryExtensions
{
    /// <summary>
    /// Проецирует записи в "сырые" значения (без ключей).
    /// </summary>
    /// <param name="query">Запрос на основе курсора.</param>
    /// <returns>Последовательность значений.</returns>
    public static IEnumerable<byte[]> Values(this MdbxQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        foreach (var entry in query)
            yield return entry.Value;
    }

    /// <summary>
    /// Проецирует значения записей в заданный тип.
    /// Позволяет писать запросы вида:
    /// <c>query.Values(Encoding.UTF8.GetString).Where(x => x == "Some Value")</c>,
    /// где <c>x</c> — это уже преобразованное значение.
    /// </summary>
    /// <typeparam name="TValue">Целевой тип значения.</typeparam>
    /// <param name="query">Запрос на основе курсора.</param>
    /// <param name="selector">Преобразование "сырых" байтов значения в TValue.</param>
    /// <returns>Последовательность преобразованных значений.</returns>
    public static IEnumerable<TValue> Values<TValue>(this MdbxQuery query, Func<byte[], TValue> selector)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        foreach (var entry in query)
            yield return selector(entry.Value);
    }

    /// <summary>
    /// Проецирует записи в "сырые" ключи.
    /// </summary>
    /// <param name="query">Запрос на основе курсора.</param>
    /// <returns>Последовательность ключей.</returns>
    public static IEnumerable<byte[]> Keys(this MdbxQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        foreach (var entry in query)
            yield return entry.Key;
    }

    /// <summary>
    /// Проецирует ключи записей в заданный тип.
    /// </summary>
    /// <typeparam name="TKey">Целевой тип ключа.</typeparam>
    /// <param name="query">Запрос на основе курсора.</param>
    /// <param name="selector">Преобразование "сырых" байтов ключа в TKey.</param>
    /// <returns>Последовательность преобразованных ключей.</returns>
    public static IEnumerable<TKey> Keys<TKey>(this MdbxQuery query, Func<byte[], TKey> selector)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        foreach (var entry in query)
            yield return selector(entry.Key);
    }

    /// <summary>
    /// Проецирует записи в пары ключ-значение заданных типов.
    /// </summary>
    /// <typeparam name="TKey">Целевой тип ключа.</typeparam>
    /// <typeparam name="TValue">Целевой тип значения.</typeparam>
    /// <param name="query">Запрос на основе курсора.</param>
    /// <param name="keySelector">Преобразование "сырых" байтов ключа в TKey.</param>
    /// <param name="valueSelector">Преобразование "сырых" байтов значения в TValue.</param>
    /// <returns>Последовательность пар ключ-значение.</returns>
    public static IEnumerable<KeyValuePair<TKey, TValue>> Entries<TKey, TValue>(
        this MdbxQuery query,
        Func<byte[], TKey> keySelector,
        Func<byte[], TValue> valueSelector)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
        foreach (var entry in query)
            yield return new KeyValuePair<TKey, TValue>(keySelector(entry.Key), valueSelector(entry.Value));
    }
}
