using System.Runtime.InteropServices;
using MDBX.Native.Types;

namespace MDBX;

/// <summary>
/// Высокоуровневая обертка окружения libmdbx.
/// </summary>
public sealed unsafe class MdbxEnvironment : IDisposable
{
    private MDBX_env* _env;
    private bool _disposed;
    private bool _ownsEnv;
    private GCHandle? _hsrHandle;
    private static GCHandle? _debugLoggerHandle;
    private static GCHandle? _debugLoggerNofmtHandle;

    private MdbxEnvironment(MDBX_env* env, bool ownsEnv)
    {
        _env = env;
        _ownsEnv = ownsEnv;
    }

    /// <summary>
    /// Создает новое окружение MDBX.
    /// </summary>
    /// <returns>Новое окружение.</returns>
    public static MdbxEnvironment Create()
    {
        MDBX_env* env = null;
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env);
        if (rc != 0)
            throw new MdbxException(rc);
        return new MdbxEnvironment(env, true);
    }

    /// <summary>
    /// Создает и открывает окружение MDBX.
    /// </summary>
    /// <param name="path">Путь к базе данных.</param>
    /// <param name="flags">Флаги окружения.</param>
    /// <param name="mode">Режим доступа (права доступа для создания файлов).</param>
    /// <param name="maxDatabases">Максимальное количество таблиц (0 = по умолчанию).</param>
    /// <returns>Открытое окружение.</returns>
    public static MdbxEnvironment Open(string path, MdbxEnvFlags flags = MdbxEnvFlags.MDBX_ENV_DEFAULTS, ushort mode = 0x700, int maxDatabases = 0)
    {
        var env = Create();
        if (maxDatabases > 0)
            env.SetOption(MdbxOption.MDBX_OPT_MAX_DB, (ulong)maxDatabases);
        env.OpenInternal(path, flags, mode);
        return env;
    }

    /// <summary>
    /// Открывает окружение MDBX.
    /// </summary>
    /// <param name="path">Путь к базе данных.</param>
    /// <param name="flags">Флаги окружения.</param>
    /// <param name="mode">Режим доступа (права доступа для создания файлов).</param>
    public void OpenInternal(string path, MdbxEnvFlags flags = MdbxEnvFlags.MDBX_ENV_DEFAULTS, ushort mode = 0x700)
    {
        ThrowIfDisposed();
        int rc;
        if (OperatingSystem.IsWindows())
        {
            char[] pathChars = StringToNullTerminatedChars(path);
            fixed (char* pathPtr = pathChars)
                rc = Native.Bindings.Env.NativeMdbx.MdbxEnvOpenW(_env, pathPtr, flags, mode);
        }
        else
        {
            byte[] pathBytes = StringToNullTerminatedBytes(path);
            fixed (byte* pathPtr = pathBytes)
                rc = Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(_env, pathPtr, flags, mode);
        }
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Закрывает окружение MDBX.
    /// </summary>
    public void Close()
    {
        Close(false);
    }

    /// <summary>
    /// Закрывает окружение MDBX с опциональной отменой синхронизации.
    /// </summary>
    /// <param name="dontSync">Не синхронизировать при закрытии.</param>
    public void Close(bool dontSync)
    {
        if (_env == null)
            return;
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvCloseEx(_env, dontSync);
        _env = null;
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Удаляет окружение MDBX по пути.
    /// </summary>
    /// <param name="path">Путь к окружению.</param>
    /// <param name="mode">Режим удаления.</param>
    public static void Delete(string path, MdbxEnvDeleteMode mode = MdbxEnvDeleteMode.MDBX_ENV_DELETE_ALL)
    {
        byte[] pathBytes = StringToNullTerminatedBytes(path);
        fixed (byte* pathPtr = pathBytes)
        {
            int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvDelete(pathPtr, mode);
            if (rc != 0)
                throw new MdbxException(rc);
        }
    }

    /// <summary>
    /// Копирует окружение MDBX в указанный путь.
    /// </summary>
    /// <param name="destinationPath">Путь назначения.</param>
    /// <param name="flags">Флаги копирования.</param>
    public void Copy(string destinationPath, MdbxCopyFlags flags = MdbxCopyFlags.MDBX_CP_DEFAULT)
    {
        ThrowIfDisposed();
        byte[] destBytes = StringToNullTerminatedBytes(destinationPath);
        fixed (byte* destPtr = destBytes)
        {
            int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvCopy(_env, destPtr, flags);
            if (rc != 0)
                throw new MdbxException(rc);
        }
    }

    /// <summary>
    /// Копирует окружение MDBX в файловый дескриптор.
    /// </summary>
    /// <param name="fd">Файловый дескриптор.</param>
    /// <param name="flags">Флаги копирования.</param>
    public void CopyToFd(IntPtr fd, MdbxCopyFlags flags = MdbxCopyFlags.MDBX_CP_DEFAULT)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvCopy2Fd(_env, fd, flags);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Открывает окружение для восстановления.
    /// </summary>
    /// <param name="path">Путь к базе данных.</param>
    /// <param name="targetMeta">Целевая мета-страница.</param>
    /// <param name="writable">Доступно для записи.</param>
    public void OpenForRecovery(string path, uint targetMeta, bool writable)
    {
        ThrowIfDisposed();
        int rc;
        if (OperatingSystem.IsWindows())
        {
            char[] pathChars = StringToNullTerminatedChars(path);
            fixed (char* pathPtr = pathChars)
                rc = Native.Bindings.Env.NativeMdbx.MdbxEnvOpenForRecoveryW(_env, pathPtr, targetMeta, writable);
        }
        else
        {
            byte[] pathBytes = StringToNullTerminatedBytes(path);
            fixed (byte* pathPtr = pathBytes)
                rc = Native.Bindings.Env.NativeMdbx.MdbxEnvOpenForRecovery(_env, pathPtr, targetMeta, writable);
        }
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Переключает окружение для восстановления на указанную мета-страницу.
    /// </summary>
    /// <param name="targetMeta">Целевая мета-страница.</param>
    public void TurnForRecovery(uint targetMeta)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvTurnForRecovery(_env, targetMeta);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Восстанавливает окружение после fork.
    /// </summary>
    public void ResurrectAfterFork()
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvResurrectAfterFork(_env);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Прогревает окружение (предзагружает страницы в память).
    /// </summary>
    /// <param name="txn">Транзакция (может быть null).</param>
    /// <param name="flags">Флаги прогрева.</param>
    /// <param name="timeoutSeconds16Dot16">Таймаут в 1/65536 сек (0 = без ограничения).</param>
    public void Warmup(MDBX_txn* txn = null, MdbxWarmupFlags flags = MdbxWarmupFlags.MDBX_WARMUP_DEFAULT, uint timeoutSeconds16Dot16 = 0)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvWarmup(_env, txn, flags, timeoutSeconds16Dot16);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Фрагментирует окружение (defrag).
    /// </summary>
    /// <param name="defragAtleast">Минимальное количество страниц для дефрагментации (0 = по умолчанию).</param>
    /// <param name="timeAtleastDot16">Минимальное время в 1/65536 сек (0 = по умолчанию).</param>
    /// <param name="defragEnough">Достаточное количество страниц (0 = по умолчанию).</param>
    /// <param name="timeLimitDot16">Лимит времени в 1/65536 сек (0 = без лимита).</param>
    /// <param name="acceptableBacklash">Допустимый откат.</param>
    /// <param name="preferredBatch">Предпочтительный размер пакета.</param>
    /// <param name="progressCallback">Функция обратного вызова для уведомления о прогрессе.</param>
    public void Defrag(
        nuint defragAtleast = 0,
        nuint timeAtleastDot16 = 0,
        nuint defragEnough = 0,
        nuint timeLimitDot16 = 0,
        nint acceptableBacklash = 0,
        nint preferredBatch = 0,
        MdbxDefragProgressFunc? progressCallback = null)
    {
        ThrowIfDisposed();
        GCHandle? handle = null;
        delegate* unmanaged[Cdecl]<void*, MDBX_defrag_result_t*, int> callbackPtr = null;
        if (progressCallback != null)
        {
            handle = GCHandle.Alloc(progressCallback);
            callbackPtr = (delegate* unmanaged[Cdecl]<void*, MDBX_defrag_result_t*, int>)Marshal.GetFunctionPointerForDelegate(progressCallback);
        }
        
        MDBX_defrag_result result = default;
        try
        {
            int rc = Native.Bindings.Env.NativeMdbx.MdbxEnvDefrag(
                _env, defragAtleast, timeAtleastDot16, defragEnough, timeLimitDot16,
                acceptableBacklash, preferredBatch, callbackPtr, null, &result);
            if (rc != 0)
                throw new MdbxException(rc);
        }
        finally
        {
            handle?.Free();
        }
    }

    /// <summary>
    /// Синхронизирует окружение на диск.
    /// </summary>
    /// <param name="force">Принудительная синхронизация.</param>
    /// <param name="nonblock">Неблокирующая синхронизация.</param>
    public void Sync(bool force = false, bool nonblock = false)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Stat.NativeMdbx.MdbxEnvSyncEx(_env, force, nonblock);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает статистику окружения.
    /// </summary>
    public MdbxStat GetStat()
    {
        ThrowIfDisposed();
        MdbxStat stat = default;
        int rc = Native.Bindings.Stat.NativeMdbx.MdbxEnvStatEx(_env, null, &stat, (nuint)sizeof(MdbxStat));
        if (rc != 0)
            throw new MdbxException(rc);
        return stat;
    }

    /// <summary>
    /// Получает информацию об окружении.
    /// </summary>
    public MDBX_envinfo GetInfo()
    {
        ThrowIfDisposed();
        MDBX_envinfo info = default;
        int rc = Native.Bindings.Stat.NativeMdbx.MdbxEnvInfoEx(_env, null, &info, (nuint)sizeof(MDBX_envinfo));
        if (rc != 0)
            throw new MdbxException(rc);
        return info;
    }

    /// <summary>
    /// Устанавливает параметр окружения.
    /// </summary>
    /// <param name="option">Параметр.</param>
    /// <param name="value">Значение.</param>
    public void SetOption(MdbxOption option, ulong value)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(_env, option, value);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает параметр окружения.
    /// </summary>
    /// <param name="option">Параметр.</param>
    /// <returns>Значение параметра.</returns>
    public ulong GetOption(MdbxOption option)
    {
        ThrowIfDisposed();
        ulong value = 0;
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetOption(_env, option, &value);
        if (rc != 0)
            throw new MdbxException(rc);
        return value;
    }

    /// <summary>
    /// Устанавливает флаги окружения.
    /// </summary>
    /// <param name="flags">Флаги.</param>
    /// <param name="on">Включить или выключить.</param>
    public void SetFlags(MdbxEnvFlags flags, bool on)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvSetFlags(_env, flags, on);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает флаги окружения.
    /// </summary>
    /// <returns>Флаги окружения.</returns>
    public MdbxEnvFlags GetFlags()
    {
        ThrowIfDisposed();
        uint flags = 0;
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetFlags(_env, &flags);
        if (rc != 0)
            throw new MdbxException(rc);
        return (MdbxEnvFlags)flags;
    }

    /// <summary>
    /// Устанавливает геометрию окружения (размеры файлов).
    /// </summary>
    /// <param name="sizeLower">Нижний предел размера (-1 = оставить без изменений).</param>
    /// <param name="sizeNow">Текущий размер (-1 = оставить без изменений).</param>
    /// <param name="sizeUpper">Верхний предел размера (-1 = оставить без изменений).</param>
    /// <param name="growthStep">Шаг роста (-1 = по умолчанию).</param>
    /// <param name="shrinkThreshold">Порог сжатия (-1 = по умолчанию).</param>
    /// <param name="pageSize">Размер страницы (0 = по умолчанию).</param>
    public void SetGeometry(long sizeLower = -1, long sizeNow = -1, long sizeUpper = -1, long growthStep = -1, long shrinkThreshold = -1, long pageSize = 0)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvSetGeometry(
            _env, (nint)sizeLower, (nint)sizeNow, (nint)sizeUpper,
            (nint)growthStep, (nint)shrinkThreshold, (nint)pageSize);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Устанавливает функцию обработки медленных читателей (HSR).
    /// </summary>
    /// <param name="callback">Функция обратного вызова (null для сброса).</param>
    public void SetHsr(MdbxHsrFunc? callback)
    {
        ThrowIfDisposed();
        _hsrHandle?.Free();
        _hsrHandle = null;
        
        delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int> callbackPtr = null;
        if (callback != null)
        {
            _hsrHandle = GCHandle.Alloc(callback);
            callbackPtr = (delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int>)Marshal.GetFunctionPointerForDelegate(callback);
        }
        
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvSetHsr(_env, callbackPtr);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает функцию обработки медленных читателей (HSR).
    /// </summary>
    /// <returns>Функция обратного вызова или null.</returns>
    public MdbxHsrFunc? GetHsr()
    {
        ThrowIfDisposed();
        var ptr = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetHsr(_env);
        if (ptr == null)
            return null;
        return Marshal.GetDelegateForFunctionPointer<MdbxHsrFunc>((IntPtr)ptr);
    }

    /// <summary>
    /// Устанавливает пользовательский контекст окружения.
    /// </summary>
    /// <param name="context">Пользовательский контекст.</param>
    public void SetUserContext(void* context)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvSetUserctx(_env, context);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает пользовательский контекст окружения.
    /// </summary>
    /// <returns>Указатель на пользовательский контекст.</returns>
    public void* GetUserContext()
    {
        ThrowIfDisposed();
        return Native.Bindings.Settings.NativeMdbx.MdbxEnvGetUserctx(_env);
    }

    /// <summary>
    /// Получает путь к окружению.
    /// </summary>
    public string Path
    {
        get
        {
            ThrowIfDisposed();
            int rc;
            if (OperatingSystem.IsWindows())
            {
                char* pathPtr = null;
                rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPathW(_env, &pathPtr);
                if (rc != 0)
                    throw new MdbxException(rc);
                if (pathPtr == null)
                    return string.Empty;
                return Marshal.PtrToStringUni((IntPtr)pathPtr)!;
            }
            else
            {
                byte* pathPtr = null;
                rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPath(_env, &pathPtr);
                if (rc != 0)
                    throw new MdbxException(rc);
                if (pathPtr == null)
                    return string.Empty;
                return Marshal.PtrToStringUTF8((IntPtr)pathPtr)!;
            }
        }
    }

    /// <summary>
    /// Получает дескриптор файла окружения.
    /// </summary>
    public IntPtr FileDescriptor
    {
        get
        {
            ThrowIfDisposed();
            IntPtr fd = IntPtr.Zero;
            int rc = Native.Bindings.Settings.NativeMdbx.MdbxEnvGetFd(_env, &fd);
            if (rc != 0)
                throw new MdbxException(rc);
            return fd;
        }
    }

    /// <summary>
    /// Получает максимальный размер ключа.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public int GetMaxKeySize(MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS)
    {
        ThrowIfDisposed();
        return Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxkeysizeEx(_env, flags);
    }

    /// <summary>
    /// Получает максимальный размер значения.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public int GetMaxValueSize(MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS)
    {
        ThrowIfDisposed();
        return Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxvalsizeEx(_env, flags);
    }

    /// <summary>
    /// Получает максимальный размер пары ключ-значение для страницы.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public int GetMaxPairSizeForPage(MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS)
    {
        ThrowIfDisposed();
        return Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPairsize4pageMax(_env, flags);
    }

    /// <summary>
    /// Получает максимальный размер значения для страницы.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public int GetMaxValueSizeForPage(MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS)
    {
        ThrowIfDisposed();
        return Native.Bindings.Settings.NativeMdbx.MdbxEnvGetValsize4pageMax(_env, flags);
    }

    /// <summary>
    /// Получает размер страницы по умолчанию.
    /// </summary>
    public static nuint DefaultPageSize => Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();

    /// <summary>
    /// Получает информацию о системной RAM.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="totalPages">Общее количество страниц.</param>
    /// <param name="availPages">Доступное количество страниц.</param>
    public static void GetSystemRamInfo(out nint pageSize, out nint totalPages, out nint availPages)
    {
        nint pageSizeLocal = 0;
        nint totalPagesLocal = 0;
        nint availPagesLocal = 0;
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxGetSysraminfo(&pageSizeLocal, &totalPagesLocal, &availPagesLocal);
        if (rc != 0)
            throw new MdbxException(rc);
        pageSize = pageSizeLocal;
        totalPages = totalPagesLocal;
        availPages = availPagesLocal;
    }

    /// <summary>
    /// Проверяет, разумно ли использовать readahead для заданного объема.
    /// </summary>
    /// <param name="volume">Объем данных.</param>
    /// <param name="redundancy">Избыточность.</param>
    public static bool IsReadaheadReasonable(nuint volume, nint redundancy)
    {
        int rc = Native.Bindings.Settings.NativeMdbx.MdbxIsReadaheadReasonable(volume, redundancy);
        return rc == -1;
    }

    /// <summary>
    /// Получает минимальный размер базы данных.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    public static nint LimitsDatabaseSizeMin(nint pageSize) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsDbsizeMin(pageSize);

    /// <summary>
    /// Получает максимальный размер базы данных.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    public static nint LimitsDatabaseSizeMax(nint pageSize) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsDbsizeMax(pageSize);

    /// <summary>
    /// Получает минимальный размер ключа.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsKeySizeMin(MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsKeysizeMin(flags);

    /// <summary>
    /// Получает максимальный размер ключа.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsKeySizeMax(nint pageSize, MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsKeysizeMax(pageSize, flags);

    /// <summary>
    /// Получает минимальный размер значения.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsValueSizeMin(MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsizeMin(flags);

    /// <summary>
    /// Получает максимальный размер значения.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsValueSizeMax(nint pageSize, MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsizeMax(pageSize, flags);

    /// <summary>
    /// Получает максимальный размер пары ключ-значение для страницы.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsPairSizeForPageMax(nint pageSize, MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsPairsize4pageMax(pageSize, flags);

    /// <summary>
    /// Получает максимальный размер значения для страницы.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="flags">Флаги таблицы.</param>
    public static nint LimitsValueSizeForPageMax(nint pageSize, MdbxDbFlags flags) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsize4pageMax(pageSize, flags);

    /// <summary>
    /// Получает максимальный размер транзакции.
    /// </summary>
    /// <param name="pageSize">Размер страницы.</param>
    public static nint LimitsTransactionSizeMax(nint pageSize) => Native.Bindings.Settings.NativeMdbx.MdbxLimitsTxnsizeMax(pageSize);

    /// <summary>
    /// Открывает именованную таблицу (DBI).
    /// </summary>
    /// <param name="name">Имя таблицы (null для дефолтной таблицы).</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Идентификатор таблицы (DBI).</returns>
    public uint OpenDatabase(string? name = null, MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS | MdbxDbFlags.MDBX_CREATE)
    {
        ThrowIfDisposed();
        MDBX_txn* txn = null;
        int rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(_env, null, MdbxTxnFlags.MDBX_TXN_DEFAULTS, &txn, 0);
        if (rc != 0)
            throw new MdbxException(rc);
        
        try
        {
            byte[]? nameBytes = null;
            byte* namePtr = null;
            if (!string.IsNullOrEmpty(name))
            {
                nameBytes = StringToNullTerminatedBytes(name);
                fixed (byte* ptr = nameBytes)
                    namePtr = ptr;
            }
            uint dbi = 0;
            rc = Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, flags, &dbi);
            if (rc != 0)
                throw new MdbxException(rc);
            
            rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null);
            if (rc != 0)
                throw new MdbxException(rc);
            return dbi;
        }
        catch
        {
            if (txn != null)
                Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(txn, null);
            throw;
        }
    }

    /// <summary>
    /// Начинает новую транзакцию.
    /// </summary>
    /// <param name="flags">Флаги транзакции.</param>
    /// <returns>Новая транзакция.</returns>
    public MdbxTransaction BeginTransaction(MdbxTxnFlags flags = MdbxTxnFlags.MDBX_TXN_DEFAULTS)
    {
        ThrowIfDisposed();
        MDBX_txn* txn = null;
        int rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(_env, null, flags, &txn, 0);
        if (rc != 0)
            throw new MdbxException(rc);
        return new MdbxTransaction(this, txn);
    }

    /// <summary>
    /// Открывает именованную таблицу (DBI) и возвращает высокоуровневую обертку.
    /// </summary>
    /// <param name="name">Имя таблицы (null для дефолтной таблицы).</param>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Обертка таблицы (DBI).</returns>
    public MdbxDatabase OpenDatabaseEx(string? name = null, MdbxDbFlags flags = MdbxDbFlags.MDBX_DB_DEFAULTS | MdbxDbFlags.MDBX_CREATE)
    {
        uint dbi = OpenDatabase(name, flags);
        return new MdbxDatabase(this, dbi);
    }

    /// <summary>
    /// Закрывает таблицу (DBI).
    /// </summary>
    /// <param name="dbi">Идентификатор таблицы.</param>
    public void CloseDatabase(uint dbi)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(_env, dbi);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Удаляет таблицу (содержимое и/или саму таблицу).
    /// </summary>
    /// <param name="dbi">Идентификатор таблицы.</param>
    /// <param name="deleteData">Удалить данные.</param>
    public void DropDatabase(uint dbi, bool deleteData = true)
    {
        ThrowIfDisposed();
        MDBX_txn* txn = null;
        int rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(_env, null, MdbxTxnFlags.MDBX_TXN_DEFAULTS, &txn, 0);
        if (rc != 0)
            throw new MdbxException(rc);
        
        try
        {
            rc = Native.Bindings.Dbi.NativeMdbx.MdbxDrop(txn, dbi, deleteData);
            if (rc != 0)
                throw new MdbxException(rc);
            
            rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null);
            if (rc != 0)
                throw new MdbxException(rc);
        }
        catch
        {
            if (txn != null)
                Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(txn, null);
            throw;
        }
    }

    /// <summary>
    /// Переименовывает таблицу.
    /// </summary>
    /// <param name="dbi">Идентификатор таблицы.</param>
    /// <param name="newName">Новое имя таблицы.</param>
    public void RenameDatabase(uint dbi, string newName)
    {
        ThrowIfDisposed();
        MDBX_txn* txn = null;
        int rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(_env, null, MdbxTxnFlags.MDBX_TXN_DEFAULTS, &txn, 0);
        if (rc != 0)
            throw new MdbxException(rc);
        
        try
        {
            byte[] nameBytes = StringToNullTerminatedBytes(newName);
            fixed (byte* namePtr = nameBytes)
            {
                rc = Native.Bindings.Dbi.NativeMdbx.MdbxDbiRename(txn, dbi, namePtr);
                if (rc != 0)
                    throw new MdbxException(rc);
            }
            
            rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null);
            if (rc != 0)
                throw new MdbxException(rc);
        }
        catch
        {
            if (txn != null)
                Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(txn, null);
            throw;
        }
    }

    /// <summary>
    /// Перечисляет все таблицы в окружении.
    /// </summary>
    /// <param name="action">Действие для каждой таблицы (имя, флаги, ID).</param>
    public void EnumerateTables(Action<string, MdbxDbFlags, uint> action)
    {
        ThrowIfDisposed();
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        MDBX_txn* txn = null;
        int rc = Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(_env, null, MdbxTxnFlags.MDBX_TXN_RDONLY, &txn, 0);
        if (rc != 0)
            throw new MdbxException(rc);

        var handle = GCHandle.Alloc(action);
        try
        {
            rc = Native.Bindings.Extra.NativeMdbx.MdbxEnumerateTables(txn, &TableEnumCallback, (void*)GCHandle.ToIntPtr(handle));
            if (rc != 0)
                throw new MdbxException(rc);
        }
        finally
        {
            Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
            handle.Free();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    private static int TableEnumCallback(void* ctx, MDBX_txn* txn, MdbxVal* name, uint flags, uint id)
    {
        var action = (Action<string, MdbxDbFlags, uint>)GCHandle.FromIntPtr((IntPtr)ctx).Target!;
        string tableName = Marshal.PtrToStringUTF8((IntPtr)name->IovBase, (int)name->IovLen) ?? string.Empty;
        action(tableName, (MdbxDbFlags)flags, id);
        return 0;
    }

    /// <summary>
    /// Проверяет читателей и возвращает количество мертвых читателей.
    /// </summary>
    /// <param name="deadReaders">Количество мертвых читателей.</param>
    public void CheckReaders(out int deadReaders)
    {
        ThrowIfDisposed();
        deadReaders = 0;
        int deadReadersLocal = 0;
        int rc = Native.Bindings.Extra.NativeMdbx.MdbxReaderCheck(_env, &deadReadersLocal);
        if (rc != 0)
            throw new MdbxException(rc);
        deadReaders = deadReadersLocal;
    }

    /// <summary>
    /// Перечисляет читателей окружения.
    /// </summary>
    /// <param name="action">Действие для каждого читателя.</param>
    public void EnumerateReaders(Action<int, int, uint, uint, ulong, ulong, string?> action)
    {
        ThrowIfDisposed();
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        var handle = GCHandle.Alloc(action);
        try
        {
            int rc = Native.Bindings.Extra.NativeMdbx.MdbxReaderList(_env, &ReaderListCallback, (void*)GCHandle.ToIntPtr(handle));
            if (rc != 0)
                throw new MdbxException(rc);
        }
        finally
        {
            handle.Free();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    private static int ReaderListCallback(void* ctx, int num, int slot, uint pid, uint thread, ulong txnid, ulong since, MdbxVal* range)
    {
        var action = (Action<int, int, uint, uint, ulong, ulong, string?>)GCHandle.FromIntPtr((IntPtr)ctx).Target!;
        string? rangeStr = range != null && range->IovBase != null
            ? Marshal.PtrToStringUTF8((IntPtr)range->IovBase, (int)range->IovLen)
            : null;
        action(num, slot, pid, thread, txnid, since, rangeStr);
        return 0;
    }

    /// <summary>
    /// Регистрирует текущий поток для работы с окружением.
    /// </summary>
    public void RegisterThread()
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Extra.NativeMdbx.MdbxThreadRegister(_env);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Отменяет регистрацию текущего потока.
    /// </summary>
    public void UnregisterThread()
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Extra.NativeMdbx.MdbxThreadUnregister(_env);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Блокирует окружение для создания транзакции.
    /// </summary>
    /// <param name="dontWait">Не ждать, если окружение заблокировано.</param>
    public void LockTransaction(bool dontWait = false)
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Extra.NativeMdbx.MdbxTxnLock(_env, dontWait);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Разблокирует окружение.
    /// </summary>
    public void UnlockTransaction()
    {
        ThrowIfDisposed();
        int rc = Native.Bindings.Extra.NativeMdbx.MdbxTxnUnlock(_env);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Настраивает отладочное логирование libmdbx.
    /// </summary>
    /// <param name="logLevel">Уровень логирования.</param>
    /// <param name="debugFlags">Флаги отладки.</param>
    /// <param name="logger">Функция логирования (null для отключения).</param>
    public static void SetupDebug(MdbxLogLevel logLevel, MdbxDebugFlags debugFlags, MdbxDebugFunc? logger)
    {
        _debugLoggerHandle?.Free();
        _debugLoggerHandle = null;
        
        delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, byte*, void> callbackPtr = null;
        if (logger != null)
        {
            _debugLoggerHandle = GCHandle.Alloc(logger);
            callbackPtr = (delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, byte*, void>)Marshal.GetFunctionPointerForDelegate(logger);
        }
        
        int rc = Native.Bindings.Debug.NativeMdbx.MdbxSetupDebug(logLevel, debugFlags, callbackPtr);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Настраивает отладочное логирование без форматирования.
    /// </summary>
    /// <param name="logLevel">Уровень логирования.</param>
    /// <param name="debugFlags">Флаги отладки.</param>
    /// <param name="logger">Функция логирования без форматирования (null для отключения).</param>
    /// <param name="loggerBuffer">Буфер для форматирования сообщений.</param>
    /// <param name="loggerBufferSize">Размер буфера.</param>
    public static void SetupDebugNoFormat(MdbxLogLevel logLevel, MdbxDebugFlags debugFlags, MdbxDebugFuncNofmt? logger, byte* loggerBuffer = null, nuint loggerBufferSize = 0)
    {
        _debugLoggerNofmtHandle?.Free();
        _debugLoggerNofmtHandle = null;
        
        delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, void> callbackPtr = null;
        if (logger != null)
        {
            _debugLoggerNofmtHandle = GCHandle.Alloc(logger);
            callbackPtr = (delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, void>)Marshal.GetFunctionPointerForDelegate(logger);
        }
        
        int rc = Native.Bindings.Debug.NativeMdbx.MdbxSetupDebugNofmt(logLevel, debugFlags, callbackPtr, loggerBuffer, loggerBufferSize);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Получает строковое описание кода ошибки.
    /// </summary>
    /// <param name="errorCode">Код ошибки.</param>
    public static string GetErrorMessage(int errorCode)
    {
        byte* ptr = Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerror(errorCode);
        if (ptr == null)
            return $"MDBX error {errorCode}";
        return Marshal.PtrToStringUTF8((IntPtr)ptr)!;
    }

    /// <summary>
    /// Получает нативный указатель на окружение.
    /// </summary>
    public MDBX_env* NativeEnv
    {
        get
        {
            ThrowIfDisposed();
            return _env;
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MdbxEnvironment));
        if (_env == null)
            throw new ObjectDisposedException(nameof(MdbxEnvironment));
    }

    private static byte[] StringToNullTerminatedBytes(string s)
    {
        int byteCount = System.Text.Encoding.UTF8.GetByteCount(s);
        byte[] bytes = new byte[byteCount + 1];
        System.Text.Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
        bytes[byteCount] = 0;
        return bytes;
    }

    private static char[] StringToNullTerminatedChars(string s)
    {
        char[] chars = new char[s.Length + 1];
        s.CopyTo(0, chars, 0, s.Length);
        chars[s.Length] = '\0';
        return chars;
    }

    private static delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int> MarshalGetFunctionPointerForDelegate(MdbxHsrFunc func)
    {
        var handle = GCHandle.Alloc(func);
        return (delegate* unmanaged[Cdecl]<MDBX_env*, MDBX_txn*, uint, uint, ulong, uint, nuint, int>)Marshal.GetFunctionPointerForDelegate(func);
    }

    private static delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, byte*, void> MarshalGetFunctionPointerForDebugLogger(MdbxDebugFunc func)
    {
        return (delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, byte*, void>)Marshal.GetFunctionPointerForDelegate(func);
    }

    private static delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, void> MarshalGetFunctionPointerForDebugLoggerNofmt(MdbxDebugFuncNofmt func)
    {
        return (delegate* unmanaged[Cdecl]<MdbxLogLevel, byte*, int, byte*, void>)Marshal.GetFunctionPointerForDelegate(func);
    }

    private static delegate* unmanaged[Cdecl]<void*, MDBX_defrag_result_t*, int> MarshalGetFunctionPointerForDefragProgress(MdbxDefragProgressFunc func)
    {
        return (delegate* unmanaged[Cdecl]<void*, MDBX_defrag_result_t*, int>)Marshal.GetFunctionPointerForDelegate(func);
    }

    /// <summary>
    /// Освобождает ресурсы окружения.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~MdbxEnvironment()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            try
            {
                Close();
            }
            catch
            {
                if (disposing)
                    throw;
            }
            finally
            {
                _hsrHandle?.Free();
                _hsrHandle = null;
                _env = null;
            }
            _disposed = true;
        }
    }
}
