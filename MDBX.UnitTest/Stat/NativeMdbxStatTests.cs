using MDBX.Native.Types;
using Xunit;

namespace MDBX.UnitTest.Stat;

/// <summary>
/// Тесты функций статистики libmdbx.
/// </summary>
public class NativeMdbxStatTests
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
    public unsafe void MdbxEnvStatEx_ReturnsStatisticsSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_stat.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MdbxStat stat;
        int err = MDBX.Native.Bindings.Stat.NativeMdbx.MdbxEnvStatEx(env, txn, &stat, (nuint)sizeof(MdbxStat));
        Assert.Equal(0, err);
        Assert.True(stat.MsPsize > 0, $"MsPsize={stat.MsPsize}, sizeof(MdbxStat)={sizeof(MdbxStat)}");

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvInfoEx_ReturnsInfoSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_info.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MDBX_envinfo info;
        Assert.Equal(0, MDBX.Native.Bindings.Stat.NativeMdbx.MdbxEnvInfoEx(env, txn, &info, (nuint)sizeof(MDBX_envinfo)));

        Assert.True(info.MiMapsize > 0);
        Assert.True(info.MiDxbFsize > 0);

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvSyncEx_SyncsSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_sync.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int err = MDBX.Native.Bindings.Stat.NativeMdbx.MdbxEnvSyncEx(env, true, false);
        Assert.True(err == 0 || err == -1);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
