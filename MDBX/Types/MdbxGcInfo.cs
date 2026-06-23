using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Информация о сборке мусора (GC) и использовании страниц.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MdbxGcInfo
{
    /// <summary>
    /// Общее количество страниц в базе данных (верхний предел, определенный геометрией).
    /// </summary>
    public nuint PagesTotal;

    /// <summary>
    /// Количество страниц, в настоящее время отображенных файлом базы данных.
    /// </summary>
    public nuint PagesBacked;

    /// <summary>
    /// Количество страниц, в настоящее время выделенных.
    /// </summary>
    public nuint PagesAllocated;

    /// <summary>
    /// Количество всех страниц внутри GC, включая страницы B-дерева структуры самого GC.
    /// </summary>
    public nuint PagesGc;

    /// <summary>
    /// Информация о возвращаемых (reclaimable) страницах.
    /// </summary>
    public MdbxGcReclaimable GcReclaimable;

    /// <summary>
    /// Максимальное отставание читателя.
    /// </summary>
    public nuint MaxReaderLag;

    /// <summary>
    /// Максимальное количество удерживаемых страниц.
    /// </summary>
    public nuint MaxRetainedPages;

    /// <summary>
    /// Информация о возвращаемых страницах.
    /// </summary>
    public unsafe struct MdbxGcReclaimable
    {
        /// <summary>
        /// Количество возвращаемых страниц, включая отложенные резервы в текущей write-транзакции.
        /// </summary>
        public nuint Pages;

        /// <summary>
        /// Гистограмма длиныSpanов последовательных возвращаемых страниц.
        /// </summary>
        public MdbxChkHistogramData SpanHistogram;

        /// <summary>
        /// Распределение возвращаемых страниц по файлу базы данных.
        /// </summary>
        public MdbxChkHistogramData PgnoDistribution;
    }
}
