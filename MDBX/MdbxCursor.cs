using MDBX.Interop;
using MDBX.Interop.Models;
using MDBX.Options;
using MDBX.Serializers;
using System.Runtime.InteropServices;

namespace MDBX;
/// <summary>
/// Представляет курсор для навигации по данным в базе данных MDBX.
/// </summary>
public class MdbxCursor : IDisposable
{
    private bool closed = false;

    /// <summary>
    /// Освобождает ресурсы, используемые курсором.
    /// </summary>
    public void Dispose()
    {
        Close();
    }

    private readonly MdbxEnvironment _env;

    private readonly MdbxTransaction _tran;

    private readonly MdbxDatabase _db;

    private readonly IntPtr _cursorPtr;

    internal MdbxCursor(MdbxEnvironment env, MdbxTransaction tran, MdbxDatabase db, IntPtr cursorPtr)
    {
        _env = env;
        _tran = tran;
        _db = db;
        _cursorPtr = cursorPtr;
    }

    /// <summary>
    /// Закрыть дескриптор курсора.
    ///
    /// Дескриптор курсора будет освобождён и не должен использоваться после этого вызова.
    /// Его транзакция всё ещё должна быть жива, если это пишущая транзакция.
    /// </summary>
    private void Close()
    {
        if (!closed)
        {
            closed = true;
            MdbxInteropCursor.Close(_cursorPtr);
        }
    }


