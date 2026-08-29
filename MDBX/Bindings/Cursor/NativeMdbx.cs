using System.Runtime.InteropServices;

namespace MDBX.Native.Bindings.Cursor;

/// <summary>
/// Частичный класс для нативных методов курсоров libmdbx.
/// </summary>
public static unsafe partial class NativeMdbx
{
    /// <summary>
    /// Создает курсор.
    /// </summary>
    /// <param name="context">Пользовательский контекст.</param>
    /// <returns>Указатель на курсор.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_create", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_cursor* MdbxCursorCreate(void* context);

    /// <summary>
    /// Открывает курсор для таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="dbi">DBI.</param>
    /// <param name="cursor">Указатель для курсора.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_open", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorOpen(MDBX_txn* txn, uint dbi, MDBX_cursor** cursor);

    /// <summary>
    /// Закрывает курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_close", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MdbxCursorClose(MDBX_cursor* cursor);

    /// <summary>
    /// Закрывает курсор с возвратом кода ошибки.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_close2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorClose2(MDBX_cursor* cursor);

    /// <summary>
    /// Привязывает курсор к транзакции и таблице.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="cursor">Курсор.</param>
    /// <param name="dbi">DBI.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_bind", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorBind(MDBX_txn* txn, MDBX_cursor* cursor, uint dbi);

    /// <summary>
    /// Отвязывает курсор от таблицы.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_unbind", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorUnbind(MDBX_cursor* cursor);

    /// <summary>
    /// Обновляет курсор для новой транзакции.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_renew", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorRenew(MDBX_txn* txn, MDBX_cursor* cursor);

    /// <summary>
    /// Сбрасывает курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_reset", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorReset(MDBX_cursor* cursor);

    /// <summary>
    /// Копирует курсор.
    /// </summary>
    /// <param name="src">Исходный курсор.</param>
    /// <param name="dest">Курсор назначения.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_copy", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorCopy(MDBX_cursor* src, MDBX_cursor* dest);

    /// <summary>
    /// Сравнивает два курсора.
    /// </summary>
    /// <param name="left">Левый курсор.</param>
    /// <param name="right">Правый курсор.</param>
    /// <param name="ignoreMultival">Игнорировать множественные значения.</param>
    /// <returns>Результат сравнения.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_compare", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorCompare(MDBX_cursor* left, MDBX_cursor* right, [MarshalAs(UnmanagedType.I1)] bool ignoreMultival);

    /// <summary>
    /// Получает элемент через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="op">Операция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_get", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorGet(MDBX_cursor* cursor, MDBX_val* key, MDBX_val* data, MDBX_cursor_op op);

    /// <summary>
    /// Получает пакет элементов через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="count">Количество элементов.</param>
    /// <param name="pairs">Массив пар ключ-значение.</param>
    /// <param name="limit">Лимит.</param>
    /// <param name="op">Операция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_get_batch", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorGetBatch(MDBX_cursor* cursor, nuint* count, MDBX_val* pairs, nuint limit, MDBX_cursor_op op);

    /// <summary>
    /// Получает пользовательский контекст курсора.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Указатель на контекст.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_get_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern void* MdbxCursorGetUserctx(MDBX_cursor* cursor);

    /// <summary>
    /// Устанавливает пользовательский контекст курсора.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="ctx">Контекст.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_set_userctx", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorSetUserctx(MDBX_cursor* cursor, void* ctx);

    /// <summary>
    /// Записывает элемент через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="data">Значение.</param>
    /// <param name="flags">Флаги.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_put", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorPut(MDBX_cursor* cursor, MDBX_val* key, MDBX_val* data, MDBX_put_flags_t flags);

    /// <summary>
    /// Удаляет элемент через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="flags">Флаги.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_del", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorDel(MDBX_cursor* cursor, MDBX_put_flags_t flags);

    /// <summary>
    /// Удаляет диапазон через курсор.
    /// </summary>
    /// <param name="begin">Начальный курсор.</param>
    /// <param name="end">Конечный курсор.</param>
    /// <param name="endIncluding">Включая конечный.</param>
    /// <param name="force">Принудительно.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_delete_range", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorDeleteRange(MDBX_cursor* begin, MDBX_cursor* end, [MarshalAs(UnmanagedType.I1)] bool endIncluding, [MarshalAs(UnmanagedType.I1)] bool force);

    /// <summary>
    /// Получает расстояние между курсорами.
    /// </summary>
    /// <param name="first">Первый курсор.</param>
    /// <param name="last">Последний курсор.</param>
    /// <param name="distance">Расстояние.</param>
    /// <param name="op">Операция.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_distance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorDistance(MDBX_cursor* first, MDBX_cursor* last, nint* distance, MDBX_cursor_op op);

