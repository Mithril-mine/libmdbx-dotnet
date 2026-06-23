using MDBX.Native.Types;
using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Extra;

/// <summary>
/// Частичный класс для нативных методов дополнительных операций libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Сравнивает ключи.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="a">Первый ключ.</param>
    /// <param name="b">Второй ключ.</param>
    /// <returns>Результат сравнения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cmp", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCmp(MDBX_txn* txn, uint dbi, MDBX_val* a, MDBX_val* b);

    /// <summary>
    /// Сравнивает значения.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="a">Первое значение.</param>
    /// <param name="b">Второе значение.</param>
    /// <returns>Результат сравнения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dcmp", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDcmp(MDBX_txn* txn, uint dbi, MDBX_val* a, MDBX_val* b);

    /// <summary>
    /// Получает функцию сравнения ключей.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Указатель на функцию сравнения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get_keycmp", CallingConvention = CallingConvention.Cdecl)]
    public static extern delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> MdbxGetKeycmp(MDBX_db_flags_t flags);

    /// <summary>
    /// Получает функцию сравнения значений.
    /// </summary>
    /// <param name="flags">Флаги таблицы.</param>
    /// <returns>Указатель на функцию сравнения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_get_datacmp", CallingConvention = CallingConvention.Cdecl)]
    public static extern delegate* unmanaged[Cdecl]<MDBX_val*, MDBX_val*, int> MdbxGetDatacmp(MDBX_db_flags_t flags);

    /// <summary>
    /// Преобразует JSON integer в ключ.
    /// </summary>
    /// <param name="jsonInteger">JSON integer.</param>
    /// <returns>Ключ.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_key_from_jsonInteger", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong MdbxKeyFromJsonInteger(long jsonInteger);

    /// <summary>
    /// Преобразует double в ключ.
    /// </summary>
    /// <param name="ieee754_64bit">Double значение.</param>
    /// <returns>Ключ.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_key_from_double", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong MdbxKeyFromDouble(double ieee754_64bit);

    /// <summary>
    /// Преобразует указатель на double в ключ.
    /// </summary>
    /// <param name="ieee754_64bit">Указатель на double.</param>
    /// <returns>Ключ.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_key_from_ptrdouble", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong MdbxKeyFromPtrdouble(double* ieee754_64bit);

    /// <summary>
    /// Преобразует float в ключ.
    /// </summary>
    /// <param name="ieee754_32bit">Float значение.</param>
    /// <returns>Ключ.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_key_from_float", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint MdbxKeyFromFloat(float ieee754_32bit);

    /// <summary>
    /// Преобразует указатель на float в ключ.
    /// </summary>
    /// <param name="ieee754_32bit">Указатель на float.</param>
    /// <returns>Ключ.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_key_from_ptrfloat", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint MdbxKeyFromPtrfloat(float* ieee754_32bit);

    /// <summary>
    /// Извлекает JSON integer из ключа.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>JSON integer.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_jsonInteger_from_key", CallingConvention = CallingConvention.Cdecl)]
    public static extern long MdbxJsonIntegerFromKey(MDBX_val key);

    /// <summary>
    /// Извлекает double из ключа.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Double значение.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_double_from_key", CallingConvention = CallingConvention.Cdecl)]
    public static extern double MdbxDoubleFromKey(MDBX_val key);

    /// <summary>
    /// Извлекает float из ключа.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Float значение.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_float_from_key", CallingConvention = CallingConvention.Cdecl)]
    public static extern float MdbxFloatFromKey(MDBX_val key);

    /// <summary>
    /// Извлекает int32 из ключа.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Int32 значение.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_int32_from_key", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxInt32FromKey(MDBX_val key);

    /// <summary>
    /// Извлекает int64 из ключа.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Int64 значение.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_int64_from_key", CallingConvention = CallingConvention.Cdecl)]
    public static extern long MdbxInt64FromKey(MDBX_val key);

    /// <summary>
    /// Перечисляет таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="func">Функция обратного вызова.</param>
    /// <param name="ctx">Контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_enumerate_tables", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnumerateTables(MDBX_txn* txn, delegate* unmanaged[Cdecl]<void*, MDBX_txn*, MDBX_val*, uint, uint, int> func, void* ctx);

    /// <summary>
    /// Получает список читателей.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="func">Функция обратного вызова.</param>
    /// <param name="ctx">Контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_reader_list", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxReaderList(MDBX_env* env, delegate* unmanaged[Cdecl]<void*, int, int, uint, uint, ulong, ulong, MDBX_val*, int> func, void* ctx);

    /// <summary>
    /// Проверяет читателей.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dead">Количество мертвых читателей.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_reader_check", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxReaderCheck(MDBX_env* env, int* dead);

    /// <summary>
    /// Регистрирует поток.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_thread_register", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxThreadRegister(MDBX_env* env);

    /// <summary>
    /// Deregisters thread.
    /// </summary>
    /// <param name="env">Environment.</param>
    /// <returns>Error code (0 on success).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_thread_unregister", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxThreadUnregister(MDBX_env* env);

    /// <summary>
    /// Блокирует окружение для транзакции.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="dontWait">Не ждать.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_lock", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnLock(MDBX_env* env, [MarshalAs(UnmanagedType.I1)] bool dontWait);

    /// <summary>
    /// Разблокирует окружение.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_txn_unlock", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxTxnUnlock(MDBX_env* env);

    /// <summary>
    /// Устанавливает функцию паники.
    /// </summary>
    /// <param name="func">Функция паники.</param>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_set_panic", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MdbxSetPanic(delegate* unmanaged[Cdecl]<byte*, byte*, uint, void*, void> func);

    /// <summary>
    /// Оценивает расстояние между курсорами.
    /// </summary>
    /// <param name="first">Первый курсор.</param>
    /// <param name="last">Последний курсор.</param>
    /// <param name="distanceItems">Количество элементов.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_estimate_distance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEstimateDistance(MDBX_cursor* first, MDBX_cursor* last, nint* distanceItems);

    /// <summary>
    /// Оценивает движение курсора.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="moveOp">Операция движения.</param>
    /// <param name="distanceItems">Количество элементов.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_estimate_move", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEstimateMove(MDBX_cursor* cursor, MDBX_val* key, MDBX_val* data, MDBX_cursor_op moveOp, nint* distanceItems);

    /// <summary>
    /// Оценивает диапазон.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="beginKey">Начальный ключ.</param>
    /// <param name="endKey">Конечный ключ.</param>
    /// <param name="distanceItems">Количество элементов.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_estimate_range", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEstimateRange(MDBX_txn* txn, uint dbi, MDBX_val* beginKey, MDBX_val* endKey, nint* distanceItems);

    /// <summary>
    /// Получает/устанавливает последовательность таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="result">Результат.</param>
    /// <param name="increment">Инкремент.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dbi_sequence", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxDbiSequence(MDBX_txn* txn, uint dbi, ulong* result, ulong increment);

    /// <summary>
    /// Форматирует значение в строку.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="buf">Буфер.</param>
    /// <param name="bufsize">Размер буфера.</param>
    /// <returns>Указатель на строку.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_dump_val", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* MdbxDumpVal(MDBX_val* key, byte* buf, nuint bufsize);

    /// <summary>
    /// Получает элемент из таблицы с использованием кэша (многопоточная версия).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="entry">Кэш-запись.</param>
    /// <returns>Результат кэш-операции (MDBX_cache_result_t).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cache_get", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_cache_result_t MdbxCacheGet(MDBX_txn* txn, uint dbi, MDBX_val* key, MDBX_val* data,
        MdbxCacheEntry* entry);

    /// <summary>
    /// Получает элемент из таблицы с использованием кэша (однопоточная версия).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="entry">Кэш-запись.</param>
    /// <returns>Результат кэш-операции (MDBX_cache_result_t).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cache_get_SingleThreaded", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_cache_result_t MdbxCacheGetSingleThreaded(MDBX_txn* txn, uint dbi, MDBX_val* key,
        MDBX_val* data, MdbxCacheEntry* entry);

    /// <summary>
    /// Проверяет целостность базы данных.
    /// </summary>
    /// <param name="env">Окружение.</param>
    /// <param name="cb">Функции обратного вызова для проверки.</param>
    /// <param name="ctx">Контекст проверки.</param>
    /// <param name="flags">Флаги проверки.</param>
    /// <param name="verbosity">Уровень детализации.</param>
    /// <param name="timeoutSeconds16dot16">Таймаут в 1/65536 секунд (0 = без ограничения).</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_chk", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvChk(MDBX_env* env, MdbxChkCallbacks* cb, MdbxChkContext* ctx,
        MdbxChkFlags flags, MdbxChkSeverity verbosity, uint timeoutSeconds16dot16);

    /// <summary>
    /// Учитывает проблемы, обнаруженные приложением во время проверки.
    /// </summary>
    /// <param name="ctx">Контекст проверки.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_env_chk_encount_problem", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxEnvChkEncountProblem(MdbxChkContext* ctx);

    /// <summary>
    /// Проверяет, является ли адрес "грязной" страницы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="ptr">Указатель для проверки.</param>
    /// <returns>
    /// MDBX_RESULT_TRUE (адрес на грязной странице),
    /// MDBX_RESULT_FALSE (адрес НЕ на грязной странице),
    /// или код ошибки.
    /// </returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_is_dirty", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxIsDirty(MDBX_txn* txn, void* ptr);

    /// <summary>
    /// Получает информацию о сборке мусора (GC) и использовании страниц.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="info">Структура для информации о GC.</param>
    /// <param name="bytes">Размер структуры info.</param>
    /// <param name="iterFunc">Функция итерации GC (nullptr = не вызывать).</param>
    /// <param name="iterCtx">Контекст для функции итерации.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_gc_info", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxGcInfo(MDBX_txn* txn, MdbxGcInfo* info, nuint bytes, delegate* unmanaged[Cdecl]<void*, MDBX_txn*, ulong, nuint, nuint, bool, int> iterFunc, void* iterCtx);
}
