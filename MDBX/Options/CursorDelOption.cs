namespace MDBX.Options;

/// <summary>
/// Опции для операции удаления курсора.
/// </summary>
[Flags]
public enum CursorDelOption : int
{
    /// <summary>
    /// Нет специальных опций.
    /// </summary>
    None = 0,

    /// <summary>
    /// Нет специальных опций.
    /// </summary>
    Unspecific = None,

    /// <summary>
    /// Ввести новую пару ключ/данные только если она еще не появилась в
    /// базе данных. Этот флаг может быть указан только если база данных была открыта
    /// с MDBX_DUPSORT. Функция вернет MDBX_KEYEXIST, если пара
    /// ключ/данные уже присутствует в базе данных.
    /// </summary>
    NoDupData = Constant.MDBX_NODUPDATA,
}
