using System.Linq;
using System.Runtime.InteropServices;
using MDBX.Native.Bindings.Crud;
using MDBX.Native.Types;

namespace MDBX;

/// <summary>
/// Высокоуровневая обертка таблицы (DBI) MDBX.
/// </summary>
public sealed unsafe class MdbxDatabase : IDisposable
{
    private readonly MdbxEnvironment _env;
    private readonly uint _dbi;
    private bool _disposed;

    internal MdbxDatabase(MdbxEnvironment env, uint dbi)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _dbi = dbi;
    }

    /// <summary>
    /// Получает идентификатор таблицы (DBI).
    /// </summary>
    public uint Dbi => _dbi;

    /// <summary>
    /// Получает значение по ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Ключ.</param>
    /// <returns>Значение или null если ключ не найден.</returns>
    public byte[]? Get(MdbxTransaction txn, byte[] key)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (key == null) throw new ArgumentNullException(nameof(key));
        ThrowIfDisposed();

        fixed (byte* keyPtr = key)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)key.Length);
            MDBX_val mdbxVal = new MDBX_val(null, 0);
            int rc = NativeMdbx.MdbxGet(txn.NativeTxn, _dbi, &mdbxKey, &mdbxVal);
            if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
                return null;
            if (rc != 0)
                throw new MdbxException(rc);

            byte[] result = new byte[mdbxVal.IovLen];
            Buffer.MemoryCopy(mdbxVal.IovBase, result.AsMemory().Pin().Pointer, mdbxVal.IovLen, mdbxVal.IovLen);
            return result;
        }
    }

    /// <summary>
    /// Получает значение по строковому ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Строковый ключ (UTF-8).</param>
    /// <returns>Значение или null если ключ не найден.</returns>
    public byte[]? Get(MdbxTransaction txn, string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        return Get(txn, keyBytes);
    }

    /// <summary>
    /// Записывает значение по ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="flags">Флаги записи.</param>
    public void Put(MdbxTransaction txn, byte[] key, byte[] value, MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));
        ThrowIfDisposed();

        fixed (byte* keyPtr = key)
        fixed (byte* valPtr = value)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)key.Length);
            MDBX_val mdbxVal = new MDBX_val(valPtr, (nuint)value.Length);
            int rc = NativeMdbx.MdbxPut(txn.NativeTxn, _dbi, &mdbxKey, &mdbxVal, (MDBX_put_flags_t)flags);
            if (rc != 0)
                throw new MdbxException(rc);
        }
    }

    /// <summary>
    /// Записывает значение по строковому ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Строковый ключ (UTF-8).</param>
    /// <param name="value">Значение.</param>
    /// <param name="flags">Флаги записи.</param>
    public void Put(MdbxTransaction txn, string key, byte[] value, MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));
        ThrowIfDisposed();

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = value)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val mdbxVal = new MDBX_val(valPtr, (nuint)value.Length);
            int rc = NativeMdbx.MdbxPut(txn.NativeTxn, _dbi, &mdbxKey, &mdbxVal, (MDBX_put_flags_t)flags);
            if (rc != 0)
                throw new MdbxException(rc);
        }
    }

    /// <summary>
    /// Записывает строковое значение по строковому ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Строковый ключ (UTF-8).</param>
    /// <param name="value">Строковое значение (UTF-8).</param>
    /// <param name="flags">Флаги записи.</param>
    public void Put(MdbxTransaction txn, string key, string value, MdbxPutFlags flags = MdbxPutFlags.MDBX_UPSERT)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
        Put(txn, keyBytes, valueBytes, flags);
    }

    /// <summary>
    /// Удаляет значение по ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Ключ.</param>
    /// <returns>True если элемент был удален, false если не найден.</returns>
    public bool Delete(MdbxTransaction txn, byte[] key)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (key == null) throw new ArgumentNullException(nameof(key));
        ThrowIfDisposed();

        fixed (byte* keyPtr = key)
        {
            MDBX_val mdbxKey = new MDBX_val(keyPtr, (nuint)key.Length);
            int rc = NativeMdbx.MdbxDel(txn.NativeTxn, _dbi, &mdbxKey, null);
            if (rc == MdbxErrorCodes.MDBX_NOTFOUND)
                return false;
            if (rc != 0)
                throw new MdbxException(rc);
            return true;
        }
    }

    /// <summary>
    /// Удаляет значение по строковому ключу.
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="key">Строковый ключ (UTF-8).</param>
    /// <returns>True если элемент был удален, false если не найден.</returns>
    public bool Delete(MdbxTransaction txn, string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        return Delete(txn, keyBytes);
    }

    /// <summary>
    /// Возвращает отложенный (lazy) запрос поверх курсора, перебирающий все записи таблицы.
    /// Каждый вызов <see cref="MdbxQuery.GetEnumerator"/> открывает новый курсор, а перебор
    /// выполняется лениво и совместим с LINQ (Where, Select, First, ToList и т.д.).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Запрос на основе курсора.</returns>
    public MdbxQuery Find(MdbxTransaction txn)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        ThrowIfDisposed();
        return new MdbxQuery(txn, this);
    }

    /// <summary>
    /// Возвращает отложенный перечислитель "сырых" значений таблицы (без ключей).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <returns>Последовательность значений.</returns>
    public IEnumerable<byte[]> FindValues(MdbxTransaction txn)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        ThrowIfDisposed();
        return Find(txn).Select(e => e.Value);
    }

    /// <summary>
    /// Возвращает отложенный перечислитель значений таблицы, преобразованных в <typeparamref name="TValue"/>.
    /// Позволяет писать LINQ-запросы, где параметр предиката — это значение:
    /// <c>db.Find(txn, Encoding.UTF8.GetString).Where(x => x == "Some Value")</c>.
    /// </summary>
    /// <typeparam name="TValue">Целевой тип значения.</typeparam>
    /// <param name="txn">Транзакция.</param>
    /// <param name="valueSelector">Преобразование "сырых" байтов значения в TValue.</param>
    /// <returns>Последовательность преобразованных значений.</returns>
    public IEnumerable<TValue> Find<TValue>(MdbxTransaction txn, Func<byte[], TValue> valueSelector)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
        ThrowIfDisposed();
        return Find(txn).Select(e => valueSelector(e.Value));
    }

    /// <summary>
    /// Возвращает отложенный перечислитель строковых значений таблицы (декодируются как UTF-8).
    /// </summary>
    /// <param name="txn">Транзакция.</param>
    /// <param name="encoding">Кодировка для декодирования значений (по умолчанию UTF-8).</param>
    /// <returns>Последовательность строковых значений.</returns>
    public IEnumerable<string> Find(MdbxTransaction txn, System.Text.Encoding? encoding = null)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        ThrowIfDisposed();
        var enc = encoding ?? System.Text.Encoding.UTF8;
        return Find(txn).Select(e => enc.GetString(e.Value));
    }

    /// <summary>
    /// Возвращает отложенный перечислитель пар ключ-значение, преобразованных в заданные типы.
    /// </summary>
    /// <typeparam name="TKey">Целевой тип ключа.</typeparam>
    /// <typeparam name="TValue">Целевой тип значения.</typeparam>
    /// <param name="txn">Транзакция.</param>
    /// <param name="keySelector">Преобразование "сырых" байтов ключа в TKey.</param>
    /// <param name="valueSelector">Преобразование "сырых" байтов значения в TValue.</param>
    /// <returns>Последовательность пар ключ-значение.</returns>
    public IEnumerable<KeyValuePair<TKey, TValue>> Find<TKey, TValue>(
        MdbxTransaction txn,
        Func<byte[], TKey> keySelector,
        Func<byte[], TValue> valueSelector)
    {
        if (txn == null) throw new ArgumentNullException(nameof(txn));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
        ThrowIfDisposed();
        return Find(txn).Select(e => new KeyValuePair<TKey, TValue>(keySelector(e.Key), valueSelector(e.Value)));
    }

    /// <summary>
    /// Освобождает ресурсы таблицы.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~MdbxDatabase()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _env.CloseDatabase(_dbi);
            }
            _disposed = true;
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MdbxDatabase));
    }
}
