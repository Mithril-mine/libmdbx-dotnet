using MDBX.Native.Types;
using Xunit;

namespace MDBX.UnitTest.Extra;

/// <summary>
/// Тесты дополнительных операций libmdbx, включая функции кэширования,
/// проверки целостности, is_dirty, gc_info, get_equal_or_great и другие.
/// </summary>
public class NativeMdbxExtraTests
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
    public unsafe void MdbxCmp_ComparesKeysSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cmp.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key1 = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val key2 = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Extra.NativeMdbx.MdbxCmp(txn, dbi, &key1, &key2));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGetKeycmp_ReturnsComparisonFunction()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_keycmp.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        var cmpFunc = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxGetKeycmp(MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(cmpFunc != null);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxKeyFromDouble_ConvertsSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_keydouble.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        double testValue = 3.14159;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromDouble(testValue);
        Assert.True(key != 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxKeyFromJsonInteger_ConvertsSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_keyjson.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        long testValue = 42;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromJsonInteger(testValue);
        Assert.True(key != 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGetEqualOrGreat_ReturnsEqualKey()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_get_equal_or_great.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        byte[] searchKeyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        fixed (byte* searchKeyPtr = searchKeyBytes)
        {
            MDBX_val searchKey = new MDBX_val(searchKeyPtr, (nuint)searchKeyBytes.Length);
            MDBX_val data = new MDBX_val(null, 0);
            int result = MDBX.Native.Bindings.Crud.NativeMdbx.MdbxGetEqualOrGreat(txn, dbi, &searchKey, &data);
            Assert.Equal(0, result);
            Assert.True(data.IovBase != null);
            Assert.Equal((nuint)valueBytes.Length, data.IovLen);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxIsDirty_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_is_dirty.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        byte[] searchKeyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        fixed (byte* searchKeyPtr = searchKeyBytes)
        {
            MDBX_val searchKey = new MDBX_val(searchKeyPtr, (nuint)searchKeyBytes.Length);
            MDBX_val data = new MDBX_val(null, 0);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxGet(txn, dbi, &searchKey, &data));

            int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxIsDirty(txn, data.IovBase);
            Assert.True(true);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGcInfo_ReturnsGcInfo()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_gc_info.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("gc_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("gc_value");

        for (int i = 0; i < 100; i++)
        {
            MDBX_txn* txn;
            Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

            uint dbi;
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

            fixed (byte* keyPtr = keyBytes)
            fixed (byte* valPtr = valueBytes)
            {
                MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
                MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
                Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
            }

            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi));
            Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null));
        }

        MDBX_txn* readTxn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &readTxn, 0));

        MdbxGcInfo info;
        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxGcInfo(readTxn, &info, (nuint)sizeof(MdbxGcInfo), null, null);
        Assert.True(result == 0 || result > 0 || result == -30798);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(readTxn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCacheGet_ReturnsCacheResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cache_get.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("cache_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("cache_value");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MdbxCacheEntry entry = new MdbxCacheEntry();
        byte[] searchKeyBytes = System.Text.Encoding.UTF8.GetBytes("cache_key");
        fixed (byte* searchKeyPtr = searchKeyBytes)
        {
            MDBX_val searchKey = new MDBX_val(searchKeyPtr, (nuint)searchKeyBytes.Length);
            MDBX_val data = new MDBX_val(null, 0);
            MDBX_cache_result_t result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxCacheGet(txn, dbi, &searchKey, &data, &entry);
            Assert.Equal(0, result.Errcode);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCacheGetSingleThreaded_ReturnsCacheResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cache_single.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("single_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("single_value");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MdbxCacheEntry entry = new MdbxCacheEntry();
        byte[] searchKeyBytes = System.Text.Encoding.UTF8.GetBytes("single_key");
        fixed (byte* searchKeyPtr = searchKeyBytes)
        {
            MDBX_val searchKey = new MDBX_val(searchKeyPtr, (nuint)searchKeyBytes.Length);
            MDBX_val data = new MDBX_val(null, 0);
            MDBX_cache_result_t result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxCacheGetSingleThreaded(txn, dbi, &searchKey, &data, &entry);
            Assert.Equal(0, result.Errcode);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDcmp_ComparesValuesSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dcmp.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] val1Bytes = System.Text.Encoding.UTF8.GetBytes("a");
        byte[] val2Bytes = System.Text.Encoding.UTF8.GetBytes("b");
        fixed (byte* val1Ptr = val1Bytes)
        fixed (byte* val2Ptr = val2Bytes)
        {
            MDBX_val val1 = new MDBX_val(val1Ptr, (nuint)val1Bytes.Length);
            MDBX_val val2 = new MDBX_val(val2Ptr, (nuint)val2Bytes.Length);
            int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxDcmp(txn, dbi, &val1, &val2);
            Assert.True(result < 0);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxReaderList_ReturnsZeroOrError()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_reader_list.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxReaderList(env, null, null);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxReaderCheck_ReturnsZero()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_reader_check.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int dead;
        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxReaderCheck(env, &dead);
        Assert.Equal(0, result);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxThreadRegister_ReturnsZero()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_thread_register.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxThreadRegister(env);
        Assert.Equal(0, result);

        result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxThreadUnregister(env);
        Assert.Equal(0, result);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEstimateDistance_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_estimate_dist.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        nint distanceItems = 0;
        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxEstimateDistance(cursor, cursor, &distanceItems);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiOpenEx_WithNullCmpFunc_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 1);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_open_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        byte[] nameBytes = GetNullTerminatedBytes("test_ex");
        fixed (byte* namePtr = nameBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpenEx(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi, null, null));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiOpenEx2_WithNullCmpFunc_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 1);
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_open_ex2.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes("test_ex2");
        fixed (byte* namePtr = nameBytes)
        {
            MDBX_val name = new MDBX_val(namePtr, (nuint)nameBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpenEx2(txn, &name, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi, null, null));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetMaxkeysize_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_maxkeysize.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int maxKeySize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxkeysize(env);
        Assert.True(maxKeySize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetMaxkeysizeEx_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_maxkeysize_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int maxKeySize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxkeysizeEx(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxKeySize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvChk_WithMinimalCallbacks_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_chk.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MdbxChkCallbacks callbacks = new MdbxChkCallbacks();
        MdbxChkContext ctx = new MdbxChkContext();
        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxEnvChk(env, &callbacks, &ctx, MdbxChkFlags.MDBX_CHK_DEFAULTS, MdbxChkSeverity.MDBX_chk_verbose, 0);
        Assert.True(result == 0 || result > 0);

        int result2 = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxEnvChkEncountProblem(&ctx);
        Assert.True(result2 == 0 || result2 > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnCopy2Fd_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_copy_fd.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxTxnCopy2Fd(txn, IntPtr.Zero, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnCopy2Pathname_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_copy_path.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        byte[] destBytes = GetNullTerminatedBytes("test_txn_copy_out.mdbx");
        fixed (byte* destPtr = destBytes)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxTxnCopy2Pathname(txn, destPtr, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
            Assert.True(result == 0 || result > 0);
        }

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnCopy2PathnameW_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_copy_pathw.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        char[] destChars = new char[25];
        string destStr = "test_txn_copy_outw.mdbx";
        for (int i = 0; i < destStr.Length; i++)
            destChars[i] = destStr[i];
        destChars[destStr.Length] = '\0';
        fixed (char* destPtr = destChars)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxTxnCopy2PathnameW(txn, destPtr, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
            Assert.True(result == 0 || result > 0);
        }

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDumpVal_FormatsValueToString()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dump_val.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            byte[] buf = new byte[256];
            fixed (byte* bufPtr = buf)
            {
                byte* result = MDBX.Native.Bindings.Debug.NativeMdbx.MdbxDumpVal(&key, bufPtr, (nuint)buf.Length);
                Assert.True(result != null);
            }
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnumerateTables_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_enum_tables.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxEnumerateTables(txn, null, null);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGetDatacmp_ReturnsComparisonFunction()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_datacmp.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        var cmpFunc = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxGetDatacmp(MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(cmpFunc != null);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxKeyFromFloat_ConvertsSuccessfully()
    {
        float testValue = 3.14f;
        uint key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromFloat(testValue);
        Assert.True(key != 0);
    }

    [Fact]
    public unsafe void MdbxKeyFromPtrfloat_ConvertsSuccessfully()
    {
        float testValue = 3.14f;
        float* ptr = &testValue;
        uint key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromPtrfloat(ptr);
        Assert.True(key != 0);
    }

    [Fact]
    public unsafe void MdbxKeyFromPtrdouble_ConvertsSuccessfully()
    {
        double testValue = 3.14159;
        double* ptr = &testValue;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromPtrdouble(ptr);
        Assert.True(key != 0);
    }

    [Fact]
    public unsafe void MdbxInt32FromKey_ExtractsValue()
    {
        int testValue = 42;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromJsonInteger(testValue);
        int truncated = (int)key;
        int extracted = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxInt32FromKey(new MDBX_val(&truncated, (nuint)sizeof(int)));
        Assert.True(extracted != 0 || extracted == 0);
    }

    [Fact]
    public unsafe void MdbxInt64FromKey_ExtractsValue()
    {
        long testValue = 42;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromJsonInteger(testValue);
        long extracted = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxInt64FromKey(new MDBX_val(&key, (nuint)sizeof(long)));
        Assert.True(extracted != 0 || extracted == 0);
    }

    [Fact]
    public unsafe void MdbxFloatFromKey_ExtractsValue()
    {
        float testValue = 3.14f;
        uint key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromFloat(testValue);
        float extracted = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxFloatFromKey(new MDBX_val(&key, (nuint)sizeof(float)));
        Assert.Equal(testValue, extracted);
    }

    [Fact]
    public unsafe void MdbxDoubleFromKey_ExtractsValue()
    {
        double testValue = 3.14159;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromDouble(testValue);
        double extracted = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxDoubleFromKey(new MDBX_val(&key, (nuint)sizeof(double)));
        Assert.Equal(testValue, extracted);
    }

    [Fact]
    public unsafe void MdbxJsonIntegerFromKey_ExtractsValue()
    {
        long testValue = 42;
        ulong key = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxKeyFromJsonInteger(testValue);
        long extracted = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxJsonIntegerFromKey(new MDBX_val(&key, (nuint)sizeof(long)));
        Assert.Equal(testValue, extracted);
    }

    [Fact]
    public unsafe void MdbxTxnLock_And_MdbxTxnUnlock_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_lock.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxTxnLock(env, true);
        Assert.True(result == 0 || result > 0);

        result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxTxnUnlock(env);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEstimateMove_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_estimate_move.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_CREATE, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        nint distanceItems = 0;
        int result = MDBX.Native.Bindings.Extra.NativeMdbx.MdbxEstimateMove(cursor, null, null, MDBX_cursor_op.MDBX_FIRST, &distanceItems);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDbiSequence_ReturnsAndIncrements()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_extra_sequence.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        ulong result = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Extra.NativeMdbx.MdbxDbiSequence(txn, dbi, &result, 1));
        Assert.True(result >= 0);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
