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

    private static readonly string countriesDataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "countries.json");
    
    private static readonly CountryModel[] countries = GetCountries();

    private static readonly EnvironmentFlag EnvironmentFlags = EnvironmentFlag.NoSync | EnvironmentFlag.WriteMap | EnvironmentFlag.Exclusive;

    public BasicTest() => CheckDataBasePath();

    private void MdbxPut()
    {
        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment.SetMaxDatabases(1).Open(dataBasePath, EnvironmentFlags, Convert.ToInt32("666", 8));

        var itemsAdded = 0;

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase("countries", DatabaseOption.Create);

            foreach (var country in countries)
            {
                db.Put(country.code, country.name);
                itemsAdded++;
            }

            transaction.Commit();

            Assert.Equal(countries.Length, itemsAdded);
        }

        mdbxEnvironment.Close();
    }

    [Fact(DisplayName = "Mdbx put")]
    public void MdbxPut1()
    {

        MdbxPut();

    }

    [Fact(DisplayName = "Mdbx get")]
    public void MdbxGet1()
    {
        MdbxPut();

        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment.SetMaxDatabases(1).Open(dataBasePath, EnvironmentFlags, Convert.ToInt32("666", 8));

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadOnly))
        {
            var db = transaction.OpenDatabase("countries");

            foreach (var country in countries)
            {
                string result = db.Get<string, string>(country.code);

                Assert.Equal(result, country.name);
            }

            transaction.Reset();
        }

        mdbxEnvironment.Close();

    }

    [Fact(DisplayName = "Mdbx del")]
    public void MdbxDelete()
    {

        using MdbxEnvironment mdbxEnvironment = new();

        mdbxEnvironment.SetMaxDatabases(1).Open(dataBasePath, EnvironmentFlags, Convert.ToInt32("666", 8));

        using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite))
        {
            var db = transaction.OpenDatabase("countries");

            foreach (var country in countries)
            {
                var res = db.Del(country.code);
                Assert.True(res);
            }

            transaction.Commit();
        }

        mdbxEnvironment.Close();

    }

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

    private static CountryModel[] GetCountries()
    {
        var rawWalue = File.ReadAllText(countriesDataPath);
        return JsonConvert.DeserializeObject<CountryModel[]>(rawWalue, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }


    private static void CheckDataBasePath()
    {
        if (!Directory.Exists(dataBasePath))
            Directory.CreateDirectory(dataBasePath);
    }
}
