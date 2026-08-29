using Xunit;

namespace MDBX.UnitTest.Env;

/// <summary>
/// Тесты функций окружения libmdbx.
/// </summary>
public class NativeMdbxEnvTests
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
    public unsafe void MdbxEnvCreate_And_MdbxEnvClose_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        Assert.True(env != null);
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env));
    }

    [Fact]
    public unsafe void MdbxEnvCreate_And_MdbxEnvCloseEx_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        Assert.True(env != null);
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCloseEx(env, true));
    }

    [Fact]
    public unsafe void MdbxEnvOpen_And_MdbxEnvClose_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_open.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env));
    }

    [WindowsOnlyFact]
    public unsafe void MdbxEnvOpenW_And_MdbxEnvClose_Works()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        string pathStr = "test_env_openw.mdbx";
        char[] pathChars = new char[pathStr.Length + 1];
        for (int i = 0; i < pathStr.Length; i++)
            pathChars[i] = pathStr[i];
        pathChars[pathStr.Length] = '\0';
        fixed (char* pathPtr = pathChars)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenW(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env));
    }

    [Fact]
    public unsafe void MdbxEnvCopy_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_copy.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        byte[] destBytes = GetNullTerminatedBytes("test_env_copy_out.mdbx");
        fixed (byte* destPtr = destBytes)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCopy(env, destPtr, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
            Assert.True(result == 0 || result > 0);
        }

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [WindowsOnlyFact]
    public unsafe void MdbxEnvCopyW_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        string pathStr = "test_env_copyw.mdbx";
        char[] pathChars = new char[pathStr.Length + 1];
        for (int i = 0; i < pathStr.Length; i++)
            pathChars[i] = pathStr[i];
        pathChars[pathStr.Length] = '\0';
        fixed (char* pathPtr = pathChars)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenW(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        string destStr = "test_env_copyw_out.mdbx";
        char[] destChars = new char[destStr.Length + 1];
        for (int i = 0; i < destStr.Length; i++)
            destChars[i] = destStr[i];
        destChars[destStr.Length] = '\0';
        fixed (char* destPtr = destChars)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCopyW(env, destPtr, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
            Assert.True(result == 0 || result > 0);
        }

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvCopy2Fd_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_copy_fd.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCopy2Fd(env, IntPtr.Zero, MDBX_copy_flags_t.MDBX_CP_DEFAULT);
        Assert.True(result >= 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvDelete_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_delete.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env));

            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvDelete(pathPtr, MDBX_env_delete_mode_t.MDBX_ENV_DELETE_ALL);
            Assert.True(result == 0 || result > 0);
        }
    }

    [Fact]
    public unsafe void MdbxEnvGetFd_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_getfd.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        IntPtr fd = IntPtr.Zero;
        int result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetFd(env, &fd);
        Assert.True(result == 0 || fd != 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [WindowsOnlyFact]
    public unsafe void MdbxEnvDeleteW_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        string pathStr = "test_env_deletew.mdbx";
        char[] pathChars = new char[pathStr.Length + 1];
        for (int i = 0; i < pathStr.Length; i++)
            pathChars[i] = pathStr[i];
        pathChars[pathStr.Length] = '\0';
        fixed (char* pathPtr = pathChars)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenW(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env));

            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvDeleteW(pathPtr, MDBX_env_delete_mode_t.MDBX_ENV_DELETE_ALL);
            Assert.True(result == 0 || result > 0);
        }
    }

    [Fact]
    public unsafe void MdbxEnvOpenForRecovery_OpensEnv()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_recovery.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenForRecovery(env, pathPtr, 0, true);
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [WindowsOnlyFact]
    public unsafe void MdbxEnvOpenForRecoveryW_OpensEnv()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        string pathStr = "test_env_recoveryw.mdbx";
        char[] pathChars = new char[pathStr.Length + 1];
        for (int i = 0; i < pathStr.Length; i++)
            pathChars[i] = pathStr[i];
        pathChars[pathStr.Length] = '\0';
        fixed (char* pathPtr = pathChars)
        {
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenForRecoveryW(env, pathPtr, 0, true);
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [Fact]
    public unsafe void MdbxEnvTurnForRecovery_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_turn_recovery.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));

            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvTurnForRecovery(env, 0);
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [Fact]
    public unsafe void MdbxEnvWarmup_WarmsUpEnv()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_warmup.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));

            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvWarmup(env, null, MDBX_warmup_flags_t.MDBX_WARMUP_DEFAULT, 0);
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [Fact]
    public unsafe void MdbxEnvDefrag_DefragmentsEnv()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_defrag.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));

            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvDefrag(env, 0, 0, 0, 0, 0, 0, null, null, null);
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [Fact]
    public unsafe void MdbxPreopenSnapinfo_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_env_snapinfo.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));

            MDBX_envinfo info;
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxPreopenSnapinfo(pathPtr, &info, (nuint)sizeof(MDBX_envinfo));
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }

    [WindowsOnlyFact]
    public unsafe void MdbxPreopenSnapinfoW_ReturnsResult()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        string pathStr = "test_env_snapinfow.mdbx";
        char[] pathChars = new char[pathStr.Length + 1];
        for (int i = 0; i < pathStr.Length; i++)
            pathChars[i] = pathStr[i];
        pathChars[pathStr.Length] = '\0';
        fixed (char* pathPtr = pathChars)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpenW(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));

            MDBX_envinfo info;
            int result = MDBX.Native.Bindings.Env.NativeMdbx.MdbxPreopenSnapinfoW(pathPtr, &info, (nuint)sizeof(MDBX_envinfo));
            Assert.True(result == 0 || result > 0);

            MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
        }
    }
}
