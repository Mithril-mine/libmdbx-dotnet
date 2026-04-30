using System.Runtime.InteropServices;


namespace MDBX.Interop;

/// <summary>
/// Statistics for a database in the environment
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct EnvStat
{
    [MarshalAs(UnmanagedType.U4)]
    private uint _pageSize;

    [MarshalAs(UnmanagedType.U4)]
    private uint _depth;

    [MarshalAs(UnmanagedType.U8)]
    private ulong _branchPages;

    [MarshalAs(UnmanagedType.U8)]
    private ulong _leafPages;

    [MarshalAs(UnmanagedType.U8)]
    private ulong _overflowPages;

    [MarshalAs(UnmanagedType.U8)]
    private ulong _entries;

    /// <summary>
    /// Размер страницы базы данных. В настоящее время одинаков для всех баз данных.
    /// </summary>
    public readonly uint PageSize { get { return _pageSize; } }

    /// <summary>
    /// Глубина (высота) B-дерева
    /// </summary>
    public readonly uint Depth { get { return _depth; } }

    /// <summary>
    /// Количество внутренних (нелистовых) страниц
    /// </summary>
    public readonly ulong BranchPages { get { return _branchPages; } }

    /// <summary>
    /// Количество листовых страниц
    /// </summary>
    public readonly ulong LeafPages { get { return _leafPages; } }

    /// <summary>
    /// Количество переполняющих страниц
    /// </summary>
    public readonly ulong OverflowPages { get { return _overflowPages; } }

    /// <summary>
    /// Количество элементов данных
    /// </summary>
    public readonly ulong Entries { get { return _entries; } }
}
