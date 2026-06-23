using System.Runtime.InteropServices;
using Xunit;

namespace MDBX.UnitTest.Settings;

/// <summary>
/// Тесты настроек окружения libmdbx.
/// </summary>
public class NativeMdbxSettingsTests
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
    public unsafe void MdbxEnvSetOption_And_MdbxEnvGetOption_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));

        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, 10));

        ulong value = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetOption(env, MDBX_option_t.MDBX_OPT_MAX_DB, &value));
        Assert.Equal(10UL, value);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvSetFlags_And_MdbxEnvGetFlags_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_flags.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetFlags(env, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, true));

        uint flags = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetFlags(env, &flags));

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetMaxkeysize_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_maxkey.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int maxKeySize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxkeysize(env);
        Assert.True(maxKeySize > 0);

        int maxKeySizeEx = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxkeysizeEx(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxKeySizeEx > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetMaxvalsizeEx_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_maxval.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int maxValSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxvalsizeEx(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxValSize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetPath_ReturnsValidPath()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_path.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        byte* pathPtrResult;
        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPath(env, &pathPtrResult));
        Assert.True(pathPtrResult != null);
        string path = Marshal.PtrToStringUTF8((IntPtr)pathPtrResult)!;
        Assert.False(string.IsNullOrEmpty(path));

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetPathW_ReturnsValidPath()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_pathw.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        char* pathPtrResult;
        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPathW(env, &pathPtrResult));
        Assert.True(pathPtrResult != null);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetFd_ReturnsValidFd()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_fd.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        IntPtr fd;
        int result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetFd(env, &fd);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvSetUserctx_And_MdbxEnvGetUserctx_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));

        int testValue = 42;
        int* testPtr = &testValue;
        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetUserctx(env, testPtr));
        void* result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetUserctx(env);
        Assert.True(result == testPtr);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxDefaultPagesize_ReturnsPositiveValue()
    {
        nuint pageSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        Assert.True(pageSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsKeysizeMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint maxKeySize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsKeysizeMax(pageSize, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxKeySize > 0);
    }

    [Fact]
    public unsafe void MdbxEnvSetGeometry_SetsGeometry()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_geometry.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetGeometry(env, -1, -1, -1, -1, -1, 0));

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvSetHsr_And_MdbxEnvGetHsr_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_hsr.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        Assert.Equal(0, MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvSetHsr(env, null));
        var hsr = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetHsr(env);
        Assert.True(hsr == null);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetMaxvalsize_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_maxval_old.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int maxValSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetMaxvalsizeEx(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxValSize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetPairsize4pageMax_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_pairsize.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int pairSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetPairsize4pageMax(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(pairSize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxEnvGetValsize4pageMax_ReturnsPositiveValue()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_settings_valsize.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        int valSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxEnvGetValsize4pageMax(env, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(valSize > 0);

        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxGetSysraminfo_ReturnsInfo()
    {
        nint pageSize = 0;
        nint totalPages = 0;
        nint availPages = 0;
        int result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxGetSysraminfo(&pageSize, &totalPages, &availPages);
        Assert.True(result == 0 || result > 0);
        Assert.True(pageSize > 0);
    }

    [Fact]
    public unsafe void MdbxIsReadaheadReasonable_ReturnsResult()
    {
        int result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxIsReadaheadReasonable(1024 * 1024, 0);
        Assert.True(result == 0 || result == -1 || result > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsDbsizeMin_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint minSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsDbsizeMin(pageSize);
        Assert.True(minSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsDbsizeMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint maxSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsDbsizeMax(pageSize);
        Assert.True(maxSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsKeysizeMin_ReturnsPositiveValue()
    {
        nint minKeySize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsKeysizeMin(MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(minKeySize >= 0);
    }

    [Fact]
    public unsafe void MdbxLimitsValsizeMin_ReturnsPositiveValue()
    {
        nint minValSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsizeMin(MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(minValSize >= 0);
    }

    [Fact]
    public unsafe void MdbxLimitsValsizeMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint maxValSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsizeMax(pageSize, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(maxValSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsPairsize4pageMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint pairSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsPairsize4pageMax(pageSize, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(pairSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsValsize4pageMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint valSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsValsize4pageMax(pageSize, MDBX_db_flags_t.MDBX_DB_DEFAULTS);
        Assert.True(valSize > 0);
    }

    [Fact]
    public unsafe void MdbxLimitsTxnsizeMax_ReturnsPositiveValue()
    {
        nint pageSize = (nint)MDBX.Native.Bindings.Settings.NativeMdbx.MdbxDefaultPagesize();
        nint txnSize = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxLimitsTxnsizeMax(pageSize);
        Assert.True(txnSize > 0);
    }

    [Fact]
    public unsafe void MdbxRatio2Digits_ReturnsString()
    {
        byte[] buffer = new byte[64];
        fixed (byte* bufPtr = buffer)
        {
            byte* result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxRatio2Digits(100, 200, 2, bufPtr, (nuint)buffer.Length);
            Assert.True(result != null);
            string str = Marshal.PtrToStringUTF8((IntPtr)result)!;
            Assert.False(string.IsNullOrEmpty(str));
        }
    }

    [Fact]
    public unsafe void MdbxRatio2Percents_ReturnsString()
    {
        byte[] buffer = new byte[64];
        fixed (byte* bufPtr = buffer)
        {
            byte* result = MDBX.Native.Bindings.Settings.NativeMdbx.MdbxRatio2Percents(50, 100, bufPtr, (nuint)buffer.Length);
            Assert.True(result != null);
            string str = Marshal.PtrToStringUTF8((IntPtr)result)!;
            Assert.False(string.IsNullOrEmpty(str));
        }
    }
}
