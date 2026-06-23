using System.Text;
using Xunit;
using MDBX;

namespace MDBX.UnitTest.Query;

/// <summary>
/// Тесты высокоуровневой LINQ-абстракции над курсором (MdbxQuery / MdbxDatabase.Find).
/// </summary>
[Collection("Sequential")]
public class MdbxQueryTests
{
    private const string DbPath = "test_query.mdbx";

    private static void Cleanup()
    {
        if (System.IO.File.Exists(DbPath))
            System.IO.File.Delete(DbPath);
    }

    [Fact]
    public void Find_WithValueSelector_Where_FiltersByValue()
    {
        Cleanup();
        using var env = MdbxEnvironment.Open(DbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("items");
        using (var txn = env.BeginTransaction())
        {
            db.Put(txn, "k1", "Some Value");
            db.Put(txn, "k2", "Other Value");
            db.Put(txn, "k3", "Some Value");
            txn.Commit();
        }

        using var readTxn = env.BeginTransaction();
        var matches = db.Find(readTxn, Encoding.UTF8.GetString)
                        .Where(x => x == "Some Value")
                        .ToList();

        Assert.Equal(2, matches.Count);
        Assert.All(matches, v => Assert.Equal("Some Value", v));

        Cleanup();
    }

    [Fact]
    public void Find_Entries_ProjectsKeyAndValue()
    {
        Cleanup();
        using var env = MdbxEnvironment.Open(DbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("entries");
        using (var txn = env.BeginTransaction())
        {
            db.Put(txn, "a", "1");
            db.Put(txn, "b", "2");
            txn.Commit();
        }

        using var readTxn = env.BeginTransaction();
        var pairs = db.Find(readTxn, Encoding.UTF8.GetString, Encoding.UTF8.GetString)
                      .OrderBy(p => p.Key)
                      .ToList();

        Assert.Equal(2, pairs.Count);
        Assert.Equal(new KeyValuePair<string, string>("a", "1"), pairs[0]);
        Assert.Equal(new KeyValuePair<string, string>("b", "2"), pairs[1]);

        Cleanup();
    }

    [Fact]
    public void FindValues_ReturnsRawBytes()
    {
        Cleanup();
        using var env = MdbxEnvironment.Open(DbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("vals");
        using (var txn = env.BeginTransaction())
        {
            db.Put(txn, "x", "hello");
            db.Put(txn, "y", "world");
            txn.Commit();
        }

        using var readTxn = env.BeginTransaction();
        var values = db.FindValues(readTxn).ToList();

        Assert.Equal(2, values.Count);
        Assert.Contains(values, v => Encoding.UTF8.GetString(v) == "hello");
        Assert.Contains(values, v => Encoding.UTF8.GetString(v) == "world");

        Cleanup();
    }

    [Fact]
    public void Find_CursorQuery_DeferredEnumeration()
    {
        Cleanup();
        using var env = MdbxEnvironment.Open(DbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("deferred");
        using (var txn = env.BeginTransaction())
        {
            for (int i = 0; i < 10; i++)
                db.Put(txn, $"k{i}", $"v{i}");
            txn.Commit();
        }

        using var readTxn = env.BeginTransaction();
        var query = db.Find(readTxn);
        var firstThree = query.Take(3).ToList();

        Assert.Equal(3, firstThree.Count);
        Assert.Equal("v0", Encoding.UTF8.GetString(firstThree[0].Value));
        Assert.Equal("v2", Encoding.UTF8.GetString(firstThree[2].Value));

        Cleanup();
    }
}
