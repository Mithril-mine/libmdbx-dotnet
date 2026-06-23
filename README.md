# libmdbx-dotnet

**libmdbx-dotnet** — нативные P/Invoke-привязки для [libmdbx](https://libmdbx.dqdkfa.ru/) и высокоуровневый API.


## Что такое libmdbx

libmdbx — исключительно быстрый, компактный и мощный встраиваемый транзакционный key-value движок без WAL. Он позволяет рою многопоточных процессов выполнять ACID-чтение и запись нескольких карт и мультикарт в локально разделяемой базе, обеспечивая экстраординарную производительность при минимальных накладных расходах за счёт отображения в память и операций O(log N) на B+ дереве. Не требует обслуживания и восстановления после сбоя, гарантирует целостность данных и свободно распространяется по лицензии Apache 2.0.

- **Полный ACID** - Атомарные, согласованные, изолированные и долговечные транзакции на основе MVCC и copy-on-write. Целостность данных гарантируется даже после сбоя.

- **Без WAL и восстановления** - Нет журнала упреждающей записи и восстановления после сбоя. Благодаря теневой подкачке страниц база не требует обслуживания и не растёт бесконтрольно.

- **Высокая производительность** - Данные отображаются в память (memory-mapped) и читаются напрямую без копирования. Операции поиска, вставки, обновления и удаления — O(log N) благодаря B+ дереву.

- **Параллельный доступ** -  Рой многопоточных процессов выполняет ACID-чтение и запись в общую базу. Читатели не блокируются и не требуют атомарных операций, масштабируясь по ядрам

- **Строго последовательные изменения** -  Изменения вносятся строго последовательно через единственный мьютекс — исключены конфликты транзакций и взаимоблокировки. Писатели не блокируют читателей и наоборот.

- **Несколько таблиц key-value** -  Множество таблиц key-value в одном файле данных, включая эффективные multimap: упорядоченные, искомые и обходимые множества значений без дублирования ключей.

- **Кроссплатформенность** - Поддержка Linux, Windows, macOS, Android, iOS, FreeBSD, Solaris и других систем, совместимых с POSIX.1-2008.

- **Компактность и встраивание** - Всего несколько плоских файлов исходного кода, никаких внутренних потоков и серверных процессов. Полностью пригодно для глубокого встраивания.

## License

This project is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).

libmdbx native library is not shipped with this assembly. The assembly will load `libmdbx` from the location below according to your platform and OS:

- **Windows**: `native/windows/x64/mdbx.dll`
- **Linux**: `native/linux/x64/libmdbx.so`

## Быстрый старт


### Установка

#### Добавить источник

```bash
dotnet nuget add source https://nexus.amsoft.spb.ru/repository/nuget-public/ --name amsoft-nexus
```

#### Установка пакета

```bash
dotnet add package mdbx --version 0.14.3-39-8-develop
```


#### Установка одной командой без добавления источника

```bash
dotnet add package mdbx --version 0.14.3-39-8-develop --source https://nexus.amsoft.spb.ru/repository/nuget-public/
```

#### [Посмотреть список всех доступных версий](https://public.amsoft.spb.ru/libmdbx/downloads/libmdbx-dotnet/)

#### [Посмотреть в Nexus](https://nexus.amsoft.spb.ru/#browse/browse:nuget-public:mdbx)


### CRUD операции

```csharp
using MDBX;

using var env = MdbxEnvironment.Open("data.mdbx", maxDatabases: 4);
using var db = env.OpenDatabaseEx("users");

using (var txn = env.BeginTransaction())
{
    db.Put(txn, "user:1", "{\"name\":\"Alice\",\"role\":\"admin\"}");
    db.Put(txn, "user:2", "{\"name\":\"Bob\",\"role\":\"user\"}");
    txn.Commit();
}

using (var readTxn = env.BeginTransaction())
{
    var alice = db.Get(readTxn, "user:1");
    Console.WriteLine(alice); // {"name":"Alice","role":"admin"}
}

using (var txn = env.BeginTransaction())
{
    var deleted = db.Delete(txn, "user:1");
    Console.WriteLine(deleted); // True
    txn.Commit();
}

using (var readTxn = env.BeginTransaction())
{
    var alice = db.Get(readTxn, "user:1");
    Console.WriteLine(alice); // null
}
```

