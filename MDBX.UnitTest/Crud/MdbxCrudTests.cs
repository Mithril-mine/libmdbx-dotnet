#nullable enable
using System.IO;
using Xunit;
using MDBX.Native.Types;

namespace MDBX.UnitTest.Crud;

/// <summary>
/// Тесты высокоуровневых CRUD операций libmdbx.
/// </summary>
public class MdbxCrudTests : IDisposable
{
    private readonly string _envPath;

    public MdbxCrudTests()
    {
        _envPath = Path.Combine(Path.GetTempPath(), $"mdbx_crud_test_{Guid.NewGuid():N}.mdbx");
    }

    [Fact]
    public void PutAndGet_ReturnsCorrectValue()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        byte[] key = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] value = System.Text.Encoding.UTF8.GetBytes("test_value");

        db.Put(txn, key, value);
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        byte[]? result = db.Get(readTxn, key);

        Assert.NotNull(result);
        Assert.Equal(value, result);
    }

    [Fact]
    public void Put_OverwritesExistingValue()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        byte[] key = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] oldValue = System.Text.Encoding.UTF8.GetBytes("old_value");
        byte[] newValue = System.Text.Encoding.UTF8.GetBytes("new_value");

        db.Put(txn, key, oldValue);
        db.Put(txn, key, newValue);
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        byte[]? result = db.Get(readTxn, key);

        Assert.NotNull(result);
        Assert.Equal(newValue, result);
    }

    [Fact]
    public void Get_ReturnsNullForMissingKey()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        byte[] key = System.Text.Encoding.UTF8.GetBytes("missing_key");
        byte[]? result = db.Get(txn, key);

        Assert.Null(result);
    }

    [Fact]
    public void Delete_RemovesExistingKey()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        byte[] key = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] value = System.Text.Encoding.UTF8.GetBytes("test_value");

        db.Put(txn, key, value);
        bool deleted = db.Delete(txn, key);
        Assert.True(deleted);
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        byte[]? result = db.Get(readTxn, key);
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ReturnsFalseForMissingKey()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        byte[] key = System.Text.Encoding.UTF8.GetBytes("missing_key");
        bool deleted = db.Delete(txn, key);

        Assert.False(deleted);
    }

    [Fact]
    public void PutWithStringKeyAndValue_WorksCorrectly()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        db.Put(txn, "string_key", "string_value");
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        byte[]? result = db.Get(readTxn, "string_key");

        Assert.NotNull(result);
        Assert.Equal("string_value", System.Text.Encoding.UTF8.GetString(result));
    }

    [Fact]
    public void DeleteWithStringKey_WorksCorrectly()
    {
        using var env = MdbxEnvironment.Open(_envPath, maxDatabases: 10);
        using var db = env.OpenDatabaseEx("test_db");
        using var txn = env.BeginTransaction();

        db.Put(txn, "string_key", "string_value");
        bool deleted = db.Delete(txn, "string_key");
        Assert.True(deleted);
        txn.Commit();

        using var readTxn = env.BeginTransaction();
        byte[]? result = db.Get(readTxn, "string_key");
        Assert.Null(result);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_envPath))
                File.Delete(_envPath);
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}
