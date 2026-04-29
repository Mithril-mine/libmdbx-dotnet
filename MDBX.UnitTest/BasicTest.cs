using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

using Xunit;

namespace MDBX.UnitTest;

public class BasicTest
{
    private static readonly string dataBasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx-basic-tests-1");

    private static readonly EnvironmentFlag EnvironmentFlags = EnvironmentFlag.NoSync | EnvironmentFlag.WriteMap | EnvironmentFlag.Exclusive;

    public BasicTest() => CheckDataBasePath();

    [Fact(DisplayName = "Mdbx combined")]
    public void MdbxCombined()
    {
        const int totalEntries = 1_000_000;

        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment
            .SetMaxDatabases(1)
            .Open(dataBasePath, EnvironmentFlags, Convert.ToInt32("666", 8));

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase("combined", DatabaseOption.IntegerKey | DatabaseOption.Create);

            var itemsAdded = 0;

            for (int i = 0; i < totalEntries; i++)
            {
                db.Put(i, $"entry {i}");
                
                itemsAdded++;             
            }

            transaction.Commit();

            Assert.Equal(totalEntries, itemsAdded);
        }

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadOnly))
        {
            var db = transaction.OpenDatabase("combined", DatabaseOption.IntegerKey);

            for (int i = 0; i < totalEntries; i++)
            {
                string res = db.Get<int, string>(i);

                Assert.Equal($"entry {i}", res);

            }

            transaction.Reset();
        }

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase("combined", DatabaseOption.IntegerKey);


            for (int i = 0; i < totalEntries; i++)
            {
                var res = db.Del(i);
                Assert.True(res);
            }

            db.Drop();
            
        }

        mdbxEnvironment.Close();

        
    }
 
    private static void CheckDataBasePath()
    {
        if (!Directory.Exists(dataBasePath))
            Directory.CreateDirectory(dataBasePath);
    }
}
