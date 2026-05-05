using System.Text;
using Xunit;

namespace Libmdbx.Tests
{
    /// <summary>Basic CRUD tests for <see cref="MdbxDatabase"/>.</summary>
    public class BasicTests : DatabaseTestBase
    {
        [Fact]
        public void CanOpenEnvironment()
        {
            using var env = OpenEnvironment();
            Assert.NotNull(env);
        }

        [Fact]
        public void PutAndGetBytes()
        {
            using var env = OpenEnvironment();

            // Write
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("hello"), Bytes("world"));
                txn.Commit();
            }

            // Read
            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                byte[]? result = db.Get(Bytes("hello"));
                Assert.Equal("world", Str(result));
            }
        }

        [Fact]
        public void GetReturnsNullForMissingKey()
        {
            using var env = OpenEnvironment();

            using var txn = env.BeginTransaction();
            var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
            txn.Commit();

            using var rtxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var rdb = rtxn.OpenDatabase();
            Assert.Null(rdb.Get(Bytes("nonexistent")));
        }

        [Fact]
        public void DeleteKey()
        {
            using var env = OpenEnvironment();

            // Insert
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("key1"), Bytes("value1"));
                txn.Commit();
            }

            // Delete
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase();
                bool deleted = db.Delete(Bytes("key1"));
                Assert.True(deleted);
                txn.Commit();
            }

            // Verify
            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.Null(db.Get(Bytes("key1")));
            }
        }

        [Fact]
        public void DeleteNonExistentKeyReturnsFalse()
        {
            using var env = OpenEnvironment();

            using var txn = env.BeginTransaction();
            var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
            bool deleted = db.Delete(Bytes("ghost"));
            Assert.False(deleted);
        }

        [Fact]
        public void OverwriteValue()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("k"), Bytes("v1"));
                db.Put(Bytes("k"), Bytes("v2"));   // overwrite
                txn.Commit();
            }

            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.Equal("v2", Str(db.Get(Bytes("k"))));
            }
        }

        [Fact]
        public void NoOverwriteFlagPreservesExistingValue()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("k"), Bytes("original"));
                var ex = Assert.Throws<MdbxException>(
                    () => db.Put(Bytes("k"), Bytes("new"), PutFlags.NoOverwrite));
                Assert.Equal(MdbxException.KeyExist, ex.ErrorCode);
            }
        }

        [Fact]
        public void ContainsKeyWorks()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("present"), Bytes("yes"));
                txn.Commit();
            }

            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.True(db.ContainsKey(Bytes("present")));
                Assert.False(db.ContainsKey(Bytes("absent")));
            }
        }

        [Fact]
        public void MultipleNamedDatabases()
        {
            using var env = OpenEnvironment(maxDbs: 5);

            using (var txn = env.BeginTransaction())
            {
                var db1 = txn.OpenDatabase("db1", DatabaseFlags.Create);
                var db2 = txn.OpenDatabase("db2", DatabaseFlags.Create);
                db1.Put(Bytes("key"), Bytes("from-db1"));
                db2.Put(Bytes("key"), Bytes("from-db2"));
                txn.Commit();
            }

            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db1 = txn.OpenDatabase("db1");
                var db2 = txn.OpenDatabase("db2");
                Assert.Equal("from-db1", Str(db1.Get(Bytes("key"))));
                Assert.Equal("from-db2", Str(db2.Get(Bytes("key"))));
            }
        }

        [Fact]
        public void EmptyDatabaseClearsAllEntries()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("a"), Bytes("1"));
                db.Put(Bytes("b"), Bytes("2"));
                db.Empty();
                txn.Commit();
            }

            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.Null(db.Get(Bytes("a")));
                Assert.Null(db.Get(Bytes("b")));
            }
        }

        [Fact]
        public void EnvironmentStatReturnsValidInfo()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("x"), Bytes("y"));
                txn.Commit();
            }

            var stat = env.Stat();
            Assert.True(stat.PageSize > 0);
            Assert.True(stat.Entries >= 0);
        }
    }
}
