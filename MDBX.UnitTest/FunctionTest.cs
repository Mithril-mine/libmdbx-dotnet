using MDBX.Options;
using System.IO;
using System.Reflection;

using Xunit;


namespace MDBX.UnitTest
{
    public class FunctionTest
    {
        [Fact(DisplayName = "mdbx_env_stat")]
        public void Test1()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoStickyThreads, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

                var stat = env.Stat();

                env.Close();
            }
        }

        [Fact(DisplayName = "mdbx_env_info_ex")]
        public void Test2()
        {
            const string dataBaseName = "mdbx_env_info_ex";

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dataBaseName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using MdbxEnvironment env = new();

            env
                .SetMaxDatabases(10)
                .Open(path, EnvironmentFlag.EnvDefaults, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite)

                ;


            var stat = env.InfoEx();

            env.Close();
        }



        [Fact(DisplayName = "mdbx_env_set_flags / mdbx_env_get_flags")]
        public void Test3()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoStickyThreads, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

                EnvironmentFlag flags = env.GetFlags();

                env.SetFlags(EnvironmentFlag.NoSync);

                env.Close();
            }
        }


        [Fact(DisplayName = "mdbx_env_set_maxreaders / mdbx_env_set_mapsize")]
        public void Test4()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.SetMapSize(1024 * 1024 * 10)
                    .SetMaxDatabases(2)
                    .SetMaxReaders(100)
                    .Open(path, EnvironmentFlag.NoStickyThreads, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

                env.Close();
            }
        }

        [Fact(DisplayName = "mdbx_env_get_maxkeysize / mdbx_env_get_maxreaders")]
        public void Test5()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoStickyThreads, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

                int maxKeySize = env.GetMaxKeySize();
                int maxReaders = env.GetMaxReaders();

                env.Close();
            }
        }

        [Fact(DisplayName = "mdbx_txn_id")]
        public void Test6()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoStickyThreads, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    ulong snapshotID = tran.GetID();
                }

                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    ulong txnID = tran.GetID();
                }

                env.Close();
            }
        }
    }
}
