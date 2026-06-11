using MDBX.Options;
using System.Globalization;

namespace MDBX;

/// <summary>
/// Предоставляет поиск по части строкового представления ключа или значения через курсор MDBX.
/// </summary>
/// <typeparam name="K">Тип ключа.</typeparam>
/// <typeparam name="V">Тип значения.</typeparam>
public class MdbxCursorSearch<K, V>
{
    private readonly MdbxCursor _cursor;

    /// <summary>
    /// Создает экземпляр поиска для существующего курсора.
    /// </summary>
    /// <param name="cursor">Курсор, по которому выполняется поиск.</param>
    public MdbxCursorSearch(MdbxCursor cursor)
    {
        _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
    }

    /// <summary>
    /// Ищет записи, у которых строковое представление ключа содержит заданную часть.
    /// </summary>
    /// <typeparam name="TSearch">Тип искомой части.</typeparam>
    /// <param name="key">Искомая часть ключа.</param>
    /// <returns>Массив найденных пар ключ/значение.</returns>
    public (K Key, V Value)[] FindByKey<TSearch>(TSearch key)
    {
        return Find((currentKey, _) => ContainsSearchText(currentKey, key));
    }

    /// <summary>
    /// Ищет записи, у которых строковое представление значения содержит заданную часть.
    /// </summary>
    /// <typeparam name="TSearch">Тип искомой части.</typeparam>
    /// <param name="value">Искомая часть значения.</param>
    /// <returns>Массив найденных пар ключ/значение.</returns>
    public (K Key, V Value)[] FindByValue<TSearch>(TSearch value)
    {
        return Find((_, currentValue) => ContainsSearchText(currentValue, value));
    }

    private (K Key, V Value)[] Find(Func<K, V, bool> predicate)
    {
        List<(K Key, V Value)> result = [];
        K key = default!;
        V value = default!;

        if (!_cursor.Get(ref key, ref value, CursorOption.First))
            return [];

        do
        {
            if (predicate(key, value))
                result.Add((key, value));
        }
        while (_cursor.Get(ref key, ref value, CursorOption.Next));

        return result.ToArray();
    }

    private static bool ContainsSearchText(object? source, object? searchText)
    {
        string sourceText = Convert.ToString(source, CultureInfo.InvariantCulture) ?? string.Empty;
        string searchTextValue = Convert.ToString(searchText, CultureInfo.InvariantCulture) ?? string.Empty;

        if (searchTextValue.Length == 0)
            return sourceText.Length == 0;

        return sourceText.Contains(searchTextValue, StringComparison.OrdinalIgnoreCase);
    }
}
