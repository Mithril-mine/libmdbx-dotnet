using Xunit;

namespace MDBX.UnitTest.Crud;

/// <summary>
/// Тесты CRUD операций libmdbx.
/// </summary>
public class NativeMdbxCrudTests
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
    public unsafe void MdbxPutAndGet_WorksCorrectly()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_put_get.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(null, 0);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxGet(txn, dbi, &key, &val));
            Assert.True(val.IovBase != null);
            Assert.Equal(valueBytes.Length, (long)val.IovLen);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxReplace_UpdatesValueSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_replace.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");
        byte[] newValueBytes = System.Text.Encoding.UTF8.GetBytes("new_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* newValPtr = newValueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val newVal = new MDBX_val(newValPtr, (nuint)newValueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &newVal, MDBX_put_flags_t.MDBX_CURRENT));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDel_RemovesValueSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_del.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxDel(txn, dbi, &key, null));
        }

        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(null, 0);
            Assert.NotEqual(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxGet(txn, dbi, &key, &val));
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGetEx_WithValueCount_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_get_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(null, 0);
            nuint valuesCount = 0;
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxGetEx(txn, dbi, &key, &val, &valuesCount));
            Assert.True(val.IovBase != null);
            Assert.Equal(valueBytes.Length, (long)val.IovLen);
            Assert.Equal(1UL, valuesCount);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxReplaceEx_WithValueCount_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_replace_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");
        byte[] newValueBytes = System.Text.Encoding.UTF8.GetBytes("new_value");

        // Вставим исходное значение
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        // Заменим значение с помощью ReplaceEx
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* newValPtr = newValueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val newVal = new MDBX_val(newValPtr, (nuint)newValueBytes.Length);
            byte* oldBuf = stackalloc byte[valueBytes.Length];
            MDBX_val oldVal = new MDBX_val(oldBuf, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxReplaceEx(txn, dbi, &key, &newVal, &oldVal, 0, null, null));
            Assert.True(oldVal.IovBase != null);
            Assert.Equal(valueBytes.Length, (long)oldVal.IovLen);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxReplace_ReturnsOldValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_crud_replace_direct.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("old_value");
        byte[] newValueBytes = System.Text.Encoding.UTF8.GetBytes("new_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* newValPtr = newValueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val newVal = new MDBX_val(newValPtr, (nuint)newValueBytes.Length);
            byte* oldBuf = stackalloc byte[valueBytes.Length];
            MDBX_val oldVal = new MDBX_val(oldBuf, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxReplace(txn, dbi, &key, &newVal, &oldVal, 0));
            Assert.True(oldVal.IovBase != null);
            Assert.Equal(valueBytes.Length, (long)oldVal.IovLen);
        }

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
