using System;
using System.IO;
using System.Text;
using Xunit;

namespace Libmdbx.Tests
{
    /// <summary>
    /// Shared test infrastructure: creates a temporary database directory and
    /// tears it down after every test.
    /// </summary>
    public abstract class DatabaseTestBase : IDisposable
    {
        protected readonly string DbPath;

        protected DatabaseTestBase()
        {
            DbPath = Path.Combine(Path.GetTempPath(), "libmdbx_test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(DbPath);
        }

        /// <summary>Open a fresh environment configured for tests.</summary>
        protected MdbxEnvironment OpenEnvironment(uint maxDbs = 10)
        {
            var env = new MdbxEnvironment();
            env.SetMaxDatabases(maxDbs);
            env.Open(DbPath);
            return env;
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(DbPath))
                    Directory.Delete(DbPath, recursive: true);
            }
            catch
            {
                // best-effort cleanup
            }
        }

        // Convenience helpers
        protected static byte[] Bytes(string s) => Encoding.UTF8.GetBytes(s);
        protected static string Str(byte[]? b) =>
            b is null ? "<null>" : Encoding.UTF8.GetString(b);
    }
}
