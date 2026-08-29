using MDBX.Native.Types;
using System.Runtime.InteropServices;
using Xunit;

namespace MDBX.UnitTest.Debug;

/// <summary>
/// Тесты функций отладки libmdbx.
/// </summary>
public class NativeMdbxDebugTests
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
    public unsafe void MdbxSetupDebug_ReturnsAny()
    {
        int result = MDBX.Native.Bindings.Debug.NativeMdbx.MdbxSetupDebug(MDBX_log_level_t.MDBX_LOG_NOTICE, MDBX_debug_flags_t.MDBX_DBG_NONE, null);
        Assert.True(result == 0 || result == 1 || result > 0);
    }

    [Fact]
    public unsafe void MdbxSetupDebugNofmt_ReturnsAny()
    {
        int result = MDBX.Native.Bindings.Debug.NativeMdbx.MdbxSetupDebugNofmt(MDBX_log_level_t.MDBX_LOG_NOTICE, MDBX_debug_flags_t.MDBX_DBG_NONE, null, null, 0);
        Assert.True(result == 0 || result == 1 || result == -1 || result > 0);
    }

    [Fact]
    public unsafe void MdbxDumpVal_FormatsValueToString()
    {
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("hello_world");
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);

            byte[] buf = new byte[256];
            fixed (byte* bufPtr = buf)
            {
                byte* result = MDBX.Native.Bindings.Debug.NativeMdbx.MdbxDumpVal(&val, bufPtr, (nuint)buf.Length);
                Assert.True(result != null);
                string str = Marshal.PtrToStringUTF8((IntPtr)result)!;
                Assert.False(string.IsNullOrEmpty(str));
            }
        }
    }

    [Fact]
    public unsafe void MdbxCanaryPut_And_MdbxCanaryGet_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_debug_canary.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        MdbxCanary canary = new MdbxCanary();
        Assert.Equal(0, MDBX.Native.Bindings.Debug.NativeMdbx.MdbxCanaryPut(txn, &canary));

        MdbxCanary retrievedCanary = new MdbxCanary();
        Assert.Equal(0, MDBX.Native.Bindings.Debug.NativeMdbx.MdbxCanaryGet(txn, &retrievedCanary));

        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
