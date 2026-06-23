using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Информация об окружении MDBX.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MDBX_envinfo
{
    /// <summary>
    /// Геометрия файлов.
    /// </summary>
    public MDBX_geo MiGeo;

    /// <summary>
    /// Размер memory map.
    /// </summary>
    public ulong MiMapsize;

    /// <summary>
    /// Текущий размер файла БД.
    /// </summary>
    public ulong MiDxbFsize;

    /// <summary>
    /// Выделенный размер файла БД.
    /// </summary>
    public ulong MiDxbFallocated;

    /// <summary>
    /// Номер последней использованной страницы.
    /// </summary>
    public ulong MiLastPgno;

    /// <summary>
    /// ID последней зафиксированной транзакции.
    /// </summary>
    public ulong MiRecentTxnid;

    /// <summary>
    /// ID последней reader транзакции.
    /// </summary>
    public ulong MiLatterReaderTxnid;

    /// <summary>
    /// ID последней reader транзакции текущего процесса.
    /// </summary>
    public ulong MiSelfLatterReaderTxnid;

    /// <summary>
    /// Мета-информация.
    /// </summary>
    public fixed ulong MiMetaTxnid[3];

    /// <summary>
    /// Сигнатуры мета-информации.
    /// </summary>
    public fixed ulong MiMetaSign[3];

    /// <summary>
    /// Максимальное количество reader слотов.
    /// </summary>
    public uint MiMaxreaders;

    /// <summary>
    /// Количество используемых reader слотов.
    /// </summary>
    public uint MiNumreaders;

    /// <summary>
    /// Размер страницы БД.
    /// </summary>
    public uint MiDxbPagesize;

    /// <summary>
    /// Системный размер страницы.
    /// </summary>
    public uint MiSysPagesize;

    /// <summary>
    /// Размер блока Unified Page Cache.
    /// </summary>
    public uint MiSysUpcblk;

    /// <summary>
    /// Размер блока IO.
    /// </summary>
    public uint MiSysIoblk;

    /// <summary>
    /// Идентификатор загрузки.
    /// </summary>
    public MDBX_bootid MiBootid;

    /// <summary>
    /// Несинхронизированный объем.
    /// </summary>
    public ulong MiUnsyncVolume;

    /// <summary>
    /// Порог автосинхронизации.
    /// </summary>
    public ulong MiAutosyncThreshold;

    /// <summary>
    /// Время с последней нестабильной фиксации (1/65536 сек).
    /// </summary>
    public uint MiSinceSyncSeconds16Dot16;

    /// <summary>
    /// Период автосинхронизации (1/65536 сек).
    /// </summary>
    public uint MiAutosyncPeriodSeconds16Dot16;

    /// <summary>
    /// Время с последней проверки читателей (1/65536 сек).
    /// </summary>
    public uint MiSinceReaderCheckSeconds16Dot16;

    /// <summary>
    /// Текущий режим окружения.
    /// </summary>
    public uint MiMode;

    /// <summary>
    /// Статистика операций со страницами.
    /// </summary>
    public MDBX_pgop_stat MiPgopStat;

    /// <summary>
    /// Идентификатор файла БД.
    /// </summary>
    public MDBX_dxbid MiDxbid;

    /// <summary>
    /// Геометрия файлов.
    /// </summary>
    public struct MDBX_geo
    {
        /// <summary>
        /// Нижний предел размера файла.
        /// </summary>
        public ulong Lower;

        /// <summary>
        /// Верхний предел размера файла.
        /// </summary>
        public ulong Upper;

        /// <summary>
        /// Текущий размер файла.
        /// </summary>
        public ulong Current;

        /// <summary>
        /// Порог сжатия.
        /// </summary>
        public ulong Shrink;

        /// <summary>
        /// Шаг роста.
        /// </summary>
        public ulong Grow;
    }

    /// <summary>
    /// Идентификатор загрузки.
    /// </summary>
    public struct MDBX_bootid
    {
        /// <summary>
        /// Текущий идентификатор.
        /// </summary>
        public MDBX_bootid_pair Current;

        /// <summary>
        /// Мета идентификаторы.
        /// </summary>
        public fixed ulong Meta[3 * 2];
    }

    /// <summary>
    /// Пара идентификаторов.
    /// </summary>
    public struct MDBX_bootid_pair
    {
        /// <summary>
        /// Первый идентификатор.
        /// </summary>
        public ulong X;

        /// <summary>
        /// Второй идентификатор.
        /// </summary>
        public ulong Y;
    }

    /// <summary>
    /// Статистика операций со страницами.
    /// </summary>
    public struct MDBX_pgop_stat
    {
        /// <summary>
        /// Количество новых страниц.
        /// </summary>
        public ulong Newly;

        /// <summary>
        /// Количество скопированных страниц.
        /// </summary>
        public ulong Cow;

        /// <summary>
        /// Количество клонированных страниц.
        /// </summary>
        public ulong Clone;

        /// <summary>
        /// Количество разделений страниц.
        /// </summary>
        public ulong Split;

        /// <summary>
        /// Количество слияний страниц.
        /// </summary>
        public ulong Merge;

        /// <summary>
        /// Количество spill операций.
        /// </summary>
        public ulong Spill;

        /// <summary>
        /// Количество unspill операций.
        /// </summary>
        public ulong Unspill;

        /// <summary>
        /// Количество операций записи.
        /// </summary>
        public ulong Wops;

        /// <summary>
        /// Количество prefault операций.
        /// </summary>
        public ulong Prefault;

        /// <summary>
        /// Количество вызовов mincore.
        /// </summary>
        public ulong Mincore;

        /// <summary>
        /// Количество операций msync.
        /// </summary>
        public ulong Msync;

        /// <summary>
        /// Количество операций fsync.
        /// </summary>
        public ulong Fsync;
    }

    /// <summary>
    /// Идентификатор файла БД.
    /// </summary>
    public struct MDBX_dxbid
    {
        /// <summary>
        /// Первый идентификатор.
        /// </summary>
        public ulong X;

        /// <summary>
        /// Второй идентификатор.
        /// </summary>
        public ulong Y;
    }
}