    /// <summary>
    /// Прокручивает курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="amount">Количество шагов.</param>
    /// <param name="deepness">Глубина.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_scroll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorScroll(MDBX_cursor* cursor, nint amount, uint deepness);

    /// <summary>
    /// Распределяет элементы между курсорами.
    /// </summary>
    /// <param name="first">Первый курсор.</param>
    /// <param name="last">Последний курсор.</param>
    /// <param name="array">Массив курсоров.</param>
    /// <param name="count">Количество.</param>
    /// <param name="deepness">Глубина.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_distribute", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorDistribute(MDBX_cursor* first, MDBX_cursor* last, MDBX_cursor** array, uint count, uint deepness);

    /// <summary>
    /// Пакетное удаление через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="action">Действие.</param>
    /// <param name="numberOfAffected">Количество затронутых элементов.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_bunch_delete", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorBunchDelete(MDBX_cursor* cursor, MDBX_bunch_action_t action, ulong* numberOfAffected);

    /// <summary>
    /// Получает количество элементов.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="count">Количество.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_count", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorCount(MDBX_cursor* cursor, nuint* count);

    /// <summary>
    /// Получает количество элементов с статистикой.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="count">Количество.</param>
    /// <param name="stat">Статистика.</param>
    /// <param name="bytes">Размер структуры.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_count_ex", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorCountEx(MDBX_cursor* cursor, nuint* count, MDBX_stat* stat, nuint bytes);

    /// <summary>
    /// Проверяет, достигнут ли конец таблицы.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>1 если конец, 0 иначе.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_eof", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorEof(MDBX_cursor* cursor);

    /// <summary>
    /// Проверяет, находится ли курсор на первом элементе.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>1 если на первом, 0 иначе.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_on_first", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorOnFirst(MDBX_cursor* cursor);

    /// <summary>
    /// Проверяет, находится ли курсор на первом дубликате.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>1 если на первом дубликате, 0 иначе.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_on_first_dup", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorOnFirstDup(MDBX_cursor* cursor);

    /// <summary>
    /// Проверяет, находится ли курсор на последнем элементе.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>1 если на последнем, 0 иначе.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_on_last", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorOnLast(MDBX_cursor* cursor);

    /// <summary>
    /// Проверяет, находится ли курсор на последнем дубликате.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>1 если на последнем дубликате, 0 иначе.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_on_last_dup", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorOnLastDup(MDBX_cursor* cursor);

    /// <summary>
    /// Получает транзакцию курсора.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Указатель на транзакцию.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_txn", CallingConvention = CallingConvention.Cdecl)]
    public static extern MDBX_txn* MdbxCursorTxn(MDBX_cursor* cursor);

    /// <summary>
    /// Получает DBI курсора.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>DBI.</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_dbi", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint MdbxCursorDbi(MDBX_cursor* cursor);

    /// <summary>
    /// Игнорирует дубликаты.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_ignord", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorIgnord(MDBX_cursor* cursor);

    /// <summary>
    /// Сканирует элементы через курсор.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="predicate">Функция предиката.</param>
    /// <param name="context">Контекст.</param>
    /// <param name="op">Операция.</param>
    /// <param name="arg">Аргумент.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_scan", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorScan(MDBX_cursor* cursor, delegate* unmanaged[Cdecl]<void*, MDBX_val*, MDBX_val*, void*, int> predicate, void* context, MDBX_cursor_op op, MDBX_val* arg);

    /// <summary>
    /// Сканирует элементы через курсор начиная с позиции.
    /// </summary>
    /// <param name="cursor">Курсор.</param>
    /// <param name="predicate">Функция предиката.</param>
    /// <param name="context">Контекст.</param>
    /// <param name="op">Операция.</param>
    /// <param name="arg">Аргумент.</param>
    /// <param name="startKey">Начальный ключ.</param>
    /// <returns>Код ошибки (0 при успехе).</returns>
    [DllImport(NativeLibraryLoader.LibraryName, EntryPoint = "mdbx_cursor_scan_from", CallingConvention = CallingConvention.Cdecl)]
    public static extern int MdbxCursorScanFrom(MDBX_cursor* cursor, delegate* unmanaged[Cdecl]<void*, MDBX_val*, MDBX_val*, void*, int> predicate, void* context, MDBX_cursor_op op, MDBX_val* arg, MDBX_val* startKey);
}
