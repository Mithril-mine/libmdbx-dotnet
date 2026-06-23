namespace MDBX;

/// <summary>
/// Коды ошибок libmdbx.
/// </summary>
public static class MdbxErrorCodes
{
    /// <summary>
    /// Элемент не найден.
    /// </summary>
    public const int MDBX_NOTFOUND = -30798;

    /// <summary>
    /// Элемент уже существует.
    /// </summary>
    public const int MDBX_KEYEXIST = -30799;
}
