using MDBX.Native.Types;
using System.Runtime.InteropServices;
using Xunit;

namespace MDBX.UnitTest.Stat;

/// <summary>
/// Тесты функций статистики libmdbx.
/// </summary>
public class LocalStructTests
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct LocalMdbxStat
    {
        public nuint MsPsize;
        public nuint MsDepth;
        public nuint MsBranchPages;
        public nuint MsLeafPages;
        public nuint MsOverflowPages;
        public nuint MsEntries;
    }

    [Fact]
    public unsafe void CompareStructSizes()
    {
        Console.WriteLine($"sizeof(MdbxStat) = {sizeof(MdbxStat)}");
        Console.WriteLine($"sizeof(LocalMdbxStat) = {sizeof(LocalMdbxStat)}");
        Assert.Equal(sizeof(LocalMdbxStat), sizeof(MdbxStat));
    }
}
