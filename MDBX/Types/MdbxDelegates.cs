namespace MDBX.Native.Types;

/// <summary>
/// Функция паники (обработчик критических ошибок).
/// </summary>
/// <param name="msg">Сообщение об ошибке.</param>
/// <param name="function">Имя функции, в которой произошла ошибка.</param>
/// <param name="line">Номер строки.</param>
/// <param name="obj">Указатель на объект (окружение, транзакция и т.д.).</param>
public unsafe delegate void MdbxPanicFunc(byte* msg, byte* function, uint line, void* obj);

/// <summary>
/// Функция обработки медленных читателей (HSR - Handle Slow Readers).
/// </summary>
/// <param name="env">Указатель на окружение.</param>
/// <param name="txn">Указатель на транзакцию.</param>
/// <param name="pid">ID процесса-читателя.</param>
/// <param name="tid">ID потока-читателя.</param>
/// <param name="laggard">ID отстающей транзакции читателя.</param>
/// <param name="gap">Задержка в чтениях.</param>
/// <param name="space">Оставшееся пространство.</param>
/// <param name="retry">Флаг повторной попытки.</param>
/// <returns>0 в случае успеха, иначе код ошибки.</returns>
public unsafe delegate int MdbxHsrFunc(MDBX_env* env, MDBX_txn* txn, uint pid, uint tid, ulong laggard, uint gap, nuint space, int retry);

/// <summary>
/// Функция уведомления о прогрессе дефрагментации.
/// </summary>
/// <param name="ctx">Пользовательский контекст.</param>
/// <param name="progress">Указатель на структуру с результатами дефрагментации.</param>
/// <returns>0 для продолжения, ненулевое значение для остановки.</returns>
public unsafe delegate int MdbxDefragProgressFunc(void* ctx, MDBX_defrag_result_t* progress);

/// <summary>
/// Функция сравнения ключей.
/// </summary>
/// <param name="a">Первый ключ.</param>
/// <param name="b">Второй ключ.</param>
/// <returns>Отрицательное значение если a &lt; b, 0 если a == b, положительное если a &gt; b.</returns>
public unsafe delegate int MdbxCmpFunc(MdbxVal* a, MdbxVal* b);

/// <summary>
/// Функция перечисления таблиц.
/// </summary>
/// <param name="ctx">Пользовательский контекст.</param>
/// <param name="txn">Транзакция.</param>
/// <param name="name">Имя таблицы.</param>
/// <param name="flags">Флаги таблицы.</param>
/// <param name="id">ID таблицы.</param>
/// <returns>0 для продолжения, ненулевое значение для остановки.</returns>
public unsafe delegate int MdbxTableEnumFunc(void* ctx, MDBX_txn* txn, MdbxVal* name, uint flags, uint id);

/// <summary>
/// Функция предиката для сканирования курсора.
/// </summary>
/// <param name="context">Пользовательский контекст.</param>
/// <param name="key">Ключ.</param>
/// <param name="value">Значение.</param>
/// <param name="arg">Дополнительный аргумент.</param>
/// <returns>0 для продолжения, ненулевое значение для остановки.</returns>
public unsafe delegate int MdbxPredicateFunc(void* context, MdbxVal* key, MdbxVal* value, void* arg);

/// <summary>
/// Функция перечисления читателей.
/// </summary>
/// <param name="ctx">Пользовательский контекст.</param>
/// <param name="num">Номер слота читателя.</param>
/// <param name="slot">Индекс слота.</param>
/// <param name="pid">ID процесса.</param>
/// <param name="thread">ID потока.</param>
/// <param name="txnid">ID транзакции.</param>
/// <param name="since">Время начала транзакции.</param>
/// <param name="range">Диапазон данных.</param>
/// <returns>0 для продолжения, ненулевое значение для остановки.</returns>
public unsafe delegate int MdbxReaderListFunc(void* ctx, int num, int slot, uint pid, uint thread, ulong txnid, ulong since, MdbxVal* range);

/// <summary>
/// Функция отладки (логгирования).
/// </summary>
/// <param name="logLevel">Уровень логирования.</param>
/// <param name="function">Имя функции.</param>
/// <param name="line">Номер строки.</param>
/// <param name="fmt">Форматная строка.</param>
/// <param name="args">Аргументы формата.</param>
public unsafe delegate void MdbxDebugFunc(MdbxLogLevel logLevel, byte* function, int line, byte* fmt, byte* args);

/// <summary>
/// Функция отладки без формата.
/// </summary>
/// <param name="logLevel">Уровень логирования.</param>
/// <param name="function">Имя функции.</param>
/// <param name="line">Номер строки.</param>
/// <param name="msg">Сообщение.</param>
public unsafe delegate void MdbxDebugFuncNofmt(MdbxLogLevel logLevel, byte* function, int line, byte* msg);

/// <summary>
/// Функция итерации GC.
/// </summary>
/// <param name="ctx">Пользовательский контекст.</param>
/// <param name="txn">Транзакция.</param>
/// <param name="spanTxnid">ID транзакции span'а.</param>
/// <param name="spanPgno">Номер начальной страницы span'а.</param>
/// <param name="spanLength">Длина span'а (количество страниц).</param>
/// <param name="spanIsReclaimable">Является ли span возвращаемым (reclaimable).</param>
/// <returns>0 для продолжения, ненулевое значение для остановки.</returns>
public unsafe delegate int MdbxGcIterFunc(void* ctx, MDBX_txn* txn, ulong spanTxnid, nuint spanPgno, nuint spanLength, bool spanIsReclaimable);

/// <summary>
/// Функция сохранения данных для mdbx_replace_ex.
/// </summary>
/// <param name="context">Пользовательский контекст.</param>
/// <param name="target">Целевой буфер.</param>
/// <param name="src">Исходные данные.</param>
/// <param name="bytes">Количество байт.</param>
/// <returns>0 при успехе, иначе код ошибки.</returns>
public unsafe delegate int MdbxPreserveFunc(void* context, MDBX_val* target, void* src, nuint bytes);
