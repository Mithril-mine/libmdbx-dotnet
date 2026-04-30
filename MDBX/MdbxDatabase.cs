using System.Runtime.InteropServices;

namespace MDBX
{
    using Interop;

    /// <summary>
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
    /// </summary>
    public class MdbxDatabase
    {

        private readonly MdbxEnvironment _env;
        private readonly MdbxTransaction _tran;
        private readonly uint _dbi;

        internal MdbxDatabase(MdbxEnvironment env, MdbxTransaction tran, uint dbi)
        {
            _env = env;
            _tran = tran;
            _dbi = dbi;
        }

        /// <summary>
        /// Закрыть дескриптор базы данных. Обычно не требуется.
        /// Закрытие дескриптора базы данных не обязательно, но позволяет mdbx_dbi_open()
        /// повторно использовать значение дескриптора. Обычно лучше установить большее
        /// значение mdbx_env_set_maxdbs(), если только это значение не будет очень большим.
        /// </summary>
        public void Close()
        {
            Dbi.Close(_env._envPtr, _dbi);
        }


        /// <summary>
        /// Удалить эту базу данных.
        /// </summary>
        public void Drop()
        {
            Dbi.Drop(_tran._txnPtr, _dbi, true);
        }

        /// <summary>
        /// Удалить все ключи в этой базе данных, чтобы опустошить её.
        /// </summary>
        public void Empty()
        {
            Dbi.Drop(_tran._txnPtr, _dbi, false);
        }

        /// <summary>
        /// Добавляет или обновляет значение по ключу в базе данных.
        /// </summary>
        /// <param name="key">Ключ для вставки.</param>
        /// <param name="value">Значение для вставки.</param>
        /// <param name="option">Опции для операции Put.</param>
        public void Put(byte[] key, byte[] value, PutOption option = PutOption.None)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);
            IntPtr valuePtr = Marshal.AllocHGlobal(value.Length);

            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);
                Marshal.Copy(value, 0, valuePtr, value.Length);


                DbValue dbKey = new DbValue(keyPtr, key.Length);
                DbValue dbValue = new DbValue(valuePtr, value.Length);
                Dbi.Put(_tran._txnPtr, _dbi, dbKey, dbValue, option);
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
                Marshal.FreeHGlobal(valuePtr);
            }
        }

        /// <summary>
        /// Добавляет или обновляет значение по ключу, используя сериализаторы.
        /// </summary>
        /// <typeparam name="K">Тип ключа.</typeparam>
        /// <typeparam name="V">Тип значения.</typeparam>
        /// <param name="key">Ключ для вставки.</param>
        /// <param name="value">Значение для вставки.</param>
        /// <param name="option">Опции для операции Put.</param>
        public void Put<K, V>(K key, V value, PutOption option = PutOption.None)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();
            Put(keySerializer.Serialize(key), valueSerializer.Serialize(value), option);
        }


        /// <summary>
        /// Получить один ключ.
        /// </summary>
        /// <param name="key">Ключ для поиска.</param>
        /// <returns>null если ключ не найден</returns>
        public byte[] Get(byte[] key)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);

            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);
                DbValue dbKey = new DbValue(keyPtr, key.Length);
                DbValue dbValue = Dbi.Get(_tran._txnPtr, _dbi, dbKey);

                byte[]? buffer = null;
                if (dbValue.Address != IntPtr.Zero && dbValue.Length >= 0)
                {
                    buffer = new byte[dbValue.Length];
                    if (dbValue.Length > 0)
                    {
                        Marshal.Copy(dbValue.Address, buffer, 0, buffer.Length);
                    }
                }

                return buffer ?? [];
            }
            catch (MdbxException ex)
            {
                if (ex.ErrorNumber == MdbxCode.MDBX_NOTFOUND)
                    return [];
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
            }
        }

        /// <summary>
        /// Получить один ключ.
        /// </summary>
        /// <typeparam name="K">Тип ключа.</typeparam>
        /// <typeparam name="V">Тип значения.</typeparam>
        /// <param name="key">Ключ для поиска.</param>
        /// <returns>Значение или default(V) если не найдено.</returns>
        public V Get<K, V>(K key)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();
            byte[]? buffer = Get(keySerializer.Serialize(key));
            if (buffer == null || buffer.Length == 0)
                return default(V)!;
            return valueSerializer.Deserialize(buffer);
        }

        /// <summary>
        /// Получить один ключ.
        /// </summary>
        /// <typeparam name="K">Тип ключа.</typeparam>
        /// <param name="key">Ключ для поиска.</param>
        /// <returns>Массив байтов со значением или пустой массив если не найдено.</returns>
        public byte[] Get<K>(K key)
        {
            return Get<K, byte[]>(key);
        }

        /// <summary>
        /// Удалить конкретный ключ.
        /// </summary>
        /// <param name="key">Ключ для удаления.</param>
        /// <returns>true если удалено успешно; false означает не найдено</returns>
        public bool Del(byte[] key)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);

            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);

                DbValue dbKey = new DbValue(keyPtr, key.Length);
                Dbi.Del(_tran._txnPtr, _dbi, dbKey, IntPtr.Zero);

                return true;
            }
            catch (MdbxException ex)
            {
                if (ex.ErrorNumber == MdbxCode.MDBX_NOTFOUND)
                    return false; // key not found
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
            }
        }

        /// <summary>
        /// Удалить конкретный ключ.
        /// </summary>
        /// <typeparam name="K">Тип ключа.</typeparam>
        /// <param name="key">Ключ для удаления.</param>
        /// <returns>true если удалено успешно; false означает не найдено</returns>
        public bool Del<K>(K key)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            return Del(keySerializer.Serialize(key));
        }


        /// <summary>
        /// Создать дескриптор курсора.
        ///
        /// Курсор ассоциирован с конкретной транзакцией и базой данных.
        /// Курсор не может быть использован, когда его дескриптор базы данных закрыт.
        /// Также, когда его транзакция завершена, кроме как с mdbx_cursor_renew().
        /// Может быть отменён с mdbx_cursor_close().
        ///
        /// Курсор должен быть закрыт явно всегда, до или после завершения его транзакции.
        /// Может быть повторно использован с mdbx_cursor_renew() перед окончательным закрытием.
        /// </summary>
        /// <returns>Новый курсор для этой базы данных.</returns>
        public MdbxCursor OpenCursor()
        {
            IntPtr ptr = Cursor.Open(_tran._txnPtr, _dbi);
            return new MdbxCursor(_env, _tran, this, ptr);
        }

    }
}
