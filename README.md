# libmdbx-dotnet

.NET bindings(dotnet) of [libmdbx](https://libmdbx.dqdkfa.ru/)

libmdbx — это чрезвычайно быстрая, компактная, мощная встраиваемая транзакционная база данных «ключ-значение» под лицензией Apache 2.0. libmdbx обладает особым набором свойств и возможностей, ориентированных на создание уникальных легковесных решений.

★ Превосходит легендарную LMDB по надежности, возможностям и производительности 

★ Используется в сотнях открытых проектах, в том числе в Ethereum.

По сути, libmdbx — это глубоко переработанный и расширенный вариант легендарной базы данных с отображением в памяти Lightning Memory-Mapped Database. libmdbx унаследовала все преимущества LMDB, но при этом решила ряд проблем и добавила множество улучшений.

### License

This project is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).

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

## Документация

Сгенерировать документацию.

```bash
docfx docfx.json --serve
```

# Статус готовности привязок функций C API

- ⏳ mdbx_canary_get
- ⏳ mdbx_canary_put
- ⏳ mdbx_cursor_bind
- ⏳ mdbx_cursor_close
- ✅ mdbx_cursor_close2
- ⏳ mdbx_cursor_compare
- ⏳ mdbx_cursor_copy
- ✅ mdbx_cursor_count
- ⏳ mdbx_cursor_count_ex
- ⏳ mdbx_cursor_create
- ⏳ mdbx_cursor_dbi
- ✅ mdbx_cursor_del
- ⏳ mdbx_cursor_eof
- ✅ mdbx_cursor_get
- ⏳ mdbx_cursor_get_userctx
- ⏳ mdbx_cursor_ignord
- ⏳ mdbx_cursor_on_first
- ⏳ mdbx_cursor_on_first_dup
- ⏳ mdbx_cursor_on_last
- ⏳ mdbx_cursor_on_last_dup
- ✅ mdbx_cursor_open
- ✅ mdbx_cursor_put
- ⏳ mdbx_cursor_renew
- ⏳ mdbx_cursor_reset
- ⏳ mdbx_cursor_set_userctx
- ⏳ mdbx_cursor_txn
- ⏳ mdbx_cursor_unbind
- ✅ mdbx_dbi_close
- ⏳ mdbx_dbi_dupsort_depthmask
- ⏳ mdbx_dbi_flags_ex
- ✅ mdbx_dbi_open
- ⏳ mdbx_dbi_open2
- ⏳ mdbx_dbi_rename
- ⏳ mdbx_dbi_rename2
- ⏳ mdbx_dbi_sequence
- ⏳ mdbx_dbi_stat
- ⏳ mdbx_default_pagesize
- ✅ mdbx_del
- ✅ mdbx_drop
- ⏳ mdbx_dump_val
- ⏳ mdbx_enumerate_tables
- ⏳ mdbx_env_chk_encount_problem
- ✅ mdbx_env_close_ex
- ⏳ mdbx_env_copy
- ⏳ mdbx_env_copy2fd
- ⏳ mdbx_env_copyW
- ✅ mdbx_env_create
- ⏳ mdbx_env_delete
- ⏳ mdbx_env_deleteW
- ⏳ mdbx_env_get_fd
- ✅ mdbx_env_get_flags
- ⏳ mdbx_env_get_hsr
- ✅ mdbx_env_get_maxkeysize
- ⏳ mdbx_env_get_maxkeysize_ex
- ⏳ mdbx_env_get_maxvalsize_ex
- ⏳ mdbx_env_get_option
- ⏳ mdbx_env_get_pairsize4page_max
- ⏳ mdbx_env_get_path
- ⏳ mdbx_env_get_pathW
- ⏳ mdbx_env_get_userctx
- ⏳ mdbx_env_get_valsize4page_max
- ⏳ mdbx_env_info_ex
- ✅ mdbx_env_open
- ⏳ mdbx_env_open_for_recovery
- ✅ mdbx_env_openW
- ⏳ mdbx_env_resurrect_after_fork
- ⏳ mdbx_env_set_assert
- ✅ mdbx_env_set_flags
- ⏳ mdbx_env_set_hsr
- ⏳ mdbx_env_set_option
- ⏳ mdbx_env_set_userctx
- ✅ mdbx_env_stat
- ⏳ mdbx_env_stat_ex
- ⏳ mdbx_env_sync_ex
- ⏳ mdbx_env_turn_for_recovery
- ⏳ mdbx_estimate_distance
- ⏳ mdbx_float_from_key
- ✅ mdbx_get
- ⏳ mdbx_get_datacmp
- ⏳ mdbx_get_equal_or_great
- ⏳ mdbx_get_ex
- ⏳ mdbx_get_keycmp
- ⏳ mdbx_get_sysraminfo
- ⏳ mdbx_int32_from_key
- ⏳ mdbx_int64_from_key
- ⏳ mdbx_is_dirty
- ⏳ mdbx_is_readahead_reasonable
- ⏳ mdbx_jsonInteger_from_key
- ⏳ mdbx_key_from_double
- ⏳ mdbx_key_from_float
- ⏳ mdbx_key_from_jsonInteger
- ⏳ mdbx_key_from_ptrdouble
- ⏳ mdbx_key_from_ptrfloat
- ⏳ mdbx_liberr2str
- ⏳ mdbx_limits_dbsize_max
- ⏳ mdbx_limits_dbsize_min
- ⏳ mdbx_limits_keysize_max
- ⏳ mdbx_limits_keysize_min
- ⏳ mdbx_limits_txnsize_max
- ⏳ mdbx_limits_valsize_max
- ⏳ mdbx_limits_valsize_min
- ⏳ mdbx_limits_valsize4page_max
- ⏳ mdbx_module_handler
- ⏳ mdbx_panic
- ⏳ mdbx_preopen_snapinfo
- ⏳ mdbx_preopen_snapinfoW
- ✅ mdbx_put
- ⏳ mdbx_reader_check
- ⏳ mdbx_reader_list
- ⏳ mdbx_setup_debug
- ✅ mdbx_strerror
- ⏳ mdbx_strerror_ANSI2OEM
- ⏳ mdbx_strerror_r
- ⏳ mdbx_strerror_r_ANSI2OEM
- ⏳ mdbx_thread_register
- ⏳ mdbx_thread_unregister
- ✅ mdbx_txn_abort
- ⏳ mdbx_txn_break
- ⏳ mdbx_txn_commit_ex
- ⏳ mdbx_txn_copy2fd
- ⏳ mdbx_txn_copy2pathname
- ⏳ mdbx_txn_copy2pathnameW
- ⏳ mdbx_txn_env
- ⏳ mdbx_txn_flags
- ⏳ mdbx_txn_get_userctx
- ✅ mdbx_txn_id
- ⏳ mdbx_txn_info
- ⏳ mdbx_txn_lock
- ⏳ mdbx_txn_park
- ⏳ mdbx_txn_release_all_cursors_ex
- ✅ mdbx_txn_renew
- ✅ mdbx_txn_reset
- ⏳ mdbx_txn_set_userctx
- ⏳ mdbx_txn_straggler
- ⏳ mdbx_txn_unlock
- ⏳ mdbx_txn_unpark
- ⏳ mdbx_setup_debug_nofmt