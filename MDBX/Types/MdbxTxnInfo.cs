using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Информация о транзакции MDBX.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MdbxTxnInfo
{
    /// <summary>
    /// ID транзакции. Для READ-ONLY транзакции соответствует снимку MVCC, который читается.
    /// </summary>
    public ulong TxnId;

    /// <summary>
    /// Для READ-ONLY транзакции: отставание от последнего MVCC-снимка.
    /// Для WRITE транзакции (при scan_rlt=true): отставание самого старого читателя от текущей транзакции.
    /// </summary>
    public ulong TxnReaderLag;

    /// <summary>
    /// Используемое пространство данной транзакцией.
    /// </summary>
    public ulong TxnSpaceUsed;

    /// <summary>
    /// Текущий размер файла базы данных.
    /// </summary>
    public ulong TxnSpaceLimitSoft;

    /// <summary>
    /// Верхняя граница размера файла базы данных.
    /// </summary>
    public ulong TxnSpaceLimitHard;

    /// <summary>
    /// Для READ-ONLY: общий размер страниц, удаленных зафиксированными write-транзакциями.
    /// Для WRITE: обобщенный размер удаленных страниц из-за Copy-On-Write.
    /// </summary>
    public ulong TxnSpaceRetired;

    /// <summary>
    /// Для READ-ONLY: пространство, доступное для writer(s).
    /// Для WRITE: пространство внутри транзакции до ошибки MDBX_TXN_FULL.
    /// </summary>
    public ulong TxnSpaceLeftover;

    /// <summary>
    /// Для READ-ONLY: пространство, которое станет доступно после завершения транзакции.
    /// Для WRITE: обобщенный размер грязных страниц, созданных в транзакции.
    /// </summary>
    public ulong TxnSpaceDirty;

    /// <summary>
    /// Количество операций получения страниц в транзакции.
    /// </summary>
    public ulong TxnPget;
}
