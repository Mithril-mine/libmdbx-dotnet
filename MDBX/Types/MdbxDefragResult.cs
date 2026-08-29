using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Структура с результатами и прогрессом дефрагментации.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MDBX_defrag_result
{
    /// <summary>Количество страниц, на которое уменьшился размер базы данных.</summary>
    public nint pages_shrinked;
    /// <summary>Общее количество перемещенных страниц.</summary>
    public nuint pages_moved;
    /// <summary>Количество страниц, запланированных к перемещению на следующем этапе.</summary>
    public nuint pages_scheduled;
    /// <summary>Количество страниц, удерживаемых другими процессами через MVCC-снимки.</summary>
    public nuint pages_retained;
    /// <summary>Оценочное количество страниц, которые потенциально могут быть дефрагментированы.</summary>
    public nuint pages_left;
    /// <summary>Общее количество страниц в базе данных.</summary>
    public nuint pages_whole;
    /// <summary>Номер страницы, на которой остановилась дефрагментация.</summary>
    public nuint obstructed_pgno;
    /// <summary>Длина спана large/overflow-страницы, на котором остановилась дефрагментация.</summary>
    public nuint obstructed_span;
    /// <summary>ID транзакции самого раннего MVCC-снимка, блокирующего дефрагментацию.</summary>
    public ulong obstructed_txnid;
    /// <summary>ID потока читателя, блокирующего дефрагментацию.</summary>
    public uint obstructor_tid;
    /// <summary>ID процесса читателя, блокирующего дефрагментацию.</summary>
    public uint obstructor_pid;
    /// <summary>Примерный прогресс текущего цикла дефрагментации в промилле (1000 = 100%).</summary>
    public uint rough_estimation_cycle_progress_permille;
    /// <summary>Количество выполненных циклов дефрагментации.</summary>
    public uint cycles;
    /// <summary>Причины остановки дефрагментации (маска флагов).</summary>
    public uint stopping_reasons;
    /// <summary>Затраченное время с начала дефрагментации в 1/65536 секунды.</summary>
    public nuint spent_time_dot16;
}
