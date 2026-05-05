using Libmdbx.Interop;
using System.Runtime.InteropServices;

namespace Libmdbx
{
    /// <summary>
    /// Exception thrown when a libmdbx native function returns an error code.
    /// </summary>
    public class MdbxException : Exception
    {
        /// <summary>The raw libmdbx error code.</summary>
        public int ErrorCode { get; }

        internal MdbxException(string functionName, int errorCode)
            : base(BuildMessage(functionName, errorCode))
        {
            ErrorCode = errorCode;
        }

        private static string BuildMessage(string functionName, int errorCode)
        {
            // Ask libmdbx for the human-readable description
            IntPtr ptr = NativeMethods.mdbx_strerror(errorCode);
            string description = ptr != IntPtr.Zero
                ? Marshal.PtrToStringAnsi(ptr) ?? errorCode.ToString()
                : errorCode.ToString();
            return $"libmdbx '{functionName}' failed with code {errorCode}: {description}";
        }

        // ------------------------------------------------------------------
        // Well-known libmdbx error codes (from mdbx.h)
        // ------------------------------------------------------------------

        /// <summary>Successful result.</summary>
        public const int Success = 0;

        /// <summary>key/data pair already exists.</summary>
        public const int KeyExist = -30799;

        /// <summary>key/data pair not found (EOF).</summary>
        public const int NotFound = -30798;

        /// <summary>Requested page not found – usually indicates corruption.</summary>
        public const int PageNotFound = -30797;

        /// <summary>Located page was of the wrong type.</summary>
        public const int Corrupted = -30796;

        /// <summary>Update of meta page failed or environment had fatal error.</summary>
        public const int Panic = -30795;

        /// <summary>DB file version mismatch with libmdbx.</summary>
        public const int VersionMismatch = -30794;

        /// <summary>File is not a valid MDBX file.</summary>
        public const int Invalid = -30793;

        /// <summary>Environment mapsize limit reached.</summary>
        public const int MapFull = -30792;

        /// <summary>Environment maxdbs limit reached.</summary>
        public const int DbsFull = -30791;

        /// <summary>Environment maxreaders limit reached.</summary>
        public const int ReadersFull = -30790;

        /// <summary>Transaction has too many dirty pages.</summary>
        public const int TxnFull = -30788;

        /// <summary>Cursor stack too deep – internal error.</summary>
        public const int CursorFull = -30787;

        /// <summary>Page has not enough space – internal error.</summary>
        public const int PageFull = -30786;

        /// <summary>DB contents grew beyond environment mapsize.</summary>
        public const int MapResized = -30785;

        /// <summary>Operation and DB incompatible, or DB type changed.</summary>
        public const int Incompatible = -30784;

        /// <summary>Invalid reuse of reader lock-table slot.</summary>
        public const int BadRSlot = -30783;

        /// <summary>Transaction must abort, has a child, or is invalid.</summary>
        public const int BadTxn = -30782;

        /// <summary>Unsupported size of key/DB-name/data, or wrong DUPFIXED size.</summary>
        public const int BadValSize = -30781;

        /// <summary>The specified DBI was changed unexpectedly.</summary>
        public const int BadDbi = -30780;

        /// <summary>Unexpected problem – transaction should abort.</summary>
        public const int Problem = -30779;

        /// <summary>Another write transaction is running.</summary>
        public const int Busy = -30778;

        /// <summary>
        /// <c>mdbx_put()</c> / <c>mdbx_replace()</c> called for a key that
        /// has more than one associated value.
        /// </summary>
        public const int EMultiVal = -30421;

        /// <summary>Bad signature of a runtime object.</summary>
        public const int EBadSign = -30420;

        /// <summary>Database should be recovered but cannot be done automatically.</summary>
        public const int WannaRecovery = -30419;

        /// <summary>
        /// Given key value is mismatched to the current cursor position.
        /// </summary>
        public const int EKeyMismatch = -30418;

        /// <summary>Database is too large for the current system.</summary>
        public const int TooLarge = -30417;

        /// <summary>
        /// A thread attempted to use an object not owned by that thread.
        /// </summary>
        public const int ThreadMismatch = -30416;
    }
}
