using System.Runtime.InteropServices;


namespace MDBX.Interop;

/// <summary>
/// Statistics for a database in the environment
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct EnvironmentStat
{
    /// <summary>Размер страницы таблицы в байтах. Данное значение одинаково для всех таблиц в рамках одной базы данных.</summary>
    public uint MsPsize;

    /// <summary>Глубина (высота) дерева B-tree.</summary>
    public uint MsDepth;

    /// <summary>Количество внутренних (ветвевых, нелистовых) страниц дерева.</summary>
    public ulong MsBranchPages;

    /// <summary>Количество листовых (leaf) страниц дерева.</summary>
    public ulong MsLeafPages;

    /// <summary>Количество больших страниц или страниц переполнения (overflow pages).</summary>
    public ulong MsOverflowPages;

    /// <summary>Общее количество элементов данных (записей), хранящихся в таблице.</summary>
    public ulong MsEntries;

    /// <summary>Идентификатор (ID) транзакции, которой было зафиксировано последнее изменение в этой таблице.</summary>
    public ulong MsModTxnid;
}
