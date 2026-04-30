using System.Runtime.InteropServices;



namespace MDBX.Interop
{
    /// <summary>
    /// Информация о среде MDBX.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvironmentInfo
    {
        private EnvironmentInfoGeo _geo;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _mapSize;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _lastPageNo;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _recentTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _lastReaderTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _selfLastReaderTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta0TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta0TxnSign;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta1TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta1TxnSign;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta2TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta2TxnSign;

        [MarshalAs(UnmanagedType.U4)]
        private uint _maxReaders;

        [MarshalAs(UnmanagedType.U4)]
        private uint _numOfReaders;

        [MarshalAs(UnmanagedType.U4)]
        private uint _dxbPageSize;

        [MarshalAs(UnmanagedType.U4)]
        private uint _sysPageSize;

        /// <summary>
        /// Информация о геометрии окружения.
        /// </summary>
        public EnvironmentInfoGeo Geo { get { return _geo; } }

        /// <summary>
        /// Размер memory map в байтах.
        /// </summary>
        public ulong MapSize { get { return _mapSize; } }

        /// <summary>
        /// Номер последней страницы.
        /// </summary>
        public ulong LastPageNumber { get { return _lastPageNo; } }

        /// <summary>
        /// Идентификатор последней зафиксированной транзакции.
        /// </summary>
        public ulong RecentTransactionID { get { return _recentTxnID; } }

        /// <summary>
        /// Идентификатор транзакции последнего читателя.
        /// </summary>
        public ulong LastReaderTransactionID { get { return _lastReaderTxnID; } }

        /// <summary>
        /// Идентификатор транзакции собственного последнего читателя.
        /// </summary>
        public ulong SelfLastReaderTransactionID { get { return _selfLastReaderTxnID; } }

        /// <summary>
        /// Идентификатор транзакции мета-страницы 0.
        /// </summary>
        public ulong Meta0TransactionID { get { return _meta0TxnID; } }

        /// <summary>
        /// Признак транзакции мета-страницы 0.
        /// </summary>
        public ulong Meta0TransactionSign { get { return _meta0TxnSign; } }

        /// <summary>
        /// Идентификатор транзакции мета-страницы 1.
        /// </summary>
        public ulong Meta1TransactionID { get { return _meta1TxnID; } }

        /// <summary>
        /// Признак транзакции мета-страницы 1.
        /// </summary>
        public ulong Meta1TransactionSign { get { return _meta1TxnSign; } }

        /// <summary>
        /// Идентификатор транзакции мета-страницы 2.
        /// </summary>
        public ulong Meta2TransactionID { get { return _meta2TxnID; } }

        /// <summary>
        /// Признак транзакции мета-страницы 2.
        /// </summary>
        public ulong Meta2TransactionSign { get { return _meta2TxnSign; } }

        /// <summary>
        /// Максимальное количество читателей.
        /// </summary>
        public uint MaxReaders { get { return _maxReaders; } }

        /// <summary>
        /// Текущее количество читателей.
        /// </summary>
        public uint NumberOfReaders { get { return _numOfReaders; } }

        /// <summary>
        /// Размер страницы базы данных (dxb page size).
        /// </summary>
        public uint DatabasePageSize { get { return _dxbPageSize; } }

        /// <summary>
        /// Системный размер страницы.
        /// </summary>
        public uint SystemPageSize { get { return _sysPageSize; } }
    }

}
