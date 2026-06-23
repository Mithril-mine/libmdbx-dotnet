#nullable enable
using System.IO;
using System.Text;
using Xunit;
using MDBX;
using MDBX.Native.Types;

namespace MDBX.UnitTest.Examples;

/// <summary>
/// Коллекция для последовательного выполнения тестов примеров.
/// </summary>
[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialExamplesCollection : ICollectionFixture<SequentialExamplesCollection>
{
}

/// <summary>
/// Примеры использования высокоуровневых CRUD операций libmdbx.
/// </summary>
public class CrudExamples
{
    private const string ExampleDbPath = "example_crud.mdbx";

    [Fact]
    public void Example1_BasicPutAndGet()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("users");
        using var txn = env.BeginTransaction();

        var userId = Encoding.UTF8.GetBytes("user:1");
        var userData = Encoding.UTF8.GetBytes("{\"name\":\"Alice\",\"age\":30}");

        db.Put(txn, userId, userData);
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var result = db.Get(readTxn, userId);

        Assert.NotNull(result);
        Assert.Equal(userData, result);

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example2_StringApiUsage()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("products");
        using var txn = env.BeginTransaction();

        db.Put(txn, "product:1", "Laptop");
        db.Put(txn, "product:2", "Mouse");
        db.Put(txn, "product:3", "Keyboard");

        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var product = db.Get(readTxn, "product:2");

        Assert.NotNull(product);
        Assert.Equal("Mouse", Encoding.UTF8.GetString(product));

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example3_UpdateAndDelete()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("tasks");
        using var txn = env.BeginTransaction();

        var taskKey = Encoding.UTF8.GetBytes("task:1");
        var initialValue = Encoding.UTF8.GetBytes("pending");
        var updatedValue = Encoding.UTF8.GetBytes("completed");

        db.Put(txn, taskKey, initialValue);
        db.Put(txn, taskKey, updatedValue);

        var existsBeforeDelete = db.Delete(txn, taskKey);
        Assert.True(existsBeforeDelete);

        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var afterDelete = db.Get(readTxn, taskKey);
        Assert.Null(afterDelete);

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example4_MultipleDatabases()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var usersDb = env.OpenDatabaseEx("users");
        using var logsDb = env.OpenDatabaseEx("logs");
        using var txn = env.BeginTransaction();

        usersDb.Put(txn, "user:1", Encoding.UTF8.GetBytes("Alice"));
        usersDb.Put(txn, "user:2", Encoding.UTF8.GetBytes("Bob"));

        logsDb.Put(txn, "log:1", Encoding.UTF8.GetBytes("User Alice created"));
        logsDb.Put(txn, "log:2", Encoding.UTF8.GetBytes("User Bob created"));

        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var user1 = usersDb.Get(readTxn, "user:1");
        var log1 = logsDb.Get(readTxn, "log:1");

        Assert.NotNull(user1);
        Assert.Equal("Alice", Encoding.UTF8.GetString(user1));
        Assert.NotNull(log1);
        Assert.Equal("User Alice created", Encoding.UTF8.GetString(log1));

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example5_SeparateWriteAndReadTransactions()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("cache");

        using (var writeTxn = env.BeginTransaction())
        {
            db.Put(writeTxn, "key:1", Encoding.UTF8.GetBytes("value:1"));
            db.Put(writeTxn, "key:2", Encoding.UTF8.GetBytes("value:2"));
            writeTxn.Commit();
        }

        using (var readTxn = env.BeginTransaction())
        {
            var value1 = db.Get(readTxn, "key:1");
            var value2 = db.Get(readTxn, "key:2");
            var missing = db.Get(readTxn, "key:missing");

            Assert.NotNull(value1);
            Assert.Equal("value:1", Encoding.UTF8.GetString(value1));
            Assert.NotNull(value2);
            Assert.Equal("value:2", Encoding.UTF8.GetString(value2));
            Assert.Null(missing);
        }

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example6_CheckKeyExistenceBeforeDelete()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("items");
        using var txn = env.BeginTransaction();

        db.Put(txn, "item:1", Encoding.UTF8.GetBytes("Widget"));

        bool existsBeforeDelete = db.Delete(txn, "item:1");
        Assert.True(existsBeforeDelete);

        bool existsAfterDelete = db.Delete(txn, "item:1");
        Assert.False(existsAfterDelete);

        txn.Commit();

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example7_PutFlagsUsage()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("flags_demo");
        using var txn = env.BeginTransaction();

        var key = Encoding.UTF8.GetBytes("counter");

        db.Put(txn, key, Encoding.UTF8.GetBytes("1"));

        db.Put(txn, key, Encoding.UTF8.GetBytes("2"), MdbxPutFlags.MDBX_CURRENT);

        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var result = db.Get(readTxn, key);
        Assert.NotNull(result);
        Assert.Equal("2", Encoding.UTF8.GetString(result));

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }

    [Fact]
    public void Example8_UsingDefaultDatabase()
    {
        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);

        using var env = MdbxEnvironment.Open(ExampleDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx();
        using var txn = env.BeginTransaction();

        db.Put(txn, "default_key", "default_value");
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        var result = db.Get(readTxn, "default_key");

        Assert.NotNull(result);
        Assert.Equal("default_value", Encoding.UTF8.GetString(result));

        if (File.Exists(ExampleDbPath))
            File.Delete(ExampleDbPath);
    }
}

