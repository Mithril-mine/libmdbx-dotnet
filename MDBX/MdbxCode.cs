namespace MDBX
{

    /// <summary>
    /// Definitions from mdbx.h
    /// </summary>
    public static class MdbxCode
    {
        /// <summary>
        /// Success.
        /// </summary>
        public const int MDBX_SUCCESS = 0;
        /// <summary>
        /// False result.
        /// </summary>
        public const int MDBX_RESULT_FALSE = 0;

        /// <summary>
        /// True result.
        /// </summary>
        public const int MDBX_RESULT_TRUE = -1;

        /* key/data pair already exists */
        /// <summary>
        /// Ключ/значение пара уже существует.
        /// </summary>
        public const int MDBX_KEYEXIST = -30799;

        /* key/data pair not found (EOF) */
        /// <summary>
        /// Ключ/значение пара не найдена (EOF).
        /// </summary>
        public const int MDBX_NOTFOUND = -30798;

        /* Requested page not found - this usually indicates corruption */
        /// <summary>
        /// Требуемая страница не найдена - обычно указывает на повреждение.
        /// </summary>
        public const int MDBX_PAGE_NOTFOUND = -30797;

        /* Located page was wrong type */
        /// <summary>
        /// Найденная страница имеет неправильный тип.
        /// </summary>
        public const int MDBX_CORRUPTED = -30796;

        /* Update of meta page failed or environment had fatal error */
        /// <summary>
        /// Обновление метаинформации не удалось или произошла критическая ошибка.
        /// </summary>
        public const int MDBX_PANIC = -30795;

        /* DB file version mismatch with libmdbx */
        /// <summary>
        /// Несоответствие версии файла БД с libmdbx.
        /// </summary>
        public const int MDBX_VERSION_MISMATCH = -30794;

        /* File is not a valid MDBX file */
        /// <summary>
        /// Файл не является действительным файлом MDBX.
        /// </summary>
        public const int MDBX_INVALID = -30793;
        /// <summary>
        /// Достигнут максимальный размер отображения среды.
        /// </summary>
        public const int MDBX_MAP_FULL = -30792;
        /// <summary>
        /// Достигнуто максимальное количество баз данных в среде.
        /// </summary>
        public const int MDBX_DBS_FULL = -30791;
        /// <summary>
        /// Достигнуто максимальное количество слотов для читателей.
        /// </summary>
        public const int MDBX_READERS_FULL = -30790;
        /// <summary>
        /// В транзакции слишком много грязных страниц.
        /// </summary>
        public const int MDBX_TXN_FULL = -30788;
        /// <summary>
        /// Стек курсора слишком глубок - внутренняя ошибка.
        /// </summary>
        public const int MDBX_CURSOR_FULL = -30787;
        /// <summary>
        /// В странице недостаточно места - внутренняя ошибка.
        /// </summary>
        public const int MDBX_PAGE_FULL = -30786;
        /// <summary>
        /// Содержимое базы данных превысило размер отображения среды.
        /// </summary>
        public const int MDBX_MAP_RESIZED = -30785;
        /// <summary>
        /// Операция и база данных несовместимы, или тип базы данных изменился. Это может означать:
        ///  - Операция ожидает базу данных с флагом MDBX_DUPSORT / MDBX_DUPFIXED.
        ///  - Открытие именованной базы, когда безымянная база имеет флаги MDBX_DUPSORT/MDBX_INTEGERKEY.
        ///  - Доступ к записи данных как к базе данных или наоборот.
        ///  - База данных была удалена и пересоздана с другими флагами. */
        /// </summary>
        public const int MDBX_INCOMPATIBLE = -30784;
        /// <summary>
        /// Некорректное повторное использование слота таблицы блокировок читателя.
        /// </summary>
        public const int MDBX_BAD_RSLOT = -30783;
        /// <summary>
        /// Транзакция должна быть прервана, имеет дочернюю транзакцию или является некорректной.
        /// </summary>
        public const int MDBX_BAD_TXN = -30782;
        /// <summary>
        /// Некорректный размер ключа/имени БД/данных или неверный размер DUPFIXED.
        /// </summary>
        public const int MDBX_BAD_VALSIZE = -30781;
        /// <summary>
        /// Указанный DBI был неожиданно изменён.
        /// </summary>
        public const int MDBX_BAD_DBI = -30780;
        /// <summary>
        /// Непредвиденная проблема - транзакция должна быть прервана.
        /// </summary>
        public const int MDBX_PROBLEM = -30779;
        /// <summary>
        /// Другая пишущая транзакция уже выполняется.
        /// </summary>
        public const int MDBX_BUSY = -30778;
        /// <summary>
        /// Последний определённый код ошибки.
        /// </summary>
        public const int MDBX_LAST_ERRCODE = -30778;

        /// <summary>
        /// mdbx_put() или mdbx_replace() был вызван для ключа,
        /// который имеет более одного связанного значения. */
        /// </summary>
        public const int MDBX_EMULTIVAL = -30421;

        /// <summary>
        /// Плохая сигнатура рантайм-объекта(ов), это может означать:
        ///  - повреждение памяти или двойное освобождение;
        ///  - несоответствие ABI версий (редкий случай); */
        /// </summary>
        public const int MDBX_EBADSIGN = -30420;

        /// <summary>
        /// База данных должна быть восстановлена, но это НЕ может быть сделано автоматически
        /// прямо сейчас (например, в режиме только для чтения и т.п.). */
        /// </summary>
        public const int MDBX_WANNA_RECOVERY = -30419;

        /// <summary>
        /// Значение данного ключа не соответствует текущей позиции курсора,
        /// когда mdbx_cursor_put() вызывается с опцией MDBX_CURRENT. */
        /// </summary>
        public const int MDBX_EKEYMISMATCH = -30418;

        /// <summary>
        /// База данных слишком большая для текущей системы,
        /// например, не может быть отображена в оперативную память. */
        /// </summary>
        public const int MDBX_TOO_LARGE = -30417;

        /// <summary>
        /// Поток попытался использовать объект, которому он не принадлежит,
        /// например, транзакцию, запущенную другим потоком. */
        /// </summary>
        public const int MDBX_THREAD_MISMATCH = -30416;
    }
}