### LINQ-запросы (Find)

```csharp
using (var txn = env.BeginTransaction())
{
    var allValues = db.Find(txn).Select(e => Encoding.UTF8.GetString(e.Value));

    var userNames = db.Find(txn, Encoding.UTF8.GetString).Where(n => n.StartsWith("A"));

    var entries = db.Find(txn, Encoding.UTF8.GetString, Encoding.UTF8.GetString);

    var rawValues = db.FindValues(txn);

    var typed = db.Find(txn, Encoding.UTF8.GetString).First();

    txn.Commit();
}
```

### Удаление элементов

```csharp
using var env = MdbxEnvironment.Open("data.mdbx", maxDatabases: 4);
using var db = env.OpenDatabaseEx("users");

using (var txn = env.BeginTransaction())
{
    db.Put(txn, "user:1", "Alice");
    db.Put(txn, "user:2", "Bob");
    db.Put(txn, "user:3", "Charlie");

    using (var cursor = txn.OpenCursor(db))
    {
        cursor.GetFirst(out var key, out var value);
        cursor.Delete();
    }

    cursor.GetFirst(out var firstKey, out _);
    Console.WriteLine(firstKey); // user:2

    txn.Commit();
}

MdbxEnvironment.Delete("data.mdbx");
```

### Удаление таблиц (DBI)

```csharp
using var env = MdbxEnvironment.Open("data.mdbx", maxDatabases: 4);
using var usersDb = env.OpenDatabaseEx("users");
using var logsDb = env.OpenDatabaseEx("logs");

using (var txn = env.BeginTransaction())
{
    logsDb.DropDatabase(logsDb.Dbi, deleteData: true);
    txn.Commit();
}
```

### Курсоры и итерация

```csharp
using (var txn = env.BeginTransaction())
using (var cursor = txn.OpenCursor(db))
{
    cursor.GetFirst(out var key, out var value);
    while (!cursor.IsEof())
    {
        Console.WriteLine($"{key} = {value}");
        cursor.GetNext(out key, out value);
    }

    cursor.GetLast(out var lastKey);
    var current = cursor.GetCurrent(out var currentKey);
    nuint count = cursor.Count();
    bool onFirst = cursor.IsOnFirst();
    bool onLast = cursor.IsOnLast();
    cursor.GetNextDup(out var dupKey);
    cursor.Renew(txn);
    cursor.Put("key", "value");
}
```

### Дубликаты (Dupsort)

```csharp
using var env = MdbxEnvironment.Open("dup.mdbx", maxDatabases: 2);
using var db = env.OpenDatabaseEx("dup_table", MdbxDbFlags.MDBX_DUPSORT | MdbxDbFlags.MDBX_CREATE);

using (var txn = env.BeginTransaction())
{
    db.Put(txn, "fruit", "apple");
    db.Put(txn, "fruit", "banana");
    db.Put(txn, "fruit", "cherry");
    txn.Commit();
}

using (var readTxn = env.BeginTransaction())
using (var cursor = readTxn.OpenCursor(db))
{
    cursor.GetFirst(out var key);
    cursor.GetNextDup(out var nextVal);
}
```

### Несколько таблиц (DBI)

```csharp
using var usersDb = env.OpenDatabaseEx("users");
using var logsDb = env.OpenDatabaseEx("logs");
using var defaultDb = env.OpenDatabaseEx();

using (var txn = env.BeginTransaction())
{
    usersDb.Put(txn, "u:1", "Alice");
    logsDb.Put(txn, "l:1", "created");
    defaultDb.Put(txn, "default_key", "default_value");

    var u1 = usersDb.Get(txn, "u:1");
    var l1 = logsDb.Get(txn, "l:1");

    txn.Commit();
}
```

### Флаги и продвинутые сценарии

