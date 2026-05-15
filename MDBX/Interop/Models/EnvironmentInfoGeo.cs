using System.Runtime.InteropServices;



namespace MDBX.Interop.Models
{
    /// <summary>
    /// Информация о геометрии (размерах) окружения базы данных MDBX.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvironmentInfoGeo
    {
        /// <summary>
        /// Lower limit for datafile size.
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        private ulong _lower;

        /// <summary>
        /// Upper limit for datafile size
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        private ulong _upper;

        /// <summary>
        /// Current datafile size.
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        private ulong _current;

        /// <summary>
        /// Shrink threshold for datafile.
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        private ulong _shrink;

        /// <summary>
        /// Growth step for datafile.
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        private ulong _grow;

        /// <summary>
        /// Нижняя граница для размера файла данных.
        /// </summary>
        public ulong LowerLimit { get { return _lower; } }

        /// <summary>
        /// Верхняя граница для размера файла данных.
        /// </summary>
        public ulong UpperLimit { get { return _upper; } }

        /// <summary>
        /// Текущий размер файла данных.
        /// </summary>
        public ulong CurrentSize { get { return _current; } }

        /// <summary>
        /// Порог сжатия для файла данных.
        /// </summary>
        public ulong ShrinkThreshold { get { return _shrink; } }

        /// <summary>
        /// Шаг увеличения размера файла данных.
        /// </summary>
        public ulong GrowStep { get { return _grow; } }
    }

}