    /// <summary>
    /// Получить элементы из базы данных.
    /// </summary>
    /// <param name="key">Буфер для ключа.</param>
    /// <param name="value">Буфер для значения.</param>
    /// <param name="op">Операция курсора.</param>
    /// <returns>true если элемент найден, иначе false.</returns>
    public bool Get(ref byte[] key, ref byte[] value, CursorOption op)
    {
        IntPtr keyPtr = IntPtr.Zero;
        IntPtr valuePtr = IntPtr.Zero;
        if (key != null)
            keyPtr = Marshal.AllocHGlobal(key.Length);
        if (value != null)
            valuePtr = Marshal.AllocHGlobal(value.Length);

        try
        {
            if (key != null && key.Length > 0)
                Marshal.Copy(key, 0, keyPtr, key.Length);
            if (value != null && value.Length > 0)
                Marshal.Copy(value, 0, valuePtr, value.Length);

            DataBaseValue dbKey = new DataBaseValue(keyPtr, key == null ? 0 : key.Length);
            DataBaseValue dbValue = new DataBaseValue(valuePtr, value == null ? 0 : value.Length);

            MdbxInteropCursor.Get(_cursorPtr, ref dbKey, ref dbValue, op);

            if (dbKey.Address != IntPtr.Zero)
            {
                if (key == null || key.Length != dbKey.Length)
                    key = new byte[dbKey.Length];
                Marshal.Copy(dbKey.Address, key, 0, key.Length);
            }
            else
            {
                key = null!;
            }
            if (dbValue.Address != IntPtr.Zero)
            {
                if (value == null || value.Length != dbValue.Length)
                    value = new byte[dbValue.Length];
                Marshal.Copy(dbValue.Address, value, 0, value.Length);
            }
            else
            {
                value = null!;
            }
        }
        catch (MdbxException ex)
        {
            if (ex.ErrorNumber == Constants.MDBX_NOTFOUND)
                return false;
            throw;
        }
        finally
        {
            if (keyPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(keyPtr);
            if (valuePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(valuePtr);
        }
        return true;
    }

    /// <summary>
    /// Получить элементы из базы данных.
    /// </summary>
    /// <typeparam name="K">Тип ключа.</typeparam>
    /// <typeparam name="V">Тип значения.</typeparam>
    /// <param name="key">Переменная для ключа.</param>
    /// <param name="value">Переменная для значения.</param>
    /// <param name="op">Операция курсора.</param>
    /// <returns>false если не найдено.</returns>
    public bool Get<K, V>(ref K key, ref V value, CursorOption op)
    {
        ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
        ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();

        byte[] keyBytes = keySerializer.Serialize(key);
        byte[] valueBytes = valueSerializer.Serialize(value);

        bool found = Get(ref keyBytes, ref valueBytes, op);
        if (found)
        {
            if (keyBytes != null)
                key = keySerializer.Deserialize(keyBytes);
            else
                key = default!;

            if (valueBytes != null)
                value = valueSerializer.Deserialize(valueBytes);
            else
                value = default!;
        }
        return found;
    }

    /// <summary>
    /// Записать через курсор.
    /// Эта функция сохраняет пары ключ/значение в базу данных. Курсор
    /// позиционируется на новый элемент, или при ошибке обычно рядом с ним.
    /// </summary>
    /// <param name="key">Ключ для записи.</param>
    /// <param name="value">Значение для записи.</param>
    /// <param name="option">Опции операции.</param>
    public void Put(byte[] key, byte[] value, CursorPutOption option)
    {
        IntPtr keyPtr = IntPtr.Zero;
        IntPtr valuePtr = IntPtr.Zero;
        if (key != null)
            keyPtr = Marshal.AllocHGlobal(key.Length);
        if (value != null)
            valuePtr = Marshal.AllocHGlobal(value.Length);

        try
        {
            if (key != null && key.Length > 0)
                Marshal.Copy(key, 0, keyPtr, key.Length);
            if (value != null && value.Length > 0)
                Marshal.Copy(value, 0, valuePtr, value.Length);

            DataBaseValue dbKey = new DataBaseValue(keyPtr, key == null ? 0 : key.Length);
            DataBaseValue dbValue = new DataBaseValue(valuePtr, value == null ? 0 : value.Length);

            MdbxInteropCursor.Put(_cursorPtr, ref dbKey, ref dbValue, option);
        }
        finally
        {
            if (keyPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(keyPtr);
            if (valuePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(valuePtr);
        }
    }

    /// <summary>
    /// Записать через курсор.
    /// Эта функция сохраняет пары ключ/значение в базу данных. Курсор
    /// позиционируется на новый элемент, или при ошибке обычно рядом с ним.
    /// </summary>
    /// <typeparam name="K">Тип ключа.</typeparam>
    /// <typeparam name="V">Тип значения.</typeparam>
    /// <param name="key">Ключ для записи.</param>
    /// <param name="value">Значение для записи.</param>
    /// <param name="option">Опции операции.</param>
    public void Put<K, V>(K key, V value, CursorPutOption option = CursorPutOption.Unspecific)
    {
        ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
        ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();

        byte[] keyBytes = keySerializer.Serialize(key);
        byte[] valueBytes = valueSerializer.Serialize(value);

        Put(keyBytes, valueBytes, option);
    }

    /// <summary>
    /// Удалить текущую пару ключ/данные.
    ///
    /// Эта функция удаляет пару ключ/данные, на которую ссылается курсор.
    /// Это не делает курсор невалидным, поэтому операции такие как MDBX_NEXT
    /// всё ещё могут быть использованы на нем. И MDBX_NEXT и MDBX_GET_CURRENT
    /// вернут ту же запись после этой операции.
    /// </summary>
    /// <param name="option">Опции удаления.</param>
    public void Del(CursorDelOption option = CursorDelOption.Unspecific)
    {
        MdbxInteropCursor.Del(_cursorPtr, option);
    }


    /// <summary>
    /// Возвращает количество дубликатов для текущего ключа.
    ///
    /// Этот вызов допустим только для баз данных, поддерживающих отсортированные
    /// дублирующиеся элементы данных MDBX_DUPSORT.
    /// </summary>
    /// <returns>Количество дубликатов.</returns>
    public int Count()
    {
        return MdbxInteropCursor.Count(_cursorPtr);
    }

    /// <summary>
    /// Финализатор.
    /// </summary>
    ~MdbxCursor()
    {
        Close();
    }
}
