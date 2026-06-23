namespace MDBX.Native.Types;

/// <summary>
/// Уровни логирования.
/// </summary>
public enum MdbxLogLevel : int
{
    /// <summary>
    /// Не изменять текущие настройки (только для mdbx_setup_debug).
    /// </summary>
    MDBX_LOG_DONTCHANGE = -1,

    /// <summary>
    /// Критические условия (аварийные отказы).
    /// </summary>
    MDBX_LOG_FATAL = 0,

    /// <summary>
    /// Ошибки.
    /// </summary>
    MDBX_LOG_ERROR = 1,

    /// <summary>
    /// Предупреждения.
    /// </summary>
    MDBX_LOG_WARN = 2,

    /// <summary>
    /// Уведомления.
    /// </summary>
    MDBX_LOG_NOTICE = 3,

    /// <summary>
    /// Подробные информационные сообщения.
    /// </summary>
    MDBX_LOG_VERBOSE = 4,

    /// <summary>
    /// Отладочные сообщения.
    /// </summary>
    MDBX_LOG_DEBUG = 5,

    /// <summary>
    /// Трассировочные отладочные сообщения.
    /// </summary>
    MDBX_LOG_TRACE = 6,

    /// <summary>
    /// Дополнительные отладочные сообщения (дамп списков pgno).
    /// </summary>
    MDBX_LOG_EXTRA = 7,

    /// <summary>
    /// Максимальный уровень логирования.
    /// </summary>
    MDBX_LOG_MAX = 7
}
