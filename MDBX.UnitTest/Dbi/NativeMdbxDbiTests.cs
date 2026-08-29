using MDBX.Native.Types;
using Xunit;

namespace MDBX.UnitTest.Dbi;

/// <summary>
/// Тесты функций работы с таблицами (DBI) libmdbx.
/// </summary>
public class NativeMdbxDbiTests
{
    private const ushort DefaultMode = 0x700;

    private static byte[] GetNullTerminatedBytes(string s)
    {
        byte[] bytes = new byte[s.Length + 1];
        for (int i = 0; i < s.Length; i++)
            bytes[i] = (byte)s[i];
        bytes[s.Length] = 0;
        return bytes;
    }

    [Fact]
    public unsafe void MdbxDbiStat_ReturnsStatistics()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_stat.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        // Вставим элемент
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        // Получить статистику
        MdbxStat stat;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiStat(txn, dbi, &stat, (nuint)sizeof(MdbxStat)));

        Assert.True(stat.MsPsize > 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiOpen2_OpensByValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 1);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_open2.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        // Откроем таблицу по значению вместо строки
        byte[] tableName = System.Text.Encoding.UTF8.GetBytes("test_table");
        uint dbi;
        fixed (byte* namePtr = tableName)
        {
            MDBX_val nameVal = new MDBX_val(namePtr, (nuint)tableName.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen2(txn, &nameVal, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiRename_RenampsTable()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 2);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_rename.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        // Открыть таблицу с исходным именем
        byte[] oldName = GetNullTerminatedBytes("old_table");
        uint dbi;
        fixed (byte* namePtr = oldName)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));
        }

        // Переименовать таблицу
        byte[] newName = GetNullTerminatedBytes("new_table");
        fixed (byte* newNamePtr = newName)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiRename(txn, dbi, newNamePtr));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiRename2_RenamesByValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 2);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_rename2.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        // Открыть таблицу
        byte[] oldName = GetNullTerminatedBytes("table_rename2");
        uint dbi;
        fixed (byte* namePtr = oldName)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));
        }

        // Переименовать таблицу по значению
        byte[] newName = System.Text.Encoding.UTF8.GetBytes("table_renamed");
        fixed (byte* newNamePtr = newName)
        {
            MDBX_val newNameVal = new MDBX_val(newNamePtr, (nuint)newName.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiRename2(txn, dbi, &newNameVal));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiDupsortDepthmask_GetsDepthMask()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 2);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_depthmask.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        // Открыть таблицу с дубликатами
        byte[] tableName = GetNullTerminatedBytes("dupsort_table");
        uint dbi;
        fixed (byte* namePtr = tableName)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_DUPSORT | MDBX_db_flags_t.MDBX_CREATE, &dbi));
        }

        uint mask = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiDupsortDepthmask(txn, dbi, &mask));
        Assert.True(mask >= 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiFlagsEx_GetsFlagsAndState()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_flags_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        uint flags = 0;
        uint state = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiFlagsEx(txn, dbi, &flags, &state));
        Assert.True(flags >= 0);
        Assert.True(state >= 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiOpen_OpensDefaultTable()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_open.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));
        Assert.True(dbi > 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiClose_ClosesTableSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_close.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi));

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDrop_DropsTableSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 2);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_drop.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        byte[] tableName = GetNullTerminatedBytes("drop_table");
        uint dbi;
        fixed (byte* namePtr = tableName)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));
        }

        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDrop(txn, dbi, true));

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiSequence_ReturnsAndIncrements()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_sequence.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        ulong result = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiSequence(txn, dbi, &result, 1));
        Assert.True(result >= 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
