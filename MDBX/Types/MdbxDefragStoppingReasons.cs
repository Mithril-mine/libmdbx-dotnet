namespace MDBX.Native.Types;

/// <summary>
/// Причины остановки дефрагментации базы данных.
/// </summary>
[Flags]
public enum MdbxDefragStoppingReasons : uint
{
    /// <summary>Нет препятствий для дефрагментации.</summary>
    MDBX_DEFRAG_NOOBSTACLES = 0,
    /// <summary>Достигнут лимит размера транзакции шага.</summary>
    MDBX_DEFRAG_STEP_SIZE = 1,
    /// <summary>Необходимо предварительное перемещение для формирования последовательности свободных страниц.</summary>
    MDBX_DEFRAG_LARGE_CHUNK = 2,
    /// <summary>Дефрагментация была прервана пользователем.</summary>
    MDBX_DEFRAG_DISCONTINUED = 4,
    /// <summary>Читатели используют старые MVCC-снимки, блокирующие дефрагментацию.</summary>
    MDBX_DEFRAG_LAGGARD_READER = 8,
    /// <summary>Достигнута цель дефрагментации, заданная пользователем.</summary>
    MDBX_DEFRAG_ENOUGH_THRESHOLD = 16,
    /// <summary>Истек лимит времени дефрагментации.</summary>
    MDBX_DEFRAG_TIME_LIMIT = 32,
    /// <summary>Дефрагментация была прервана пользователем с немедленным завершением.</summary>
    MDBX_DEFRAG_ABORTED = 64,
    /// <summary>Произошла ошибка во время дефрагментации.</summary>
    MDBX_DEFRAG_ERROR = 128
}