```csharp
using var env = MdbxEnvironment.Open("cache.mdbx", maxDatabases: 2);

env.SetOption(MdbxOption.MDBX_OPT_MAX_DB, 4);
env.SetGeometry(sizeLower: -1, sizeNow: -1, sizeUpper: -1);

var stat = env.GetStat();
var info = env.GetInfo();

env.Defrag(progressCallback: (result) =>
{
    Console.WriteLine($"Pages compacted: {result->pages}");
    return 0;
});

using var db = env.OpenDatabaseEx("flags_demo");
using var txn = env.BeginTransaction();
db.Put(txn, "counter", Encoding.UTF8.GetBytes("1"));
db.Put(txn, "counter", Encoding.UTF8.GetBytes("2"), MdbxPutFlags.MDBX_CURRENT);
txn.Commit();
```

### Обнаружение проблем с медленными читателями (HSR)

```csharp
env.SetHsr((envPtr, txnPtr, laggard, readers, total, dead, retryNum) =>
{
    Console.WriteLine($"Slow reader detected: txn={txnPtr->txn_id}, lag={laggard}");
    return 0;
});
```

## Полный список низкоуровневых привязок (LIBMDBX_API)

Каждая функция libmdbx имеет соответствующее P/Invoke-объявление в пространстве имен `MDBX.Native.Bindings.*`.

### Окружение (Env)
- `mdbx_env_create` — создать окружение
- `mdbx_env_open` / `mdbx_env_openW` — открыть окружение (ANSI/Unicode)
- `mdbx_env_close` / `mdbx_env_close_ex` — закрыть окружение
- `mdbx_env_delete` / `mdbx_env_deleteW` — удалить окружение
- `mdbx_env_copy` / `mdbx_env_copyW` / `mdbx_env_copy2fd` — копирование окружения
- `mdbx_env_open_for_recovery` / `mdbx_env_open_for_recoveryW` — открыть для восстановления
- `mdbx_env_turn_for_recovery` — переключить мета-страницу для восстановления
- `mdbx_env_resurrect_after_fork` — восстановить после fork
- `mdbx_env_warmup` — прогрев (предзагрузка страниц в память)
- `mdbx_env_defrag` — дефрагментация
- `mdbx_env_get_fd` — получить файловый дескриптор
- `mdbx_env_get_path` / `mdbx_env_get_pathW` — получить путь
- `mdbx_env_get_option` / `mdbx_env_set_option` — получить/установить параметр
- `mdbx_env_get_flags` / `mdbx_env_set_flags` — получить/установить флаги
- `mdbx_env_set_geometry` — установить геометрию (размеры файлов)
- `mdbx_env_get_hsr` / `mdbx_env_set_hsr` — Handle-Slow-Readers колбэк
- `mdbx_env_get_userctx` / `mdbx_env_set_userctx` — пользовательский контекст
- `mdbx_env_get_maxkeysize` / `mdbx_env_get_maxkeysize_ex` — максимальный размер ключа
- `mdbx_env_get_maxvalsize_ex` — максимальный размер значения
- `mdbx_env_get_pairsize4page_max` — максимальный размер пары для страницы
- `mdbx_env_get_valsize4page_max` — максимальный размер значения для страницы
- `mdbx_env_info_ex` — информация об окружении
- `mdbx_env_stat_ex` — статистика окружения
- `mdbx_env_sync_ex` — синхронизация
- `mdbx_preopen_snapinfo` / `mdbx_preopen_snapinfoW` — снапшот информации
- `mdbx_txn_copy2pathname` / `mdbx_txn_copy2pathnameW` / `mdbx_txn_copy2fd` — копирование транзакции

