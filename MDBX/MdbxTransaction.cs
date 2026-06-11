namespace MDBX;

using Interop;
using MDBX.Options;

/// <summary>
/// Представляет транзакцию в базе данных MDBX.
/// </summary>
public class MdbxTransaction : IDisposable
{
    #region IDisposable Support
    /// <summary>
    /// Освобождает транзакцию путем отката.
    /// </summary>
    public void Dispose()
    {
        Abort();
    }
    #endregion


    private bool _released = false;

    private readonly MdbxEnvironment _env;
    internal readonly IntPtr _txnPtr;

    internal MdbxTransaction(MdbxEnvironment env, IntPtr txnPtr)
    {
        _env = env;
        _txnPtr = txnPtr;
    }

    private readonly Lock _commitLock = new();

    /// <summary>
    /// Фиксирует все операции транзакции в базе данных.
    ///
    /// Дескриптор транзакции освобождается. Он и его курсоры больше не должны
    /// использоваться после этого вызова, кроме как с mdbx_cursor_renew().
    /// </summary>
    public void Commit()
    {
        lock (_commitLock)
        {
            if (!_released)
            {
                _released = true;

                MdbxInteropTransaction.Commit(_txnPtr);
            }
            else
            {
                throw new InvalidOperationException("MDBX transaction handle was freed. It can't be used again unless Reset() is called");
            }
        }
    }

    /// <summary>
    /// Отменить все операции транзакции вместо их сохранения.
    /// </summary>
    public void Abort()
    {
        if (!_released)
        {
            _released = true;
            MdbxInteropTransaction.Abort(_txnPtr);
        }
    }

    /// <summary>
    /// Сбросить транзакцию только для чтения.
    ///
    /// Прервать транзакцию как Abort(), но сохранить дескриптор транзакции.
    /// Поэтому Renew() может повторно использовать дескриптор. Это экономит
    /// накладные расходы на выделение памяти, если процесс скоро начнёт новую
    /// транзакцию только для чтения, а также накладные расходы на блокировку,
    /// если используется MDBX_NOTLS. Блокировка таблицы читателей освобождается,
    /// но слот таблицы остаётся привязанным к他的 потоку или MDBX_txn.
    /// Используйте mdbx_txn_abort() для отмены сброшенного дескриптора и
    /// освобождения слота таблицы блокировок, если используется MDBX_NOTLS.
    /// </summary>
    public void Reset()
    {
        MdbxInteropTransaction.Reset(_txnPtr);
    }

    /// <summary>
    /// Обновить транзакцию только для чтения.
    ///
    /// Это приобретает новую блокировку читателя для дескриптора транзакции,
    /// который был освобождён mdbx_txn_reset(). Должно быть вызвано перед тем,
    /// как сброшенная транзакция может быть использована снова.
    /// </summary>
    public void Renew()
    {
        MdbxInteropTransaction.Renew(_txnPtr);
    }

    /// <summary>
    /// Открыть таблицу в среде.
    ///
    /// Дескриптор таблицы обозначает имя и параметры таблицы, независимо от
    /// того, существует ли такая таблица. Дескриптор таблицы может быть отменён
    /// вызовом mdbx_dbi_close(). Старый дескриптор таблицы возвращается, если таблица
    /// уже была открыта. Дескриптор может быть закрыт только один раз.
    ///
    /// Дескриптор таблицы будет приватным для текущей транзакции до тех пор,
    /// пока транзакция не будет успешно зафиксирована. Если транзакция
    /// прервана, дескриптор будет закрыт автоматически.
    /// После успешной фиксации дескриптор будет находиться в общей
    /// среде и может использоваться другими транзакциями.
    ///
    /// Эта функция не должна вызываться из нескольких конкурентных
    /// транзакций в одном процессе. Транзакция, использующая эту функцию,
    /// должна завершиться (либо фиксацией, либо прерыванием) до того,
    /// как любая другая транзакция в процессе может использовать эту функцию.
    /// </summary>
    /// <param name="name">Имя таблицы (может быть null для безымянной БД).</param>
    /// <param name="option">Опции открытия базы данных.</param>
    /// <returns>Дескриптор базы данных.</returns>
    public MdbxDatabase OpenDatabase(string? name = null, DatabaseOption option = DatabaseOption.Unspecific)
    {
        return new MdbxDatabase(_env, this, MdbxInteropDataBase.Open(_txnPtr, name ?? string.Empty, option));
    }

    /// <summary>
    /// Возвращает идентификатор, связанный с этой транзакцией.
    /// Для транзакции только для чтения это соответствует снимку, который читается;
    /// concurrent readers часто будут иметь одинаковый идентификатор транзакции.
    /// </summary>
    /// <returns>Идентификатор транзакции.</returns>
    public ulong GetID()
    {
        return MdbxInteropTransaction.GetID(_txnPtr);
    }
}
