using Xunit;

namespace Libmdbx.Tests
{
    /// <summary>Tests for transaction commit, abort, and nested behaviour.</summary>
    public class TransactionTests : DatabaseTestBase
    {
        [Fact]
        public void AbortedTransactionDoesNotPersistData()
        {
            using var env = OpenEnvironment();

            // First: create the database
            using (var setupTxn = env.BeginTransaction())
            {
                setupTxn.OpenDatabase(flags: DatabaseFlags.Create);
                setupTxn.Commit();
            }

            // Then: write and abort
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase();
                db.Put(Bytes("key"), Bytes("value"));
                txn.Abort();   // no commit → data must not appear
            }

            // Read: should not find the key
            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.Null(db.Get(Bytes("key")));
            }
        }

        [Fact]
        public void DisposeWithoutCommitAbortsTransaction()
        {
            using var env = OpenEnvironment();

            // Create DB
            using (var t = env.BeginTransaction())
            {
                t.OpenDatabase(flags: DatabaseFlags.Create);
                t.Commit();
            }

            // Write then dispose (no commit)
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase();
                db.Put(Bytes("k"), Bytes("v"));
                // txn.Dispose() is called by the using block – should abort
            }

            // Verify
            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                Assert.Null(db.Get(Bytes("k")));
            }
        }

        [Fact]
        public void CommitAfterCommitThrows()
        {
            using var env = OpenEnvironment();
            using var txn = env.BeginTransaction();
            txn.OpenDatabase(flags: DatabaseFlags.Create);
            txn.Commit();

            Assert.Throws<InvalidOperationException>(() => txn.Commit());
        }

        [Fact]
        public void TransactionIdIsPositive()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                txn.OpenDatabase(flags: DatabaseFlags.Create);
                txn.Commit();
            }

            using var rtxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            ulong id = rtxn.Id();
            Assert.True(id > 0, $"Expected positive transaction ID, got {id}");
        }

        [Fact]
        public void ReadOnlyTransactionCannotWrite()
        {
            using var env = OpenEnvironment();

            // Create the DB first
            using (var t = env.BeginTransaction())
            {
                t.OpenDatabase(flags: DatabaseFlags.Create);
                t.Commit();
            }

            using var roTxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var db = roTxn.OpenDatabase();
            Assert.Throws<MdbxException>(() =>
                db.Put(Bytes("k"), Bytes("v")));
        }

        [Fact]
        public void CommittedDataIsVisibleInSubsequentTransaction()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                for (int i = 0; i < 100; i++)
                    db.Put(Bytes($"key{i:D4}"), Bytes($"val{i:D4}"));
                txn.Commit();
            }

            using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
            {
                var db = txn.OpenDatabase();
                for (int i = 0; i < 100; i++)
                    Assert.Equal($"val{i:D4}", Str(db.Get(Bytes($"key{i:D4}"))));
            }
        }
    }
}
