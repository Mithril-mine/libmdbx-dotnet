# mdbx-dotnet

.NET bindings(dotnet) of [libmdbx](https://libmdbx.dqdkfa.ru/)

<img alt="pipeline status" src="http://git.amsoft.spb.ru/devops/deploy/libmdbx/badges/libmdbx-dotnet/pipeline.svg" />

libmdbx — это чрезвычайно быстрая, компактная, мощная встраиваемая транзакционная база данных «ключ-значение» под лицензией Apache 2.0. libmdbx обладает особым набором свойств и возможностей, ориентированных на создание уникальных легковесных решений.

★ Превосходит легендарную LMDB по надежности, возможностям и производительности 

★ Используется в сотнях открытых проектах, в том числе в Ethereum.

По сути, libmdbx — это глубоко переработанный и расширенный вариант легендарной базы данных с отображением в памяти Lightning Memory-Mapped Database. libmdbx унаследовала все преимущества LMDB, но при этом решила ряд проблем и добавила множество улучшений.

### License

This project is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).

The `libmdbx` library is not shipped with this assembly. And the assembly will load `libmdbx` from the location below according to your platform and OS.
```


## Использование

### Запись, чтение, удаление.

```csharp

using MDBX;
using MDBX.Options;

// окружение.
using MdbxEnvironment mdbxEnvironment = new();

// настройка окружения.
mdbxEnvironment
  .SetMaxDatabases(1)
  .Open(
    "path/to/db", 
    EnvironmentFlag.Exclusive, UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite
);

// Запись.
using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite)) {
  var db = transaction.OpenDatabase("dataBaseName", DatabaseOption.IntegerKey | DatabaseOption.Create);

  for (int i = 0; i < 1_000_000; i++) {
    db.Put(i, $"entry {i}");
  }

  transaction.Commit();
}

// Чтение.
using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite)) {
  for (int i = 0; i < 1_000_000; i++) {
    var result = db.Get<int, string>(i);
  }
}

// Удаление.
using (var transaction = mdbxEnvironment.BeginTransaction(TransactionOption.ReadWrite)) {
  for (int i = 0; i < 1_000_000; i++) {
    var result = db.Del<int, string>(i);
  }

  transaction.Commit();
}

mdbxEnvironment.Close();

```
