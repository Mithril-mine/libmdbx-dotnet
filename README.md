# libmdbx-dotnet

.NET bindings for [libmdbx](https://github.com/Mithril-mine/libmdbx) — an extremely
fast, compact, powerful, embeddable, transactional key-value storage engine, and the
successor to [LMDB](https://www.symas.com/lmdb) (Lightning Memory-Mapped Database).

## Features

- Full C# wrapper around the core libmdbx C API
- Environment, transaction, database and cursor management
- Read-write and read-only transactions
- Cursor-based iteration (forward, backward, seek)
- Named (and unnamed) databases within a single environment
- Modern .NET 8+ API using `Span<byte>` for zero-copy key/value access
- Bundled native binaries for Linux x64 (`runtimes/linux-x64/native/libmdbx.so`)

## Quick start

```csharp
using Libmdbx;

// Open (or create) an environment
using var env = new MdbxEnvironment();
env.SetMaxDatabases(10);
env.Open("/path/to/my/db");

// Write some data
using (var txn = env.BeginTransaction())
{
    var db = txn.OpenDatabase(flags: DatabaseFlags.Create);
    db.Put("hello"u8, "world"u8);
    txn.Commit();
}

// Read it back
using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
{
    var db = txn.OpenDatabase();
    byte[]? value = db.Get("hello"u8);
    Console.WriteLine(System.Text.Encoding.UTF8.GetString(value!)); // world
}
```

## Cursor iteration

```csharp
using (var txn = env.BeginTransaction(TransactionFlags.ReadOnly))
{
    var db = txn.OpenDatabase();
    using var cursor = db.OpenCursor();

    byte[]? key = null, value = null;
    bool found = cursor.Get(ref key, ref value, CursorOp.First);
    while (found)
    {
        Console.WriteLine($"{System.Text.Encoding.UTF8.GetString(key!)} = {System.Text.Encoding.UTF8.GetString(value!)}");
        found = cursor.Get(ref key, ref value, CursorOp.Next);
    }
}
```

## Building

Requirements:
- .NET 8 SDK
- `libmdbx` native library for your platform (a pre-built Linux x64 binary is included)

```bash
dotnet build
dotnet test
```

To rebuild the native library from source:

```bash
# Clone libmdbx
git clone https://github.com/Mithril-mine/libmdbx /tmp/libmdbx-src
cd /tmp/libmdbx-src
gcc -shared -fPIC -O2 -o libmdbx.so mdbx.c
cp libmdbx.so /path/to/runtimes/linux-x64/native/
```

## Licence

Apache 2.0 — see [LICENSE](LICENSE).
