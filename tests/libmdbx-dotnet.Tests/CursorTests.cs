using Xunit;

namespace Libmdbx.Tests
{
    /// <summary>Tests for <see cref="MdbxCursor"/>.</summary>
    public class CursorTests : DatabaseTestBase
    {
        private void PopulateDatabase(MdbxEnvironment env, int count = 5)
        {
            using var txn = env.BeginTransaction();
            var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
            for (int i = 1; i <= count; i++)
                db.Put(Bytes($"key{i:D2}"), Bytes($"val{i:D2}"));
            txn.Commit();
        }

        [Fact]
        public void IterateAllKeysForward()
        {
            using var env = OpenEnvironment();
            PopulateDatabase(env, 5);

            using var txn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var db = txn.OpenDatabase();
            using var cursor = db.OpenCursor();

            var keys = new List<string>();
            byte[]? key = null;
            byte[]? value = null;

            bool found = cursor.Get(ref key, ref value, CursorOp.First);
            while (found)
            {
                keys.Add(Str(key));
                found = cursor.Get(ref key, ref value, CursorOp.Next);
            }

            Assert.Equal(5, keys.Count);
            Assert.Equal("key01", keys[0]);
            Assert.Equal("key05", keys[4]);
        }

        [Fact]
        public void IterateAllKeysBackward()
        {
            using var env = OpenEnvironment();
            PopulateDatabase(env, 4);

            using var txn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var db = txn.OpenDatabase();
            using var cursor = db.OpenCursor();

            var keys = new List<string>();
            byte[]? key = null;
            byte[]? value = null;

            bool found = cursor.Get(ref key, ref value, CursorOp.Last);
            while (found)
            {
                keys.Add(Str(key));
                found = cursor.Get(ref key, ref value, CursorOp.Prev);
            }

            Assert.Equal(4, keys.Count);
            Assert.Equal("key04", keys[0]);
            Assert.Equal("key01", keys[3]);
        }

        [Fact]
        public void SetPositionsByKey()
        {
            using var env = OpenEnvironment();
            PopulateDatabase(env, 5);

            using var txn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var db = txn.OpenDatabase();
            using var cursor = db.OpenCursor();

            byte[]? key = Bytes("key03");
            byte[]? value = null;
            bool found = cursor.Get(ref key, ref value, CursorOp.Set);

            Assert.True(found);
            Assert.Equal("val03", Str(value));
        }

        [Fact]
        public void SetRangeFindsNearestKey()
        {
            using var env = OpenEnvironment();

            // Insert only keys 01, 03, 05
            using (var txn = env.BeginTransaction())
            {
                var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
                db.Put(Bytes("key01"), Bytes("val01"));
                db.Put(Bytes("key03"), Bytes("val03"));
                db.Put(Bytes("key05"), Bytes("val05"));
                txn.Commit();
            }

            using var rtxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var rdb = rtxn.OpenDatabase();
            using var cursor = rdb.OpenCursor();

            // SetRange on "key02" should position at "key03" (the next one)
            byte[]? key = Bytes("key02");
            byte[]? value = null;
            bool found = cursor.Get(ref key, ref value, CursorOp.SetRange);

            Assert.True(found);
            Assert.Equal("key03", Str(key));
            Assert.Equal("val03", Str(value));
        }

        [Fact]
        public void CursorPutAndDelete()
        {
            using var env = OpenEnvironment();
            PopulateDatabase(env, 3);

            using var txn = env.BeginTransaction();
            var db = txn.OpenDatabase();
            using var cursor = db.OpenCursor();

            // Position on key02
            byte[]? key = Bytes("key02");
            byte[]? value = null;
            cursor.Get(ref key, ref value, CursorOp.Set);

            // Delete via cursor
            cursor.Delete();
            txn.Commit();

            // Verify key02 is gone
            using var vtxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var vdb = vtxn.OpenDatabase();
            Assert.Null(vdb.Get(Bytes("key02")));
            Assert.NotNull(vdb.Get(Bytes("key01")));
            Assert.NotNull(vdb.Get(Bytes("key03")));
        }

        [Fact]
        public void GetCurrentAfterSetReturnsCorrectPair()
        {
            using var env = OpenEnvironment();
            PopulateDatabase(env, 5);

            using var txn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var db = txn.OpenDatabase();
            using var cursor = db.OpenCursor();

            byte[]? key = Bytes("key04");
            byte[]? value = null;
            cursor.Get(ref key, ref value, CursorOp.Set);

            // Re-read using GetCurrent
            cursor.Get(ref key, ref value, CursorOp.GetCurrent);
            Assert.Equal("key04", Str(key));
            Assert.Equal("val04", Str(value));
        }

        [Fact]
        public void EmptyDatabaseFirstOpReturnsFalse()
        {
            using var env = OpenEnvironment();

            using (var txn = env.BeginTransaction())
            {
                txn.OpenDatabase(flags: DatabaseFlags.Create);
                txn.Commit();
            }

            using var rtxn = env.BeginTransaction(TransactionFlags.ReadOnly);
            var rdb = rtxn.OpenDatabase();
            using var cursor = rdb.OpenCursor();

            byte[]? key = null;
            byte[]? value = null;
            bool found = cursor.Get(ref key, ref value, CursorOp.First);
            Assert.False(found);
        }
    }
}
