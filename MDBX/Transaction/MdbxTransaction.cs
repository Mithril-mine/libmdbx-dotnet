using System.Runtime.InteropServices;
using MDBX.Native.Bindings.Txn;

namespace MDBX;

/// <summary>
/// Высокоуровневая обертка транзакции MDBX.
/// </summary>
public sealed unsafe class MdbxTransaction : IDisposable
{
    private readonly MdbxEnvironment _env;
    private MDBX_txn* _txn;
    private bool _disposed;
    private bool _committed;

    internal MdbxTransaction(MdbxEnvironment env, MDBX_txn* txn)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _txn = txn;
    }

    /// <summary>
    /// Получает нативный указатель на транзакцию.
    /// </summary>
    public MDBX_txn* NativeTxn
    {
        get
        {
            ThrowIfDisposed();
            return _txn;
        }
    }

    /// <summary>
    /// Фиксирует транзакцию.
    /// </summary>
    public void Commit()
    {
        ThrowIfDisposed();
        if (_committed)
            throw new InvalidOperationException("Transaction is already committed.");

        int rc = NativeMdbx.MdbxTxnCommitEx(_txn, null);
        if (rc != 0)
            throw new MdbxException(rc);

        _committed = true;
        _txn = null;
    }

    /// <summary>
    /// Откатывает транзакцию.
    /// </summary>
    public void Rollback()
    {
        ThrowIfDisposed();
        if (_committed)
            throw new InvalidOperationException("Transaction is already committed.");

        int rc = NativeMdbx.MdbxTxnRollback(_txn);
        if (rc != 0)
            throw new MdbxException(rc);

        _txn = null;
    }

    /// <summary>
    /// Освобождает ресурсы транзакции.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Открывает курсор для таблицы в рамках этой транзакции.
    /// </summary>
    /// <param name="db">Таблица (DBI).</param>
    /// <returns>Новый курсор.</returns>
    public MdbxCursor OpenCursor(MdbxDatabase db)
    {
        ThrowIfDisposed();
        if (db == null) throw new ArgumentNullException(nameof(db));
        return MdbxCursor.Create(this, db);
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~MdbxTransaction()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && _txn != null && !_committed)
        {
            try
            {
                NativeMdbx.MdbxTxnRollback(_txn);
            }
            catch
            {
                if (disposing)
                    throw;
            }
            finally
            {
                _txn = null;
            }
        }
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MdbxTransaction));
        if (_txn == null)
            throw new ObjectDisposedException(nameof(MdbxTransaction));
    }
}
