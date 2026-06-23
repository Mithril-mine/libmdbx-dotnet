using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Гистограмма для проверки целостности базы данных.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxChkHistogram
{
    /// <summary>
    /// Общее количество элементов.
    /// </summary>
    public nuint Amount;

    /// <summary>
    /// Количество интервалов.
    /// </summary>
    public nuint Count;

    /// <summary>
    /// Количество элементов &lt;= 1.
    /// </summary>
    public nuint Le1Amount;

    /// <summary>
    /// Количество интервалов &lt;= 1.
    /// </summary>
    public nuint Le1Count;

    /// <summary>
    /// Диапазоны гистограммы (9 штук).
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
    public MdbxChkHistogramRange[] Ranges;

    /// <summary>
    /// Диапазон гистограммы.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MdbxChkHistogramRange
    {
        /// <summary>
        /// Начало диапазона.
        /// </summary>
        public nuint Begin;

        /// <summary>
        /// Конец диапазона.
        /// </summary>
        public nuint End;

        /// <summary>
        /// Количество элементов в диапазоне.
        /// </summary>
        public nuint Amount;

        /// <summary>
        /// Количество интервалов в диапазоне.
        /// </summary>
        public nuint Count;
    }
}
