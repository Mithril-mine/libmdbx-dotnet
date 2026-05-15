using System.Runtime.InteropServices;



namespace MDBX.Interop;

/// <summary>
/// Геометрия и ограничения размера файла базы данных.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxGeo
{
    /// <summary>Нижний предел размера файла данных.</summary>
    public ulong Lower;
    /// <summary>Верхний предел размера файла данных.</summary>
    public ulong Upper;
    /// <summary>Текущий размер файла данных.</summary>
    public ulong Current;
    /// <summary>Порог уменьшения (shrink) файла данных.</summary>
    public ulong Shrink;
    /// <summary>Шаг увеличения (growth) файла данных.</summary>
    public ulong Grow;
}

/// <summary>
/// Пара 64-битных идентификаторов.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxIdPair
{
    /// <summary>Компонент X идентификатора.</summary>
    public ulong X;
    /// <summary>Компонент Y идентификатора.</summary>
    public ulong Y;
}

/// <summary>
/// Идентификаторы загрузки системы, используемые для контроля целостности данных при перезагрузках.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxBootId
{
    /// <summary>Идентификатор текущей загрузки системы.</summary>
    public MdbxIdPair Current;
    /// <summary>Первый мета-идентификатор загрузки.</summary>
    public MdbxIdPair Meta0;
    /// <summary>Второй мета-идентификатор загрузки.</summary>
    public MdbxIdPair Meta1;
    /// <summary>Третий мета-идентификатор загрузки.</summary>
    public MdbxIdPair Meta2;
}

/// <summary>
/// Статистика операций со страницами памяти.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxPgOpStat
{
    /// <summary>Количество добавленных новых страниц.</summary>
    public ulong Newly;
    /// <summary>Количество страниц, скопированных при обновлении (Copy-On-Write).</summary>
    public ulong Cow;
    /// <summary>Количество клонов грязных страниц родительской транзакции для вложенных транзакций.</summary>
    public ulong Clone;
    /// <summary>Количество разделений (splits) страниц.</summary>
    public ulong Split;
    /// <summary>Количество слияний (merges) страниц.</summary>
    public ulong Merge;
    /// <summary>Количество вытесненных (spilled) грязных страниц на диск.</summary>
    public ulong Spill;
    /// <summary>Количество возвращенных/перечитанных (unspilled/reloaded) страниц.</summary>
    public ulong Unspill;
    /// <summary>Количество явных операций записи на диск (кол-во вызовов, а не страниц).</summary>
    public ulong Wops;
    /// <summary>Количество упреждающих (prefault) операций записи.</summary>
    public ulong Prefault;
    /// <summary>Количество вызовов функции mincore().</summary>
    public ulong Mincore;
    /// <summary>Количество явных операций синхронизации msync на диск.</summary>
    public ulong Msync;
    /// <summary>Количество явных операций синхронизации fsync на диск.</summary>
    public ulong Fsync;
}

/// <summary>
/// Полная информация о состоянии окружения (Environment) libmdbx.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct EnvironmentInfo
{
    /// <summary>Информация о геометрии и границах размера файла данных.</summary>
    public MdbxGeo MiGeo;

    /// <summary>Размер отображения базы данных в памяти (размер memory map).</summary>
    public ulong MiMapsize;

    /// <summary>Текущий фактический размер файла базы данных.</summary>
    public ulong MiDxbFsize;

    /// <summary>Пространство, выделенное под файл базы данных в файловой системе.</summary>
    public ulong MiDxbFallocated;

    /// <summary>Номер последней используемой страницы.</summary>
    public ulong MiLastPgno;

    /// <summary>ID последней зафиксированной (committed) транзакции.</summary>
    public ulong MiRecentTxnid;

    /// <summary>ID последней транзакции чтения (reader transaction).</summary>
    public ulong MiLatterReaderTxnid;

    /// <summary>ID последней транзакции чтения текущего (этого) процесса.</summary>
    public ulong MiSelfLatterReaderTxnid;

    /// <summary>Фиксированный массив из 3 элементов для ID мета-транзакций (размер равен 3).</summary>
    public unsafe fixed ulong MiMetaTxnid[3];

    /// <summary>Фиксированный массив из 3 элементов для сигнатур мета-транзакций (размер равен 3).</summary>
    public unsafe fixed ulong MiMetaSign[3];

    /// <summary>Общее количество слотов для читателей, доступных в окружении.</summary>
    public uint MiMaxreaders;

    /// <summary>Максимальное количество одновременно использованных слотов читателей в окружении.</summary>
    public uint MiNumreaders;

    /// <summary>Размер страницы базы данных.</summary>
    public uint MiDxbPagesize;

    /// <summary>Размер страницы операционной системы.</summary>
    public uint MiSysPagesize;

    /// <summary>Размер блока единого кэша страниц операционной системы (Unified Page Cache).</summary>
    public uint MiSysUpcblk;

    /// <summary>Размер блока ввода-вывода (I/O) файловой системы.</summary>
    public uint MiSysIoblk;

    /// <summary>Уникальный ID загрузки операционной системы для контроля согласованности данных.</summary>
    public MdbxBootId MiBootid;

    /// <summary>Объем данных в байтах, который еще не был явно синхронизирован на диск.</summary>
    public ulong MiUnsyncVolume;

    /// <summary>Текущий порог автоматической синхронизации в байтах.</summary>
    public ulong MiAutosyncThreshold;

    /// <summary>Время, прошедшее с момента перехода в "грязное" (несинхронизированное) состояние, в единицах 1/65536 секунды.</summary>
    public uint MiSinceSyncSeconds16dot16;

    /// <summary>Текущий период автоматической синхронизации в единицах 1/65536 секунды.</summary>
    public uint MiAutosyncPeriodSeconds16dot16;

    /// <summary>Время, прошедшее с момента последней проверки читателей, в единицах 1/65536 секунды.</summary>
    public uint MiSinceReaderCheckSeconds16dot16;

    /// <summary>Текущий режим работы окружения (возвращаемые флаги среды).</summary>
    public uint MiMode;

    /// <summary>Общая статистика операций со страницами памяти в текущей многопроцессной сессии.</summary>
    public MdbxPgOpStat MiPgopStat;

    /// <summary>GUID файла базы данных (DXB).</summary>
    public MdbxIdPair MiDxbid;
}