### Транзакции (Txn)
- `mdbx_txn_begin_ex` — начать транзакцию
- `mdbx_txn_commit_ex` — зафиксировать транзакцию
- `mdbx_txn_commit_embark_read` — зафиксировать для embark read
- `mdbx_txn_rollback` — откатить транзакцию
- `mdbx_txn_abort_ex` — отменить транзакцию
- `mdbx_txn_amend` — превратить read-транзакцию в write
- `mdbx_txn_clone` — клонировать транзакцию
- `mdbx_txn_reset` — сбросить транзакцию (освободить ресурсы)
- `mdbx_txn_renew` — обновить (renew) транзакцию
- `mdbx_txn_refresh` — обновить (refresh) транзакцию
- `mdbx_txn_break` — прервать транзакцию
- `mdbx_txn_park` / `mdbx_txn_unpark` — парковка/распарковка транзакции
- `mdbx_txn_info` — информация о транзакции
- `mdbx_txn_env` — получить окружение транзакции
- `mdbx_txn_flags` — получить флаги транзакции
- `mdbx_txn_id` — получить ID транзакции
- `mdbx_txn_checkpoint` — checkpoint транзакции
- `mdbx_txn_get_userctx` / `mdbx_txn_set_userctx` — пользовательский контекст
- `mdbx_txn_release_all_cursors_ex` — освободить все курсоры
- `mdbx_txn_straggler` — информация о «стragler» транзакции
- `mdbx_txn_lock` / `mdbx_txn_unlock` — блокировка окружения для транзакции

### Таблицы (DBI)
- `mdbx_dbi_open` / `mdbx_dbi_open2` — открыть таблицу по имени/значению
- `mdbx_dbi_open_ex` / `mdbx_dbi_open_ex2` — открыть с кастомными функциями сравнения
- `mdbx_dbi_close` — закрыть таблицу
- `mdbx_dbi_rename` / `mdbx_dbi_rename2` — переименовать таблицу
- `mdbx_dbi_stat` — статистика таблицы
- `mdbx_dbi_flags_ex` — флаги и состояние таблицы
- `mdbx_dbi_dupsort_depthmask` — маска глубины дубликатов
- `mdbx_dbi_sequence` — последовательность таблицы
- `mdbx_drop` — удалить таблицу

### CRUD операции
- `mdbx_get` / `mdbx_get_ex` — получить значение
- `mdbx_put` — записать значение
- `mdbx_replace` / `mdbx_replace_ex` — заменить значение
- `mdbx_del` — удалить элемент
- `mdbx_get_equal_or_great` — получить значение или ближайший больший ключ

### Курсоры (Cursor)
- `mdbx_cursor_create` — создать курсор
- `mdbx_cursor_open` — открыть курсор для таблицы
- `mdbx_cursor_close` / `mdbx_cursor_close2` — закрыть курсор
- `mdbx_cursor_bind` / `mdbx_cursor_unbind` — привязать/отвязать от таблицы
- `mdbx_cursor_renew` — обновить для новой транзакции
- `mdbx_cursor_reset` — сбросить курсор
- `mdbx_cursor_copy` — скопировать курсор
- `mdbx_cursor_compare` — сравнить два курсора
- `mdbx_cursor_get` / `mdbx_cursor_get_batch` — получить элемент/пакет
- `mdbx_cursor_put` — записать через курсор
- `mdbx_cursor_del` — удалить через курсор
- `mdbx_cursor_delete_range` — удалить диапазон
- `mdbx_cursor_count` / `mdbx_cursor_count_ex` — количество элементов
- `mdbx_cursor_distance` — расстояние между курсорами
- `mdbx_cursor_scroll` — прокрутка
- `mdbx_cursor_distribute` — распределение элементов между курсорами
- `mdbx_cursor_bunch_delete` — пакетное удаление
- `mdbx_cursor_eof` — достигнут ли конец таблицы
- `mdbx_cursor_on_first` / `mdbx_cursor_on_last` — позиция на первом/последнем
- `mdbx_cursor_on_first_dup` / `mdbx_cursor_on_last_dup` — позиция на первом/последнем дубликате
- `mdbx_cursor_txn` / `mdbx_cursor_dbi` — получить транзакцию/DBI курсора
- `mdbx_cursor_get_userctx` / `mdbx_cursor_set_userctx` — пользовательский контекст
- `mdbx_cursor_ignord` — игнорировать дубликаты
- `mdbx_cursor_scan` / `mdbx_cursor_scan_from` — сканирование с предикатом

