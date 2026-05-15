using MDBX.Interop.Functions;

namespace MDBX.Interop;

internal static partial class Transaction
{
    /// <summary>
    /// Привязывает все делегаты транзакций к нативным функциям библиотеки MDBX.
    /// </summary>
    internal static void Bind()
    {
        _beginDelegate = NativeLibraryLoader.GetProcAddress<BeginDelegate>(MdbxFunctions.Transaction.Begin);
        _commitDelegate = NativeLibraryLoader.GetProcAddress<CommitDelegate>(MdbxFunctions.Transaction.Commit);
        _abortDelegate = NativeLibraryLoader.GetProcAddress<AbortDelegate>(MdbxFunctions.Transaction.Abort);
        _resetDelegate = NativeLibraryLoader.GetProcAddress<ResetDelegate>(MdbxFunctions.Transaction.Reset);
        _renewDelegate = NativeLibraryLoader.GetProcAddress<RenewDelegate>(MdbxFunctions.Transaction.Renew);
        _getTxnIdDelegate = NativeLibraryLoader.GetProcAddress<GetTxnIdDelegate>(MdbxFunctions.Transaction.GetID);
    }
}
