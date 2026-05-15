namespace MDBX;

using Interop;
using MDBX.Interop.Models;
using MDBX.Options;
using System.Numerics;

/// <summary>
/// Представляет среду базы данных MDBX.
/// </summary>
public class MdbxEnvironment : IDisposable
{

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls
    private bool closed = false;

    /// <summary>
    /// Освобождает протектированные и непротектированные ресурсы.
    /// </summary>
    /// <param name="disposing">true если метод вызван из специальных ресурсов.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // No managed resources to dispose.
                Close();
            }
            else
            {
                try
                {
                    Close();
                }
                catch
                {
                    // Suppress exceptions during finalization.
                }
            }
            disposedValue = true;
        }
    }

    // override a finalizer because Dispose(bool disposing) above has code to free unmanaged resources.
    /// <summary>
    /// Освобождает необработанные ресурсы при вырывании жизни объекта.
    /// </summary>
    ~MdbxEnvironment()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(false);
    }

    // This code added to correctly implement the disposable pattern.
    /// <summary>
    /// Освобождает ресурсы, используемые средой.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    internal readonly IntPtr _envPtr = IntPtr.Zero;

    internal IntPtr _transactionPtr = IntPtr.Zero;

    private readonly object _syncRoot = new object();

    static MdbxEnvironment() => NativeLibraryLoader.Load();

    /// <summary>
    /// Обыкновенный конструктор для создания новых экземпляров MdbxEnvironment.
    /// </summary>
    public MdbxEnvironment() => _envPtr = Environment.Create();

    /// <summary>
    /// Закрывает среду базы данных.
    /// </summary>
    /// <param name="dontSync">Если true, то синхронизация не будет проведена.</param>
    public void Close(bool dontSync = false)
    {
        lock (_syncRoot)
        {
            if (closed) return;
            Environment.Close(_envPtr, dontSync);
            closed = true;
        }
    }

    /// <summary>
    /// Открыть дескриптор среды.
    ///
    /// Эта функция выделяет память для структуры MDBX_env. Для освобождения
    /// выделенной памяти и отмены дескриптора, вызовите mdbx_env_close().
    /// Возможные исключения:
    ///    - MDBX_VERSION_MISMATCH - версия библиотеки MDBX не соответствует
    ///      версии, которая создала среду базы данных.
    ///    - MDBX_INVALID  - заголовки файла среды повреждены.
    ///    - MDBX_ENOENT   - директория, указанная параметром path, не существует.
    ///    - MDBX_EACCES   - у пользователя нет прав на доступ к файлам среды.
    ///    - MDBX_EAGAIN   - среда была заблокирована другим процессом.
    /// </summary>
    /// <param name="path">Путь к среде.</param>
    /// <param name="flags">Флаги окружения.</param>
    /// <param name="mode">Права доступа к файлам (в стиле POSIX).</param>
    public void Open(string path, EnvironmentFlag flags, UnixFileMode mode)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        lock (_syncRoot)
        {
            if (closed) throw new InvalidOperationException("MDBX environment is closed.");
            Environment.Open(_envPtr, path, flags, (int)mode);
        }
    }


    /// <summary>
    /// Создать транзакцию для использования в среде.
    ///
    /// Дескриптор транзакции может быть отменён с помощью Abort() или Commit();
    /// ПРИМЕЧАНИЕ: Транзакция и её курсоры должны использоваться только одним
    /// потоком, и поток может иметь только одну транзакцию одновременно.
    /// Если используется MDBX_NOTLS, это не применяется к транзакциям только для чтения.
    /// ПРИМЕЧАНИЕ: Курсоры не могут пересекать транзакции.
    /// </summary>
    /// <param name="flags">Флаги транзакции.</param>
    /// <returns>Созданная транзакция.</returns>
    public MdbxTransaction BeginTransaction(TransactionOption flags = TransactionOption.Unspecific)
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                IntPtr ptr = Transaction.Begin(_envPtr, IntPtr.Zero, flags);

                _transactionPtr = ptr;

                return new MdbxTransaction(this, ptr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }

    /// <summary>
    /// Возвращает информацию о среде MDBX.
    /// По крайней мере один из аргументов env или txn не должен быть нулевым.
    /// Если аргумент txn не равен нулю, то stat будет заполнен в соответствии с указанной транзакцией. 
    /// В противном случае, если аргумент txn равен нулю, stat будет заполнен на основе снимка 
    /// последней зафиксированной транзакции записи,
    /// а в следующий раз может быть возвращена другая информация.
    /// </summary>
    /// <returns>Информация о среде.</returns>
    public EnvironmentInfo InfoEx()
    {
        lock (_syncRoot)
        {
            if (!closed)
            {
                return Environment.InfoEx(_envPtr, _transactionPtr);
            }

            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }

    /// <summary>
    /// Возвращает статистику о среде MDBX.
    /// </summary>
    /// <returns>Статистика среды.</returns>
    public EnvironmentStat Stat()
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Environment.Stat(_envPtr, _transactionPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }


    /// <summary>
    /// Сбросить буферы данных на диск.
    ///
    /// Данные всегда записываются на диск при вызове mdbx_txn_commit(),
    /// но операционная система может их буферизировать. MDBX всегда сбрасывает
    /// буферы ОС при фиксации также, если только среда не была открыта
    /// с флагом MDBX_NOSYNC или частично с MDBX_NOMETASYNC.
    /// Этот вызов недопустим, если среда была открыта с MDBX_RDONLY.
    /// </summary>
    /// <param name="force">
    /// Если ненулевое, выполнить принудительный синхронный сброс. В противном случае,
    /// если среда имеет установленный флаг MDBX_NOSYNC, сбросы будут опущены,
    /// а с MDBX_MAPASYNC они будут асинхронными.
    /// </param>
    public void Sync(bool force)
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.Sync(_envPtr, force);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
    }

    /// <summary>
    /// Сброс буферов данных среды на диск.
    /// Если среда не была открыта с флагами отсутствия синхронизации (MDBX_NOMETASYNC, MDBX_SAFE_NOSYNC и MDBX_UTTERLY_NOSYNC), 
    /// то данные всегда записываются на диск и сбрасываются в него при вызове mdbx_txn_commit(). 
    /// В противном случае можно вызвать mdbx_env_sync(), чтобы вручную записать на диск несинхронизированные данные и сбросить их.
    /// 
    /// Кроме того, функция mdbx_env_sync_ex() с аргументом force=false может использоваться для обеспечения 
    /// режима опроса при отложенной/асинхронной синхронизации в сочетании с функциями mdbx_env_set_syncbytes() и/или mdbx_env_set_syncperiod().
    /// </summary>
    public void SyncEx(bool force, bool nonblock)
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.SyncEx(_envPtr, force, nonblock);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
    }

    /// <summary>
    /// Установить максимальное количество именованных баз данных для среды.
    /// Эта функция нужна только если несколько баз данных будут использоваться
    /// в среде. Более простые приложения, которые используют среду как одну
    /// безымянную базу данных, могут игнорировать эту опцию.
    ///
    /// Эта функцию может быть вызвана только после mdbx_env_create() и перед
    /// mdbx_env_open().
    /// </summary>
    /// <param name="num">Максимальное количество баз данных.</param>
    /// <returns>this для цепочки вызовов.</returns>
    public MdbxEnvironment SetMaxDatabases(uint num)
    {
        if (num == 0)
            throw new ArgumentOutOfRangeException(nameof(num), "Number of databases must be greater than zero.");
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.SetMaxDBs(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
        return this;
    }

    /// <summary>
    /// Установить максимальное количество слотов для потоков-читателей в среде.
    ///
    /// Это определяет количество слотов в таблице блокировок, которая используется
    /// для отслеживания читателей в среде. Значение по умолчанию - 61.
    /// Запуск транзакции только для чтения обычно привязывает слот таблицы
    /// блокировок к текущему потоку до закрытия среды или завершения потока.
    /// Если используется MDBX_NOTLS, mdbx_txn_begin() вместо этого привязывает
    /// слот к объекту MDBX_txn до его уничтожения или уничтожения объекта MDBX_env.
    /// Эта функция может быть вызвана только после mdbx_env_create() и перед Open().
    /// </summary>
    /// <param name="num">Максимальное количество читателей.</param>
    /// <returns>this для цепочки вызовов.</returns>
    public MdbxEnvironment SetMaxReaders(uint num)
    {
        if (num == 0)
            throw new ArgumentOutOfRangeException(nameof(num), "Number of readers must be greater than zero.");
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.SetMaxReaders(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
        return this;
    }

    /// <summary>
    /// Установить размер области памяти (memory map) для этой среды.
    ///
    /// Размер должен быть кратен размеру страницы ОС. По умолчанию -
    /// 10485760 байт. Размер memory map также является максимальным размером
    /// базы данных. Значение должно выбираться как можно большим,
    /// чтобы вместить будущий рост базы данных.
    ///
    /// Эту функцию следует вызывать перед Open().
    /// Её можно вызывать позже, если в этом процессе нет активных транзакций.
    /// Обратите внимание, что библиотека не проверяет это условие, вызывающий
    /// должен обеспечить его явно.
    ///
    /// Новый размер вступает в силу немедленно для текущего процесса, но
    /// не будет сохранён в других до тех пор, пока пишущая транзакция не будет
    /// зафиксирована текущим процессом. Также, только увеличения mapsize
    /// сохраняются в среде.
    ///
    /// Если mapsize увеличен другим процессом и данные выросли
    /// за пределы текущего mapsize, mdbx_txn_begin() вернёт
    /// MDBX_MAP_RESIZED. Эту функцию можно вызвать с размером
    /// равным нулю, чтобы принять новый размер.
    ///
    /// Любая попытка установить размер меньше, чем пространство, уже занятое средой,
    /// будет тихо изменена на текущий размер используемого пространства.
    /// </summary>
    /// <param name="num">Новый размер в байтах.</param>
    /// <returns>this для цепочки вызовов.</returns>
    public MdbxEnvironment SetMapSize(uint num)
    {
        if (num == 0)
            throw new ArgumentOutOfRangeException(nameof(num), "Map size must be greater than zero.");
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.SetMapSize(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
        return this;
    }


    /// <summary>
    /// Устанавливает или изменяет флаги среды.
    /// </summary>
    /// <param name="flags">Флаги для задания.</param>
    /// <param name="option">На что операция для флагов.</param>
    public void SetFlags(EnvironmentFlag flags, EnvironmentFlagOption option = EnvironmentFlagOption.Add)
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Environment.SetFlags(_envPtr, flags, option == EnvironmentFlagOption.Add);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
    }

    /// <summary>
    /// Получает текущие флаги среды.
    /// </summary>
    /// <returns>Текущие флаги среды.</returns>
    public EnvironmentFlag GetFlags()
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Environment.GetFlags(_envPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }

    /// <summary>
    /// Получить текущее максимальное количество потоков-читателей для среды.
    /// </summary>
    /// <returns>Максимальное количество читателей.</returns>
    public int GetMaxReaders()
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Environment.GetMaxReaders(_envPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }

    /// <summary>
    /// Получить максимальный размер ключей и данных MDBX_DUPSORT, которые можно записать.
    /// </summary>
    /// <returns>Максимальный размер ключа в байтах.</returns>
    public int GetMaxKeySize()
    {
        lock (_syncRoot)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Environment.GetMaxKeySize(_envPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }
    }
}
