namespace MDBX.Native.Types;

/// <summary>
/// Действие для пакетного удаления курсором.
/// </summary>
public enum MdbxBunchAction : int
{
    /// <summary>
    /// Удалить текущее значение.
    /// </summary>
    MDBX_DELETE_CURRENT_VALUE = 0,

    /// <summary>
    /// Удалить все мультизначения до текущего (исключая текущее).
    /// </summary>
    MDBX_DELETE_CURRENT_MULTIVAL_BEFORE_EXCLUDING = 1,

    /// <summary>
    /// Удалить все мультизначения до текущего (включая текущее).
    /// </summary>
    MDBX_DELETE_CURRENT_MULTIVAL_BEFORE_INCLUDING = 2,

    /// <summary>
    /// Удалить все мультизначения после текущего (включая текущее).
    /// </summary>
    MDBX_DELETE_CURRENT_MULTIVAL_AFTER_INCLUDING = 3,

    /// <summary>
    /// Удалить все мультизначения после текущего (исключая текущее).
    /// </summary>
    MDBX_DELETE_CURRENT_MULTIVAL_AFTER_EXCLUDING = 4,

    /// <summary>
    /// Удалить все мультизначения для текущего ключа.
    /// </summary>
    MDBX_DELETE_CURRENT_MULTIVAL_ALL = 5,

    /// <summary>
    /// Удалить все до текущего (исключая текущее).
    /// </summary>
    MDBX_DELETE_BEFORE_EXCLUDING = 6,

    /// <summary>
    /// Удалить все до текущего (включая текущее).
    /// </summary>
    MDBX_DELETE_BEFORE_INCLUDING = 7,

    /// <summary>
    /// Удалить все после текущего (включая текущее).
    /// </summary>
    MDBX_DELETE_AFTER_INCLUDING = 8,

    /// <summary>
    /// Удалить все после текущего (исключая текущее).
    /// </summary>
    MDBX_DELETE_AFTER_EXCLUDING = 9,

    /// <summary>
    /// Удалить все.
    /// </summary>
    MDBX_DELETE_WHOLE = 10
}