### Настройки (Settings)
- `mdbx_env_set_option` / `mdbx_env_get_option` — параметры окружения
- `mdbx_env_set_flags` / `mdbx_env_get_flags` — флаги окружения
- `mdbx_env_set_geometry` — геометрия окружения
- `mdbx_env_set_hsr` / `mdbx_env_get_hsr` — HSR колбэк
- `mdbx_env_set_userctx` / `mdbx_env_get_userctx` — пользовательский контекст
- `mdbx_default_pagesize` — размер страницы по умолчанию
- `mdbx_get_sysraminfo` — информация о системной RAM
- `mdbx_is_readahead_reasonable` — проверка readahead
- `mdbx_limits_dbsize_min` / `mdbx_limits_dbsize_max` — лимиты размера БД
- `mdbx_limits_keysize_min` / `mdbx_limits_keysize_max` — лимиты размера ключа
- `mdbx_limits_valsize_min` / `mdbx_limits_valsize_max` — лимиты размера значения
- `mdbx_limits_pairsize4page_max` — максимальный размер пары для страницы
- `mdbx_limits_valsize4page_max` — максимальный размер значения для страницы
- `mdbx_limits_txnsize_max` — максимальный размер транзакции
- `mdbx_ratio2digits` / `mdbx_ratio2percents` — форматирование соотношений

### Обработка ошибок (Error Handling)
- `mdbx_strerror` / `mdbx_strerror_r` — описание ошибки
- `mdbx_liberr2str` — описание ошибки (только libmdbx)
- `mdbx_strerror_ANSI2OEM` / `mdbx_strerror_r_ANSI2OEM` — ANSI/OEM версии
- `mdbx_assert_fail` — аварийное завершение при assert
- `mdbx_set_panic` — функция обработки паники

### Дополнительные возможности (Extra)
- `mdbx_cmp` / `mdbx_dcmp` — сравнение ключей/значений
- `mdbx_get_keycmp` / `mdbx_get_datacmp` — получить функцию сравнения
- `mdbx_key_from_jsonInteger` / `mdbx_jsonInteger_from_key` — JSON integer ключи
- `mdbx_key_from_double` / `mdbx_double_from_key` — double ключи
- `mdbx_key_from_float` / `mdbx_float_from_key` — float ключи
- `mdbx_key_from_ptrdouble` / `mdbx_key_from_ptrfloat` — указатели на float/double
- `mdbx_int32_from_key` / `mdbx_int64_from_key` — int32/int64 из ключа
- `mdbx_enumerate_tables` — перечисление таблиц
- `mdbx_reader_list` / `mdbx_reader_check` — работа с читателями
- `mdbx_thread_register` / `mdbx_thread_unregister` — регистрация потоков
- `mdbx_txn_lock` / `mdbx_txn_unlock` — блокировка окружения
- `mdbx_estimate_distance` / `mdbx_estimate_move` / `mdbx_estimate_range` — оценки
- `mdbx_cache_get` / `mdbx_cache_get_SingleThreaded` — кэширование
- `mdbx_env_chk` / `mdbx_env_chk_encount_problem` — проверка целостности
- `mdbx_is_dirty` — проверка «грязных» страниц
- `mdbx_gc_info` — информация о GC и использовании страниц
- `mdbx_dump_val` — форматирование значения в строку

### Статистика (Stat)
- `mdbx_env_stat_ex` — статистика окружения
- `mdbx_env_info_ex` — информация об окружении
- `mdbx_env_sync_ex` — синхронизация окружения

### Отладка (Debug)
- `mdbx_setup_debug` / `mdbx_setup_debug_nofmt` — настройка логирования
- `mdbx_canary_put` / `mdbx_canary_get` — структура-маяк

## Покрытие тестами

Проект содержит модульные тесты для большинства низкоуровневых привязок и высокоуровневых сценариев использования.

### ✅ Хорошее покрытие

