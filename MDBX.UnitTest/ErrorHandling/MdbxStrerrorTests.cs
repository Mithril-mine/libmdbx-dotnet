using System.Runtime.InteropServices;
using Xunit;

namespace MDBX.UnitTest.ErrorHandling;

/// <summary>
/// Тесты функций обработки ошибок и отладки.
/// </summary>
public class NativeMdbxTests
{
    [Fact]
    public unsafe void MdbxStrerror_ReturnsErrorMessage()
    {
        byte* errorMsg = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerror(0);
        Assert.True(errorMsg != null);
        string msg = Marshal.PtrToStringUTF8((IntPtr)errorMsg)!;
        Assert.False(string.IsNullOrEmpty(msg));
    }

    [Fact]
    public unsafe void MdbxStrerrorR_ReturnsErrorMessage()
    {
        byte[] buffer = new byte[256];
        fixed (byte* bufPtr = buffer)
        {
            byte* result = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerrorR(0, bufPtr, (nuint)buffer.Length);
            Assert.True(result != null);
            string msg = Marshal.PtrToStringUTF8((IntPtr)result)!;
            Assert.False(string.IsNullOrEmpty(msg));
        }
    }

    [Fact]
    public unsafe void MdbxLiberr2Str_ReturnsLibMdbxErrorMessage()
    {
        byte* errorMsg = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxLiberr2Str(0);
        Assert.True(errorMsg != null);
        string msg = Marshal.PtrToStringUTF8((IntPtr)errorMsg)!;
        Assert.False(string.IsNullOrEmpty(msg));
    }

    [Fact]
    public unsafe void MdbxStrerror_And_MdbxLiberr2Str_AreConsistent()
    {
        byte* str1 = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerror(0);
        byte* str2 = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxLiberr2Str(0);
        Assert.True(str1 != null);
        Assert.True(str2 != null);
        string msg1 = Marshal.PtrToStringUTF8((IntPtr)str1)!;
        string msg2 = Marshal.PtrToStringUTF8((IntPtr)str2)!;
        Assert.False(string.IsNullOrEmpty(msg1));
        Assert.False(string.IsNullOrEmpty(msg2));
    }

    [Fact]
    public unsafe void MdbxStrerrorAnsi2Oem_ReturnsErrorMessage()
    {
        byte* errorMsg = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerrorAnsi2Oem(0);
        Assert.True(errorMsg != null);
        string msg = Marshal.PtrToStringUTF8((IntPtr)errorMsg)!;
        Assert.False(string.IsNullOrEmpty(msg));
    }

    [Fact]
    public unsafe void MdbxStrerrorRAnsi2Oem_ReturnsErrorMessage()
    {
        byte[] buffer = new byte[256];
        fixed (byte* bufPtr = buffer)
        {
            byte* result = MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxStrerrorRAnsi2Oem(0, bufPtr, (nuint)buffer.Length);
            Assert.True(result != null);
            string msg = Marshal.PtrToStringUTF8((IntPtr)result)!;
            Assert.False(string.IsNullOrEmpty(msg));
        }
    }

    [Fact]
    public unsafe void MdbxSetPanic_DoesNotThrow()
    {
        MDBX.Native.Bindings.ErrorHandling.NativeMdbx.MdbxSetPanic(null);
        Assert.True(true);
    }
}
