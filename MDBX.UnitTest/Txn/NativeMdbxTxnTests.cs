using Xunit;

namespace MDBX.UnitTest.Txn;

/// <summary>
/// Тесты функций транзакций libmdbx.
/// </summary>
public class NativeMdbxTxnTests
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
    public unsafe void MdbxTxnBeginEx_And_MdbxTxnCommitEx_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_commit.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnCommitEx(txn, null));
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnBeginEx_And_MdbxTxnRollback_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_rollback.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn));
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnInfo_ReturnsValidInfo()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_info.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MDBX_txn_info info = new MDBX_txn_info();
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnInfo(txn, &info, false));

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnId_ReturnsValidId()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_id.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        ulong id = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnId(txn);
        Assert.True(id >= 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnEnv_ReturnsValidEnv()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_env.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MDBX_env* txnEnv = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnEnv(txn);
        Assert.True(txnEnv == env);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnFlags_ReturnsValidFlags()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_flags.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MDBX_txn_flags_t flags = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnFlags(txn);
        Assert.True((int)flags >= 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnClone_And_MdbxTxnAbortEx_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_clone.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MDBX_txn* clone = null;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnClone(txn, &clone, null));
        Assert.True(clone != null);

        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(txn, null));
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(clone, null));

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnReset_And_MdbxTxnRenew_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_reset.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_RDONLY, &txn, 0));

        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnReset(txn));
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRenew(txn));
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnAbortEx(txn, null));

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnRefresh_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_refresh.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int result = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRefresh(txn);
        Assert.True(result >= 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnSetUserctx_And_MdbxTxnGetUserctx_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_userctx.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int testValue = 42;
        int* testPtr = &testValue;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnSetUserctx(txn, testPtr));
        void* result = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnGetUserctx(txn);
        Assert.True(result == testPtr);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnCheckpoint_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_checkpoint.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int result = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnCheckpoint(txn, 0, null);
        Assert.True(result != int.MinValue);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnBreak_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_break.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int result = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBreak(txn);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnPark_And_MdbxTxnUnpark_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_park.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_RDONLY, &txn, 0));

        int parkResult = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnPark(txn, false);
        Assert.True(parkResult != int.MinValue);

        int unparkResult = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnUnpark(txn, false);
        Assert.True(unparkResult != int.MinValue);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnReleaseAllCursorsEx_ReleasesCursors()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_release_cursors.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        nuint count = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnReleaseAllCursorsEx(txn, false, &count));

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxTxnStraggler_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_txn_straggler.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        int percent = 0;
        int result = MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnStraggler(txn, &percent);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
