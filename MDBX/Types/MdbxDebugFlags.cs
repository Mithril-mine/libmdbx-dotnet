namespace MDBX.Native.Types;

/// <summary>
/// Флаги отладки.
/// </summary>
[Flags]
public enum MdbxDebugFlags : uint
{
    /// <summary>
    /// Без флагов отладки.
    /// </summary>
    MDBX_DBG_NONE = 0x00000000,

    /// <summary>
    /// Включить дорогостоящую проверку отладочных утверждений.
    /// </summary>
    MDBX_DBG_ASSERT = 0x00000001,

    /// <summary>
    /// Включить дополнительные дорогостоящие проверки и глубокую верификацию списков страниц.
    /// </summary>
    MDBX_DBG_AUDIT = 0x00000002,

    /// <summary>
    /// Включить небольшие случайные задержки в критических точках.
    /// </summary>
    MDBX_DBG_JITTER = 0x00000004,

    /// <summary>
    /// Включить включение мета-страниц БД в файлы core dump.
    /// </summary>
    MDBX_DBG_DUMP = 0x00000008,

    /// <summary>
    /// Разрешить многократное открытие окружения.
    /// </summary>
    MDBX_DBG_LEGACY_MULTIOPEN = 0x00000010,

    /// <summary>
    /// Разрешить перекрытие read и write транзакций для одного потока.
    /// </summary>
    MDBX_DBG_LEGACY_OVERLAP = 0x00000020,

    /// <summary>
    /// Отключить автоматическое обновление формата БД.
    /// </summary>
    MDBX_DBG_DONT_UPGRADE = 0x00000040
}
