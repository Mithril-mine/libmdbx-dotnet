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
        /// Environment mapsize reached
        /// </summary>
        public const int MDBX_MAP_FULL = -30792;
        /// <summary>
        /// Environment maxdbs reached
        /// </summary>
        public const int MDBX_DBS_FULL = -30791;
        /// <summary>
        /// Environment maxreaders reached
        /// </summary>
        public const int MDBX_READERS_FULL = -30790;
        /// <summary>
        /// Txn has too many dirty pages
        /// </summary>
        public const int MDBX_TXN_FULL = -30788;
        /// <summary>
        /// Cursor stack too deep - internal error
        /// </summary>
        public const int MDBX_CURSOR_FULL = -30787;
        /// <summary>
        /// Page has not enough space - internal error.
        /// </summary>
        public const int MDBX_PAGE_FULL = -30786;
        /// <summary>
        /// Database contents grew beyond environment mapsize
        /// </summary>
        public const int MDBX_MAP_RESIZED = -30785;
        /// <summary>
        /// Operation and DB incompatible, or DB type changed. This can mean:
        ///  - The operation expects an MDBX_DUPSORT / MDBX_DUPFIXED database.
        ///  - Opening a named DB when the unnamed DB has MDBX_DUPSORT/MDBX_INTEGERKEY.
        ///  - Accessing a data record as a database, or vice versa.
        ///  - The database was dropped and recreated with different flags. */
        /// </summary>
        public const int MDBX_INCOMPATIBLE = -30784;
        /// <summary>
        /// Invalid reuse of reader locktable slot
        /// </summary>
        public const int MDBX_BAD_RSLOT = -30783;
        /// <summary>
        /// Transaction must abort, has a child, or is invalid
        /// </summary>
        public const int MDBX_BAD_TXN = -30782;
        /// <summary>
        /// Unsupported size of key/DB name/data, or wrong DUPFIXED size
        /// </summary>
        public const int MDBX_BAD_VALSIZE = -30781;
        /// <summary>
        /// The specified DBI was changed unexpectedly
        /// </summary>
        public const int MDBX_BAD_DBI = -30780;
        /// <summary>
        /// Unexpected problem - txn should abort
        /// </summary>
        public const int MDBX_PROBLEM = -30779;
        /// <summary>
        /// Another write transaction is running
        /// </summary>
        public const int MDBX_BUSY = -30778;
        /// <summary>
        /// The last defined error code
        /// </summary>
        public const int MDBX_LAST_ERRCODE = -30778;

        /// <summary>
        /// The mdbx_put() or mdbx_replace() was called for key,
        /// that has more that one associated value. */
        /// </summary>
        public const int MDBX_EMULTIVAL = -30421;

        /// <summary>
        /// Bad signature of a runtime object(s), this can mean:
        ///  - memory corruption or double-free;
        ///  - ABI version mismatch (rare case); */
        /// </summary>
        public const int MDBX_EBADSIGN = -30420;

        /// <summary>
        ///Database should be recovered, but this could NOT be done automatically
        ///right now (e.g. in readonly mode and so forth). */
        /// </summary>
        public const int MDBX_WANNA_RECOVERY = -30419;

        /// <summary>
        /// The given key value is mismatched to the current cursor position,
        /// when mdbx_cursor_put() called with MDBX_CURRENT option. */
        /// </summary>
        public const int MDBX_EKEYMISMATCH = -30418;

        /// <summary>
        /// Database is too large for current system,
        /// e.g. could NOT be mapped into RAM. */
        /// </summary>
        public const int MDBX_TOO_LARGE = -30417;

        /// <summary>
        /// A thread has attempted to use a not owned object,
        /// e.g. a transaction that started by another thread. */
        /// </summary>
        public const int MDBX_THREAD_MISMATCH = -30416;
    }
}
