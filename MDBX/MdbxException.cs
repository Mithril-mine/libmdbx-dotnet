namespace MDBX
{
    using Interop;

    /// <summary>
    /// MdbxException.
    /// </summary>
    public class MdbxException : Exception
    {
        /// <summary>
        /// ErrorNumber.
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