/// <summary>
/// Примеры использования курсоров libmdbx.
/// </summary>
[Collection("Sequential")]
public class CursorExamples
{
    private const string ExampleCursorDbPath = "example_cursor.mdbx";

    [Fact]
    public void Example1_BasicCursorNavigation()
    {
        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);

        using var env = MdbxEnvironment.Open(ExampleCursorDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("cursor_demo");
        using var txn = env.BeginTransaction();

        db.Put(txn, "a", "1");
        db.Put(txn, "b", "2");
        db.Put(txn, "c", "3");
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        using var cursor = readTxn.OpenCursor(db);

        var first = cursor.GetFirst(out var firstKey);
        Assert.NotNull(first);
        Assert.NotNull(firstKey);
        Assert.Equal("1", Encoding.UTF8.GetString(first));
        Assert.Equal("a", Encoding.UTF8.GetString(firstKey));

        var next = cursor.GetNext(out var nextKey);
        Assert.NotNull(next);
        Assert.Equal("2", Encoding.UTF8.GetString(next));
        Assert.Equal("b", Encoding.UTF8.GetString(nextKey));

        cursor.GetLast(out var lastKey);
        Assert.NotNull(cursor.GetCurrent(out var currentKey));
        Assert.Equal("c", Encoding.UTF8.GetString(currentKey));
        Assert.Equal("c", Encoding.UTF8.GetString(lastKey));

        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);
    }

    [Fact]
    public void Example2_CursorPutAndDelete()
    {
        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);

        using var env = MdbxEnvironment.Open(ExampleCursorDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("cursor_put_del");
        using var txn = env.BeginTransaction();

        db.Put(txn, "key1", "value1");
        txn.Commit();

        using var writeTxn = env.BeginTransaction();
        using var cursor = writeTxn.OpenCursor(db);

        cursor.GetFirst(out var key);
        cursor.Put("key1", "updated_value");

        var current = cursor.GetCurrent(out var currentKey);
        Assert.NotNull(current);
        Assert.Equal("updated_value", Encoding.UTF8.GetString(current));
        Assert.Equal("key1", Encoding.UTF8.GetString(currentKey));

        bool deleted = cursor.Delete();
        Assert.True(deleted);

        writeTxn.Commit();

        using var readTxn = env.BeginTransaction();
        var afterDelete = db.Get(readTxn, "key1");
        Assert.Null(afterDelete);

        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);
    }

    [Fact]
    public void Example3_CursorCountAndPosition()
    {
        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);

        using var env = MdbxEnvironment.Open(ExampleCursorDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("cursor_count");
        using var txn = env.BeginTransaction();

        for (int i = 0; i < 5; i++)
            db.Put(txn, $"k{i}", $"v{i}");

        txn.Commit();

        using var readTxn = env.BeginTransaction();
        using var cursor = readTxn.OpenCursor(db);

        cursor.GetFirst(out _);
        nuint count = cursor.Count();
        Assert.Equal(1u, count);

        Assert.False(cursor.IsEof());
        Assert.True(cursor.IsOnFirst());

        cursor.GetLast(out _);
        Assert.True(cursor.IsOnLast());

        cursor.GetNext(out _);
        Assert.True(cursor.IsEof());

        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);
    }

    [Fact]
    public void Example4_DuplicateKeysIteration()
    {
        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);

        using var env = MdbxEnvironment.Open(ExampleCursorDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("dup_table", MdbxDbFlags.MDBX_DUPSORT | MdbxDbFlags.MDBX_CREATE);
        using var txn = env.BeginTransaction();

        db.Put(txn, "fruit", "apple");
        db.Put(txn, "fruit", "banana");
        db.Put(txn, "fruit", "cherry");
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        using var cursor = readTxn.OpenCursor(db);

        cursor.GetFirst(out var key);
        Assert.Equal("fruit", Encoding.UTF8.GetString(key!));

        int dupCount = 0;
        string[] values = new string[3];

        values[dupCount++] = Encoding.UTF8.GetString(cursor.GetCurrent(out _)!);

        while (!cursor.IsEof() && dupCount < 3)
        {
            var nextVal = cursor.GetNextDup(out _);
            if (nextVal == null)
                break;
            values[dupCount++] = Encoding.UTF8.GetString(nextVal);
        }

        Assert.Equal(3, dupCount);

        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);
    }

    [Fact]
    public void Example5_RenewCursorForNewTransaction()
    {
        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);

        using var env = MdbxEnvironment.Open(ExampleCursorDbPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("cursor_renew");

        using (var writeTxn = env.BeginTransaction())
        {
            db.Put(writeTxn, "x", "10");
            db.Put(writeTxn, "y", "20");
            writeTxn.Commit();
        }

        using var txn1 = env.BeginTransaction();
        using var cursor = txn1.OpenCursor(db);

        cursor.GetFirst(out var key1);
        Assert.Equal("x", Encoding.UTF8.GetString(key1!));
        cursor.Renew(txn1);

        txn1.Commit();

        using var txn2 = env.BeginTransaction();
        cursor.Renew(txn2);

        cursor.GetLast(out var key2);
        Assert.Equal("y", Encoding.UTF8.GetString(key2!));

        var current = cursor.GetCurrent(out var currentKey);
        Assert.NotNull(current);
        Assert.Equal("20", Encoding.UTF8.GetString(current));
        Assert.Equal("y", Encoding.UTF8.GetString(currentKey));

        txn2.Commit();

        if (File.Exists(ExampleCursorDbPath))
            File.Delete(ExampleCursorDbPath);
    }
}
