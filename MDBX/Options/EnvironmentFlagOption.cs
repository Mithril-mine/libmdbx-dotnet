namespace MDBX.Options;

/// <summary>
/// Опции для задания флагов.
/// </summary>
public enum EnvironmentFlagOption
{
    /// <summary>
    /// Добавить флаг, если он не существует
    /// </summary>
    Add,

    /// <summary>
    /// Снять флаг, если он существует
    /// </summary>
    Clear,
}
