using System.Runtime.InteropServices;
using MDBX.Native.Bindings.Cursor;
using MDBX.Native.Types;

namespace MDBX;

/// <summary>
/// Высокоуровневая обертка курсора libmdbx.
/// </summary>
public sealed unsafe class MdbxCursor : IDisposable
{
    private readonly MdbxDatabase _db;
    private MDBX_cursor* _cursor;
    private MdbxTransaction _txn;
    private bool _disposed;

    internal MdbxCursor(MdbxTransaction txn, MdbxDatabase db, MDBX_cursor* cursor)
    {
        _txn = txn ?? throw new ArgumentNullException(nameof(txn));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _cursor = cursor;
    }

    /// <summary>
    /// Создает новый курсор для указанной транзакции и таблицы.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="db">Таблица (DBI).</param>
    /// <returns>Новый курсор.</returns>
    public static MdbxCursor Create(MdbxTransaction txn, MdbxDatabase db)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (db == null) throw new ArgumentNullException(nameof(db));

        MDBX_cursor* cursor = null;
        int rc = NativeMdbx.MdbxCursorOpen(txn.NativeTxn, db.Dbi, &cursor);
        if (rc != 0)
            throw new MdbxException(rc);

        return new MdbxCursor(txn, db, cursor);
    }

    /// <summary>
    /// Получает нативный указатель на курсор.
    /// </summary>
    public MDBX_cursor* NativeCursor
    {
        get
        {
            ThrowIfDisposed();
            return _cursor;
        }
    }

    /// <summary>
    /// Получает текущий элемент (ключ и значение).
    /// </summary>
    /// <param name="key">Ключ текущего элемента.</param>
    /// <returns>Значение текущего элемента или null если курсор не позиционирован.</returns>
    public byte[]? GetCurrent(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_GET_CURRENT);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на первый элемент.
    /// </summary>
    /// <param name="key">Ключ первого элемента.</param>
    /// <returns>Значение первого элемента или null если таблица пуста.</returns>
    public byte[]? GetFirst(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_FIRST);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на последний элемент.
    /// </summary>
    /// <param name="key">Ключ последнего элемента.</param>
    /// <returns>Значение последнего элемента или null если таблица пуста.</returns>
    public byte[]? GetLast(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_LAST);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на следующий элемент.
    /// </summary>
    /// <param name="key">Ключ следующего элемента.</param>
    /// <returns>Значение следующего элемента или null если достигнут конец таблицы.</returns>
    public byte[]? GetNext(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_NEXT);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на предыдущий элемент.
    /// </summary>
    /// <param name="key">Ключ предыдущего элемента.</param>
    /// <returns>Значение предыдущего элемента или null если достигнуто начало таблицы.</returns>
    public byte[]? GetPrev(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_PREV);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Позиционирует курсор на указанный ключ.
    /// </summary>
    /// <param name="key">Ключ для позиционирования.</param>
    /// <param name="foundKey">Фактический найденный ключ.</param>
    /// <returns>Значение найденного элемента или null если ключ не существует.</returns>
    public byte[]? GetSet(byte[] key, out byte[]? foundKey)
    {
        ThrowIfDisposed();
        if (key == null) throw new ArgumentNullException(nameof(key));
        foundKey = null;

        fixed (byte* keyPtr = key)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)key.Length);
            MDBX_val mdbxVal = new MDBX_val(null, 0);

            int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_SET_KEY);
            if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
                return null;
            if (rc != 0)
                throw new MdbxException(rc);

            foundKey = CopyVal(mdbxKey);
            return CopyVal(mdbxVal);
        }
    }

    /// <summary>
    /// Перемещает курсор на первый дубликат текущего ключа.
    /// </summary>
    /// <param name="key">Ключ элемента.</param>
    /// <returns>Значение первого дубликата или null если дубликатов нет.</returns>
    public byte[]? GetFirstDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_FIRST_DUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на последний дубликат текущего ключа.
    /// </summary>
    /// <param name="key">Ключ элемента.</param>
    /// <returns>Значение последнего дубликата или null если дубликатов нет.</returns>
    public byte[]? GetLastDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_LAST_DUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на следующий дубликат.
    /// </summary>
    /// <param name="key">Ключ элемента.</param>
    /// <returns>Значение следующего дубликата или null если достигнут конец дубликатов.</returns>
    public byte[]? GetNextDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_NEXT_DUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на предыдущий дубликат.
    /// </summary>
    /// <param name="key">Ключ элемента.</param>
    /// <returns>Значение предыдущего дубликата или null если достигнуто начало дубликатов.</returns>
    public byte[]? GetPrevDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_PREV_DUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на следующий элемент с другим ключом.
    /// </summary>
    /// <param name="key">Ключ следующего элемента.</param>
    /// <returns>Значение следующего элемента с другим ключом или null если достигнут конец таблицы.</returns>
    public byte[]? GetNextNoDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_NEXT_NODUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Перемещает курсор на предыдущий элемент с другим ключом.
    /// </summary>
    /// <param name="key">Ключ предыдущего элемента.</param>
    /// <returns>Значение предыдущего элемента с другим ключом или null если достигнуто начало таблицы.</returns>
    public byte[]? GetPrevNoDup(out byte[]? key)
    {
        ThrowIfDisposed();
        key = null;

        MDBX_val mdbxKey = new MDBX_val(null, 0);
        MDBX_val mdbxVal = new MDBX_val(null, 0);

        int rc = NativeMdbx.MdbxCursorGet(_cursor, &mdbxKey, &mdbxVal, MDBX_cursor_op.MDBX_PREV_NODUP);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return null;
        if (rc != 0)
            throw new MdbxException(rc);

        key = CopyVal(mdbxKey);
        return CopyVal(mdbxVal);
    }

    /// <summary>
    /// Записывает пару ключ-значение через курсор.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="flags">Флаги операции записи.</param>
    public void Put(byte[] key, byte[] value, MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        ThrowIfDisposed();
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));

        fixed (byte* keyPtr = key)
        fixed (byte* valPtr = value)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)key.Length);
            MDBX_val mdbxVal = new MDBX_val(valPtr, (nuint)value.Length);
            int rc = NativeMdbx.MdbxCursorPut(_cursor, &mdbxKey, &mdbxVal, (MDBX_put_flags_t)flags);
            if (rc != 0)
                throw new MdbxException(rc);
        }
    }

    /// <summary>
    /// Записывает строковую пару ключ-значение через курсор (UTF-8).
    /// </summary>
    /// <param name="key">Строковый ключ.</param>
    /// <param name="value">Строковое значение.</param>
    /// <param name="flags">Флаги операции записи.</param>
    public void Put(string key, string value, MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
        Put(keyBytes, valueBytes, flags);
    }

    /// <summary>
    /// Удаляет текущий элемент через курсор.
    /// </summary>
    /// <param name="flags">Флаги удаления.</param>
    /// <returns>True если элемент был удален, false если элемент не найден.</returns>
    public bool Delete(MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        ThrowIfDisposed();
        int rc = NativeMdbx.MdbxCursorDel(_cursor, (MDBX_put_flags_t)flags);
        if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
            return false;
        if (rc != 0)
            throw new MdbxException(rc);
        return true;
    }

    /// <summary>
    /// Получает количество элементов, доступных с текущей позиции курсора.
    /// </summary>
    /// <returns>Количество элементов.</returns>
    public nuint Count()
    {
        ThrowIfDisposed();
        nuint count = 0;
        int rc = NativeMdbx.MdbxCursorCount(_cursor, &count);
        if (rc != 0)
            throw new MdbxException(rc);
        return count;
    }

    /// <summary>
    /// Проверяет, достигнут ли конец таблицы.
    /// </summary>
    /// <returns>True если курсор за пределами таблицы.</returns>
    public bool IsEof()
    {
        ThrowIfDisposed();
        return NativeMdbx.MdbxCursorEof(_cursor) != 0;
    }

    /// <summary>
    /// Проверяет, находится ли курсор на первом элементе.
    /// </summary>
    /// <returns>True если курсор на первом элементе.</returns>
    public bool IsOnFirst()
    {
        ThrowIfDisposed();
        return NativeMdbx.MdbxCursorOnFirst(_cursor) != 0;
    }

    /// <summary>
    /// Проверяет, находится ли курсор на последнем элементе.
    /// </summary>
    /// <returns>True если курсор на последнем элементе.</returns>
    public bool IsOnLast()
    {
        ThrowIfDisposed();
        return NativeMdbx.MdbxCursorOnLast(_cursor) != 0;
    }

    /// <summary>
    /// Проверяет, находится ли курсор на первом дубликате.
    /// </summary>
    /// <returns>True если курсор на первом дубликате.</returns>
    public bool IsOnFirstDup()
    {
        ThrowIfDisposed();
        return NativeMdbx.MdbxCursorOnFirstDup(_cursor) != 0;
    }

    /// <summary>
    /// Проверяет, находится ли курсор на последнем дубликате.
    /// </summary>
    /// <returns>True если курсор на последнем дубликате.</returns>
    public bool IsOnLastDup()
    {
        ThrowIfDisposed();
        return NativeMdbx.MdbxCursorOnLastDup(_cursor) != 0;
    }

    /// <summary>
    /// Обновляет курсор для использования в новой транзакции.
    /// </summary>
    /// <param name="txn">Новая транзакция.</param>
    public void Renew(MdbxTransaction txn)
    {
        ThrowIfDisposed();
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        int rc = NativeMdbx.MdbxCursorRenew(txn.NativeTxn, _cursor);
        if (rc != 0)
            throw new MdbxException(rc);
        _txn = txn;
    }

    /// <summary>
    /// Сбрасывает курсор (отвязывает от таблицы).
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();
        int rc = NativeMdbx.MdbxCursorReset(_cursor);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Копирует состояние курсора в другой курсор.
    /// </summary>
    /// <param name="dest">Курсор назначения.</param>
    public void CopyTo(MdbxCursor dest)
    {
        ThrowIfDisposed();
        if (dest == null) throw new ArgumentNullException(nameof(dest));
        int rc = NativeMdbx.MdbxCursorCopy(_cursor, dest._cursor);
        if (rc != 0)
            throw new MdbxException(rc);
    }

    /// <summary>
    /// Закрывает курсор.
    /// </summary>
    public void Close()
    {
        if (!_disposed && _cursor != null)
        {
            NativeMdbx.MdbxCursorClose(_cursor);
            _cursor = null;
        }
    }

    /// <summary>
    /// Освобождает ресурсы курсора.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~MdbxCursor()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && _cursor != null)
        {
            try
            {
                NativeMdbx.MdbxCursorClose(_cursor);
            }
            catch
            {
                if (disposing)
                    throw;
            }
            finally
            {
                _cursor = null;
            }
        }
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MdbxCursor));
        if (_cursor == null)
            throw new ObjectDisposedException(nameof(MdbxCursor));
    }

    private static byte[]? CopyVal(MDBX_val val)
    {
        if (val.IovBase == null || val.IovLen == 0)
            return null;
        byte[] result = new byte[val.IovLen];
        Buffer.MemoryCopy(val.IovBase, result.AsMemory().Pin().Pointer, val.IovLen, val.IovLen);
        return result;
    }
}
