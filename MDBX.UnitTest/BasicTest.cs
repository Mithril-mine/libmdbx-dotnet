using MDBX.Options;
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

    private static readonly string dataBasePathForSingle = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx-basic-tests-single-txn");

    private static readonly EnvironmentFlag EnvironmentFlags = EnvironmentFlag.NoSync | EnvironmentFlag.WriteMap | EnvironmentFlag.Exclusive;

    private static readonly string countriesDataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "countries.json");

    private static readonly CountryModel[] countries = GetCountries();

    private static readonly long totalEntries = 2_000_000;

    public BasicTest() => CheckDataBasePath();

    [Fact(DisplayName = "Mdbx combined integer key")]
    public void MdbxCombinedIntegerKey()
    {
        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment
            .SetMaxDatabases(1)
            .Open(dataBasePath, EnvironmentFlags, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

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

            transaction.Commit();

        }

        mdbxEnvironment.Close();

    }

    [Fact(DisplayName = "Mdbx single item at transaction")]
    public void SingleItemInTransaction()
    {
        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment
            .SetMaxDatabases(1)
            .Open(dataBasePathForSingle,
             EnvironmentFlag.WriteMap, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite)
            ;

        var dbName = Guid.NewGuid().ToString();

        const int totalEntriesForSingleTxn = 1_000;

        var itemsAdded = 0;

        for (long i = 0; i < totalEntriesForSingleTxn; i++)
        {
            using var transaction = mdbxEnvironment.BeginTransaction();

            var db = transaction.OpenDatabase(dbName, DatabaseOption.IntegerKey | DatabaseOption.Create);

            db.Put(i, $"entry {i}");

            transaction.Commit();
            transaction.Dispose();

            itemsAdded++;
        }

        Assert.Equal(totalEntriesForSingleTxn, itemsAdded);

        mdbxEnvironment.Close();
    }

    [Fact(DisplayName = "Mdbx combined json list")]
    public void MdbxCombinedJsonList()
    {
        const string dataBaseName = "countries";

        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment
            .SetMaxDatabases(1)
            .Open(dataBasePath, EnvironmentFlags, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase(dataBaseName, DatabaseOption.Create);

            var itemsAdded = 0;

            foreach (var country in GetCountries())
            {
                db.Put<string, string>(country.code, country.name);

                itemsAdded++;
            }

            transaction.Commit();

            Assert.Equal(countries.Length, itemsAdded);
        }

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadOnly))
        {
            var db = transaction.OpenDatabase(dataBaseName);

            foreach (var country in countries)
            {
                var res = db.Get<string, string>(country.code);

                Assert.Equal(country.name, res);
            }

            transaction.Reset();
        }

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase(dataBaseName);

            foreach (var country in countries)
            {
                var res = db.Del(country.code);
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

    private static CountryModel[] GetCountries()
    {
        var rawWalue = File.ReadAllText(countriesDataPath);
        return JsonConvert.DeserializeObject<CountryModel[]>(rawWalue, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}
