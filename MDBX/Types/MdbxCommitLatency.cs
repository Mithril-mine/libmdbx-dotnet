using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Информация о задержках при фиксации транзакции.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MdbxCommitLatency
{
    /// <summary>
    /// Длительность подготовки (фиксация дочерних транзакций, обновление записей таблиц и уничтожение курсоров).
    /// </summary>
    public uint Preparation;

    /// <summary>
    /// Длительность обновления GC по стене часов.
    /// </summary>
    public uint GcWallclock;

    /// <summary>
    /// Длительность внутреннего аудита, если он включен.
    /// </summary>
    public uint Audit;

    /// <summary>
    /// Длительность записи грязных/измененных страниц данных в файловую систему.
    /// </summary>
    public uint Write;

    /// <summary>
    /// Длительность синхронизации записанных данных на диск/хранилище.
    /// </summary>
    public uint Sync;

    /// <summary>
    /// Длительность завершения транзакции (освобождение ресурсов).
    /// </summary>
    public uint Ending;

    /// <summary>
    /// Общая длительность фиксации транзакции.
    /// </summary>
    public uint Whole;

    /// <summary>
    /// CPU-время в пользовательском режиме, затраченное на обновление GC.
    /// </summary>
    public uint GcCputime;

    /// <summary>
    /// Статистика профилирования GC.
    /// </summary>
    public MdbxGcProf GcProf;

    /// <summary>
    /// Статистика профилирования GC.
    /// </summary>
    public struct MdbxGcProf
    {
        /// <summary>
        /// Количество итераций обновления GC больше 1, если были повторы/перезапуски.
        /// </summary>
        public uint Wloops;

        /// <summary>
        /// Количество итераций слияния элементов GC.
        /// </summary>
        public uint Coalescences;

        /// <summary>
        /// Количество предыдущих надежных/стабильных зафиксированных точек, удаленных при работе в режиме MDBX_UTTERLY_NOSYNC.
        /// </summary>
        public uint Wipes;

        /// <summary>
        /// Количество принудительных фиксаций на диск для избежания роста базы данных при работе вне режима MDBX_UTTERLY_NOSYNC.
        /// </summary>
        public uint Flushes;

        /// <summary>
        /// Количество обращений к механизму Handle-Slow-Readers.
        /// </summary>
        public uint Kicks;

        /// <summary>
        /// Количество медленных/глубоких поисков GC для размещения пользовательских данных.
        /// </summary>
        public uint WorkCounter;

        /// <summary>
        /// Время "по стене часов", затраченное на чтение и поиск внутри GC для пользовательских данных.
        /// </summary>
        public uint WorkRtimeMonotonic;

        /// <summary>
        /// CPU-время в пользовательском режиме, затраченное на подготовку страниц из GC для пользовательских данных.
        /// </summary>
        public uint WorkXtimeCpu;

        /// <summary>
        /// Количество итераций поиска внутри GC при выделении страниц для пользовательских данных.
        /// </summary>
        public uint WorkRsteps;

        /// <summary>
        /// Количество запросов на выделение последовательностей страниц для пользовательских данных.
        /// </summary>
        public uint WorkXpages;

        /// <summary>
        /// Количество страничных ошибок внутри GC при выделении и подготовке страниц для пользовательских данных.
        /// </summary>
        public uint WorkMajflt;

        /// <summary>
        /// Количество выполнений медленного пути GC для обслуживания и обновления самого GC.
        /// </summary>
        public uint SelfCounter;

        /// <summary>
        /// Время "по стене часов", затраченное на чтение и поиск внутри GC для обслуживания самого GC.
        /// </summary>
        public uint SelfRtimeMonotonic;

        /// <summary>
        /// CPU-время в пользовательском режиме, затраченное на подготовку страниц из GC для обслуживания самого GC.
        /// </summary>
        public uint SelfXtimeCpu;

        /// <summary>
        /// Количество итераций поиска внутри GC при выделении страниц для обслуживания самого GC.
        /// </summary>
        public uint SelfRsteps;

        /// <summary>
        /// Количество запросов на выделение последовательностей страниц для самого GC.
        /// </summary>
        public uint SelfXpages;

        /// <summary>
        /// Количество страничных ошибок внутри GC при выделении и подготовке страниц для самого GC.
        /// </summary>
        public uint SelfMajflt;

        /// <summary>
        /// Метрики объема работы и стоимости слияния списков страниц (merge work).
        /// </summary>
        public MdbxPnlMerge PnlMergeWork;

        /// <summary>
        /// Метрики объема работы и стоимости слияния списков страниц (merge self).
        /// </summary>
        public MdbxPnlMerge PnlMergeSelf;

        /// <summary>
        /// Максимальная наблюдаемая разница между последним и старейшим прочитанными MVCC-снимками.
        /// </summary>
        public uint MaxReaderLag;

        /// <summary>
        /// Максимальное отмеченное количество страниц, удерживаемых от повторного использования.
        /// </summary>
        public uint MaxRetainedPages;
    }

    /// <summary>
    /// Метрики слияния списков страниц.
    /// </summary>
    public struct MdbxPnlMerge
    {
        /// <summary>
        /// Время слияния.
        /// </summary>
        public uint Time;

        /// <summary>
        /// Объем слияния.
        /// </summary>
        public ulong Volume;

        /// <summary>
        /// Количество вызовов слияния.
        /// </summary>
        public uint Calls;
    }
}
