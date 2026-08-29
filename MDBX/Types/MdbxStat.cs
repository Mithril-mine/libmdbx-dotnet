using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Статистика окружения MDBX.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MdbxStat
{
    /// <summary>
    /// Размер страницы базы данных в байтах.
    /// </summary>
    public nuint MsPsize;

    /// <summary>
    /// Глубина дерева (глубина B+ дерева).
    /// </summary>
    public nuint MsDepth;

    /// <summary>
    /// Количество страниц ветвей (внутренние страницы B+ дерева).
    /// </summary>
    public nuint MsBranchPages;

    /// <summary>
    /// Количество листовых страниц.
    /// </summary>
    public nuint MsLeafPages;

    /// <summary>
    /// Количество больших/переполняющих страниц.
    /// </summary>
    public nuint MsOverflowPages;

    /// <summary>
    /// Количество элементов данных.
    /// </summary>
    public nuint MsEntries;
}
