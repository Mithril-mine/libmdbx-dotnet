using MDBX.Options;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    /// <summary>
    /// Взаимодействие с нативными функциями MDBX для операций с курсором.
    /// </summary>
    internal static class Cursor
    {
        /// <summary>
        /// Делегат для функции mdbx_cursor_open.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <param name="dbi">Дескриптор базы данных.</param>
        /// <param name="cursor">Возвращаемый указатель на курсор.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenDelegate(IntPtr txn, uint dbi, out IntPtr cursor);

        private static OpenDelegate? _openDelegate = null;

        /// <summary>
        /// Открывает новый курсор для заданной базы данных в транзакции.
        /// </summary>
        /// <param name="txn">Указатель на транзакцию.</param>
        /// <param name="dbi">Дескриптор базы данных.</param>
        /// <returns>Указатель на созданный курсор.</returns>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        /// <exception cref="MdbxException">Ошибка при открытии курсора.</exception>
        internal static IntPtr Open(IntPtr txn, uint dbi)
        {
            if (_openDelegate is null)
                throw new InvalidOperationException("Cursor.Open called before Library.Load()");
            IntPtr ptr;
            int err = _openDelegate(txn, dbi, out ptr);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_open", err);
            return ptr;
        }

        /// <summary>
        /// Делегат для функции mdbx_cursor_close.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate(IntPtr cursor);

        private static CloseDelegate? _closeDelegate = null;

        /// <summary>
        /// Закрывает курсор и освобождает его ресурсы.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        internal static void Close(IntPtr cursor)
        {
            if (_closeDelegate is null)
                throw new InvalidOperationException("Cursor.Close called before Library.Load()");
            _closeDelegate(cursor);
        }

        /// <summary>
        /// Делегат для функции mdbx_cursor_get.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="key">Ключ (вход/выход).</param>
        /// <param name="value">Значение (вход/выход).</param>
        /// <param name="op">Операция позиционирования.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetDelegate(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorOption op);

        private static GetDelegate? _getDelegate = null;

        /// <summary>
        /// Получает элемент данных через курсор.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="key">Структура ключа (входное значение инициализирует позицию, выходное - получает ключ).</param>
        /// <param name="value">Структура значения (получает данные).</param>
        /// <param name="op">Операция курсора (First, Next, Prev и т.д.).</param>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        /// <exception cref="MdbxException">Ошибка при операции get.</exception>
        internal static void Get(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorOption op)
        {
            if (_getDelegate is null)
                throw new InvalidOperationException("Cursor.Get called before Library.Load()");
            int err = _getDelegate(cursor, ref key, ref value, op);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_get", err);
        }

        /// <summary>
        /// Делегат для функции mdbx_cursor_put.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="option">Опции операции.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PutDelegate(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorPutOption option);

        private static PutDelegate? _putDelegate = null;

        /// <summary>
        /// Записывает пару ключ/значение через курсор.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="option">Опции операции.</param>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        /// <exception cref="MdbxException">Ошибка при операции put.</exception>
        internal static void Put(IntPtr cursor, ref DataBaseValue key, ref DataBaseValue value, CursorPutOption option)
        {
            if (_putDelegate is null)
                throw new InvalidOperationException("Cursor.Put called before Library.Load()");
            int err = _putDelegate(cursor, ref key, ref value, option);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_put", err);
        }

        /// <summary>
        /// Делегат для функции mdbx_cursor_del.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="option">Опции удаления.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DelDelegate(IntPtr cursor, CursorDelOption option);

        private static DelDelegate? _delDelegate = null;

        /// <summary>
        /// Удаляет текущий элемент через курсор.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="option">Опции удаления.</param>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        /// <exception cref="MdbxException">Ошибка при операции удаления.</exception>
        internal static void Del(IntPtr cursor, CursorDelOption option)
        {
            if (_delDelegate is null)
                throw new InvalidOperationException("Cursor.Del called before Library.Load()");
            int err = _delDelegate(cursor, option);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_del", err);
        }

        /// <summary>
        /// Делегат для функции mdbx_cursor_count.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <param name="count">Возвращаемое количество.</param>
        /// <returns>Код ошибки или 0 при успехе.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CountDelegate(IntPtr cursor, ref IntPtr count);

        private static CountDelegate? _countDelegate = null;

        /// <summary>
        /// Возвращает количество дубликатов для текущего ключа.
        /// </summary>
        /// <param name="cursor">Указатель на курсор.</param>
        /// <returns>Количество дубликатов.</returns>
        /// <exception cref="InvalidOperationException">Вызывается до загрузки библиотеки.</exception>
        /// <exception cref="MdbxException">Ошибка при подсчёте.</exception>
        internal static int Count(IntPtr cursor)
        {
            if (_countDelegate is null)
                throw new InvalidOperationException("Cursor.Count called before Library.Load()");
            IntPtr count = IntPtr.Zero;
            int err = _countDelegate(cursor, ref count);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_count", err);
            return count.ToInt32();
        }

        /// <summary>
        /// Привязывает все делегаты курсора к нативным функциям библиотеки MDBX.
        /// </summary>
        internal static void Bind()
        {
            _closeDelegate = NativeLibraryLoader.GetProcAddress<CloseDelegate>("mdbx_cursor_close");
            _openDelegate = NativeLibraryLoader.GetProcAddress<OpenDelegate>("mdbx_cursor_open");
            _getDelegate = NativeLibraryLoader.GetProcAddress<GetDelegate>("mdbx_cursor_get");
            _putDelegate = NativeLibraryLoader.GetProcAddress<PutDelegate>("mdbx_cursor_put");
            _delDelegate = NativeLibraryLoader.GetProcAddress<DelDelegate>("mdbx_cursor_del");
            _countDelegate = NativeLibraryLoader.GetProcAddress<CountDelegate>("mdbx_cursor_count");
        }
    }
}
