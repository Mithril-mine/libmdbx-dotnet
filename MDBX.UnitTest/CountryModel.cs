namespace MDBX.UnitTest;

/// <summary>
/// Модель страны для тестирования.
/// </summary>
public class CountryModel
{
    /// <summary>
    /// Название страны.
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// Код страны (например, ISO 3166-1 alpha-2 или alpha-3).
    /// </summary>
    public string code { get; set; }
}
