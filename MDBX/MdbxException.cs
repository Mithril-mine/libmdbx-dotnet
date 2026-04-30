namespace MDBX
{
    using Interop;

    /// <summary>
    /// Исключение MDBX.
    /// </summary>
    public class MdbxException : Exception
    {
        /// <summary>
        /// Номер ошибки.
        /// </summary>
        public int ErrorNumber { get { return _errorNumber; } }
        private readonly int _errorNumber;
        internal MdbxException(string method, int errNum) :
            base(GetMessage(method, errNum))
        {
            _errorNumber = errNum;
        }

        private static string GetMessage(string method, int errNum)
        {
            return string.Format("MDBX {0} returned ({1}) - {2}"
                , method
                , errNum
                , Misc.StringError(errNum)
                );
        }
    }
}
