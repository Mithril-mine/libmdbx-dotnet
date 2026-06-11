namespace MDBX;

using Interop;

/// <summary>
/// Исключение MDBX.
/// </summary>
public class MdbxException : Exception
{
    /// <summary>
    /// Номер ошибки MDBX.
    /// </summary>
    public int ErrorNumber { get { return _errorNumber; } }
    private readonly int _errorNumber;

    /// <summary>
    /// Инициализирует новый экземпляр класса MdbxException.
    /// </summary>
    /// <param name="method">Имя метода, который вызвал ошибку.</param>
    /// <param name="errNum">Код ошибки MDBX.</param>
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
            , MdbxInteropMisc.StringError(errNum)
            );
    }
}