| Модуль | Что покрыто |
|---|---|
| **Cursor (Native)** | Open/Close/Create/Bind/Copy, Get/Put/Del, навигация (First/Last/Next/Prev), Count/CountEx, Distance, DeleteRange, DupSort (OnFirstDup/OnLastDup, Ignord), Batch, Userctx, Renew/Reset/Unbind, Compare, Txn/Dbi accessors |
| **CRUD (Native)** | Put (`MDBX_UPSERT`, `MDBX_CURRENT`), Get, GetEx, Replace, ReplaceEx, Del, GetEqualOrGreat |
| **DBI (Native)** | Open/Open2/OpenEx/OpenEx2, Close, Drop, Rename/Rename2, Stat, FlagsEx, DupsortDepthmask, Sequence |
| **Settings** | SetOption/GetOption, SetFlags/GetFlags, GetMaxkeysize/Ex, GetMaxvalsize/Ex, GetPath/GetPathW, GetFd, SetUserctx/GetUserctx, DefaultPagesize, SetGeometry, SetHsr/GetHsr, все Limits* функции, Ratio2Digits, Ratio2Percents |
| **Extra (Native)** | Cmp, GetKeycmp, GetDatacmp, KeyFrom* конвертации, Float/Double/Int32/Int64/JsonInteger извлечение, IsDirty, GcInfo, CacheGet/SingleThreaded, ReaderList/Check, ThreadRegister/Unregister, EstimateDistance/EstimateMove, EnumerateTables, TxnLock/Unlock, EnvChk |
| **Txn (Native)** | BeginEx, CommitEx, Rollback, AbortEx, Clone, Reset, Renew, Refresh, Info, Id, Env, Flags, SetUserctx/GetUserctx, Checkpoint, Break, Park/Unpark, ReleaseAllCursorsEx, Straggler |
| **Env (Native)** | Create, Open/OpenW, Close/CloseEx, Delete/DeleteW, Copy/CopyW/Copy2Fd, OpenForRecovery/OpenForRecoveryW, TurnForRecovery, Warmup, Defrag, PreopenSnapinfo/PreopenSnapinfoW |
| **ErrorHandling** | Strerror, StrerrorR, Liberr2Str, ANSI2OEM версии, SetPanic |

### ⚠️ Ограниченное покрытие

| Модуль | Что не покрыто |
|---|---|
| **Query (High-level)** | Только 4 базовых теста; нет тестов для range queries, `First/Single/Any/Count`, cursor navigation внутри query, сложных предикатов |
| **Stat** | Только 3 функции (`EnvStatEx`, `EnvInfoEx`, `EnvSyncEx`); нет `DbiStat` в isolation, нет non-Ex variants |
| **CRUD (High-level)** | Нет тестов для dupsort/multiple-value; нет тестов флагов `MDBX_APPEND`, `MDBX_APPENDDUP`, `MDBX_NOOVERWRITE`, `MDBX_MULTIPLE`; нет concurrency-тестов |
| **Debug** | Только 4 функции (`SetupDebug`, `SetupDebugNofmt`, `CanaryPut/Get`, `DumpVal`) |
| **Examples** | Служат integration-level smoke-тестами, но не покрывают edge-cases |

### ❌ Не покрыто

- **Многопоточность** — нет тестов для concurrent readers/writers, deadlock detection
- **Transaction Nesting** — нет тестов для вложенных транзакций
- **Dupsort/Многозначение (High-level)** — нет high-level тестов для дубликатов, append, multi-value
- **Восстановление после повреждений** — `OpenForRecoverage` тестируется, но нет actual corruption/recovery validation
- **HSR (Handle-Slow-Readers)** — только SetHsr/GetHsr с null; нет реальных HSR behavior тестов
- **Checkpoint** — только вызов с null; нет LSN/TWN validation
- **Large values / overflow pages** — нет тестов для значений превышающих размер страницы

## Документация

Сгенерировать документацию:

```bash
docfx docfx.json --serve
```

## Сборка

```bash
dotnet build libmdbx-dotnet.sln
```

## Тестирование

```bash
dotnet test libmdbx-dotnet.sln
```
