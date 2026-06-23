namespace MDBX.Native.Types;

/// <summary>
/// Операции курсора.
/// </summary>
public enum MdbxCursorOp : int
{
    /// <summary>
    /// Получить первый элемент.
    /// </summary>
    MDBX_FIRST = 0,

    /// <summary>
    /// Получить первый дубликат.
    /// </summary>
    MDBX_FIRST_DUP = 1,

    /// <summary>
    /// Получить пару ключ/данные.
    /// </summary>
    MDBX_GET_BOTH = 2,

    /// <summary>
    /// Получить ключ и первые данные, большие или равные указанным.
    /// </summary>
    MDBX_GET_BOTH_RANGE = 3,

    /// <summary>
    /// Получить текущий элемент.
    /// </summary>
    MDBX_GET_CURRENT = 4,

    /// <summary>
    /// Получить несколько дубликатов.
    /// </summary>
    MDBX_GET_MULTIPLE = 5,

    /// <summary>
    /// Получить последний элемент.
    /// </summary>
    MDBX_LAST = 6,

    /// <summary>
    /// Получить последний дубликат.
    /// </summary>
    MDBX_LAST_DUP = 7,

    /// <summary>
    /// Получить следующий элемент.
    /// </summary>
    MDBX_NEXT = 8,

    /// <summary>
    /// Получить следующий дубликат.
    /// </summary>
    MDBX_NEXT_DUP = 9,

    /// <summary>
    /// Получить несколько следующих дубликатов.
    /// </summary>
    MDBX_NEXT_MULTIPLE = 10,

    /// <summary>
    /// Получить первый элемент следующего ключа.
    /// </summary>
    MDBX_NEXT_NODUP = 11,

    /// <summary>
    /// Получить предыдущий элемент.
    /// </summary>
    MDBX_PREV = 12,

    /// <summary>
    /// Получить предыдущий дубликат.
    /// </summary>
    MDBX_PREV_DUP = 13,

    /// <summary>
    /// Получить последний элемент предыдущего ключа.
    /// </summary>
    MDBX_PREV_NODUP = 14,

    /// <summary>
    /// Установить ключ.
    /// </summary>
    MDBX_SET = 15,

    /// <summary>
    /// Установить ключ и получить данные.
    /// </summary>
    MDBX_SET_KEY = 16
}
