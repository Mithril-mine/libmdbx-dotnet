namespace MDBX.Native.Types;

/// <summary>
/// Флаги транзакции.
/// </summary>
[Flags]
public enum MdbxTxnFlags : uint
{
    /// <summary>
    /// Флаги по умолчанию (read-write транзакция).
    /// </summary>
    MDBX_TXN_DEFAULTS = 0,

    /// <summary>
    /// Только для чтения.
    /// </summary>
    MDBX_TXN_RDONLY = 0x00020000,

    /// <summary>
    /// Подготовить read-only транзакцию без запуска.
    /// </summary>
    MDBX_TXN_RDONLY_PREPARE = 0x01020000,

    /// <summary>
    /// Не блокировать при запуске write-транзакции.
    /// </summary>
    MDBX_TXN_TRY = 0x10000000,

    /// <summary>
    /// Использовать режим окружения для durability.
    /// </summary>
    MDBX_TXN_NOWEAKING = 0,

    /// <summary>
    /// Отключить метасинхронизацию для этой транзакции.
    /// </summary>
    MDBX_TXN_NOMETASYNC = 0x00040000,

    /// <summary>
    /// Безопасный режим без синхронизации для этой транзакции.
    /// </summary>
    MDBX_TXN_NOSYNC = 0x00010000
}
