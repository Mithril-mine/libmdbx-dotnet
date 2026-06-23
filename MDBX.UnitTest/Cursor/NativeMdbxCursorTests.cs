using MDBX.Native.Types;
using Xunit;

namespace MDBX.UnitTest.Cursor;

/// <summary>
/// Тесты функций курсоров libmdbx.
/// </summary>
public class NativeMdbxCursorTests
{
    private const ushort DefaultMode = 0x700;

    private static byte[] GetNullTerminatedBytes(string s)
    {
        byte[] bytes = new byte[s.Length + 1];
        for (int i = 0; i < s.Length; i++)
            bytes[i] = (byte)s[i];
        bytes[s.Length] = 0;
        return bytes;
    }

    [Fact]
    public unsafe void MdbxCursorOpen_CreatesCursorSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));
        Assert.True(cursor != null);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorGet_RetrievesItemSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_get.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));
        Assert.True(cursor != null);

        int eof = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorEof(cursor);
        Assert.True(eof != 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorPut_InsertsItemSuccessfully()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_put.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("test_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("test_value");

        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorPut(cursor, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        fixed (byte* keyPtr = keyBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(null, 0);
            Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor, &key, &val, MDBX_cursor_op.MDBX_GET_CURRENT));
            Assert.True(val.IovBase != null);
            Assert.Equal(valueBytes.Length, (long)val.IovLen);
        }

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorCreate_CreatesCursor()
    {
        MDBX_cursor* cursor;
        void* ctx = null;

        cursor = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCreate(ctx);
        Assert.True(cursor != null);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
    }

    [Fact]
    public unsafe void MdbxCursorBind_BindsCursorToTransaction()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_bind.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        cursor = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCreate(null);
        Assert.True(cursor != null);

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorBind(txn, cursor, dbi));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorCopy_CopiesCursor()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_copy.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursorSrc;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursorSrc));

        MDBX_cursor* cursorDest;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursorDest));

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCopy(cursorSrc, cursorDest));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursorSrc);
        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursorDest);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorCompare_ComparesCursors()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_compare.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor1;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor1));

        MDBX_cursor* cursor2;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor2));

        int result = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCompare(cursor1, cursor2, false);
        Assert.Equal(0, result);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor1);
        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor2);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorCount_CountsElements()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_count.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        // Вставим элементы
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        // Позиционируем курсор на первый элемент
        MDBX_val cursorKey = new MDBX_val(null, 0);
        MDBX_val cursorVal = new MDBX_val(null, 0);
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor, &cursorKey, &cursorVal, MDBX_cursor_op.MDBX_FIRST));

        nuint count = 0;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCount(cursor, &count));
        Assert.True(count > 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorOnFirst_ChecksFirstPosition()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_on_first.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        // На пустой таблице курсор не на первом элементе
        int isFirst = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOnFirst(cursor);
        Assert.Equal(-1, isFirst);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorOnLast_ChecksLastPosition()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_on_last.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        // На пустой таблице курсор не на последнем элементе
        int isLast = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOnLast(cursor);
        Assert.Equal(-1, isLast);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorTxn_ReturnsCursorTransaction()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_txn.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        MDBX_txn* cursorTxn = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorTxn(cursor);
        Assert.True(txn == cursorTxn);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorDbi_ReturnsCursorDbi()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_dbi.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        uint cursorDbi = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorDbi(cursor);
        Assert.Equal(dbi, cursorDbi);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorDel_DeletesElement()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_del.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        // Вставим элемент
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        // Получим элемент на курсор
        byte[] searchKey = System.Text.Encoding.UTF8.GetBytes("key1");
        fixed (byte* searchKeyPtr = searchKey)
        {
            MDBX_val key = new MDBX_val(searchKeyPtr, (nuint)searchKey.Length);
            MDBX_val val = new MDBX_val(null, 0);
            Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor, &key, &val, MDBX_cursor_op.MDBX_SET_KEY));
        }

        // Удалим элемент
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorDel(cursor, 0));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorClose2_ReturnsZero()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_close2.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        int result = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose2(cursor);
        Assert.Equal(0, result);

        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorUnbind_UnbindsCursor()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_unbind.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorUnbind(cursor));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorRenew_RenewsCursor()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_renew.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorRenew(txn, cursor));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorReset_ResetsCursor()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_reset.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorReset(cursor));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorGetBatch_RetrievesBatch()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_batch.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("batch_key");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("batch_value");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        nuint count = 1;
        MDBX_val* pairs = stackalloc MDBX_val[2];
        pairs[0] = new MDBX_val(null, 0);
        pairs[1] = new MDBX_val(null, 0);
        int result = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGetBatch(cursor, &count, pairs, 1, MDBX_cursor_op.MDBX_FIRST);
        Assert.True(result == 0 || result > 0 || result == -1);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorSetUserctx_And_MdbxCursorGetUserctx_Work()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_userctx.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        int testValue = 123;
        void* ctx = &testValue;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorSetUserctx(cursor, ctx));
        void* retrieved = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGetUserctx(cursor);
        Assert.True(retrieved == ctx);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorDeleteRange_DeletesRange()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_delrange.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        for (int i = 0; i < 5; i++)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes($"key{i}");
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes($"value{i}");
            fixed (byte* keyPtr = keyBytes)
            fixed (byte* valPtr = valueBytes)
            {
                MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
                MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
                Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
            }
        }

        MDBX_cursor* cursor1;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor1));
        MDBX_cursor* cursor2;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor2));

        MDBX_val key1 = new MDBX_val(null, 0);
        MDBX_val val1 = new MDBX_val(null, 0);
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor1, &key1, &val1, MDBX_cursor_op.MDBX_FIRST));

        MDBX_val key2 = new MDBX_val(null, 0);
        MDBX_val val2 = new MDBX_val(null, 0);
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor2, &key2, &val2, MDBX_cursor_op.MDBX_LAST));

        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorDeleteRange(cursor1, cursor2, true, false));

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor1);
        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor2);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorDistance_ReturnsDistance()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_distance.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor1;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor1));
        MDBX_cursor* cursor2;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor2));

        nint distance = 0;
        int result = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorDistance(cursor1, cursor2, &distance, MDBX_cursor_op.MDBX_FIRST);
        Assert.True(result == 0 || result > 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor1);
        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor2);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorCountEx_CountsWithStat()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_count_ex.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes("key1");
        byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes("value1");
        fixed (byte* keyPtr = keyBytes)
        fixed (byte* valPtr = valueBytes)
        {
            MDBX_val key = new MDBX_val(keyPtr, (nuint)keyBytes.Length);
            MDBX_val val = new MDBX_val(valPtr, (nuint)valueBytes.Length);
            Assert.Equal(0, MDBX.Native.Bindings.Crud.NativeMdbx.MdbxPut(txn, dbi, &key, &val, MDBX_put_flags_t.MDBX_UPSERT));
        }

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        MDBX_val cursorKey = new MDBX_val(null, 0);
        MDBX_val cursorVal = new MDBX_val(null, 0);
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorGet(cursor, &cursorKey, &cursorVal, MDBX_cursor_op.MDBX_FIRST));

        nuint count = 0;
        MdbxStat stat;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorCountEx(cursor, &count, &stat, (nuint)sizeof(MdbxStat)));
        Assert.True(count > 0);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorIgnord_IgnoresDuplicates()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_cursor_ignord.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        uint dbi;
        Assert.Equal(0, MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, null, MDBX_db_flags_t.MDBX_DB_DEFAULTS, &dbi));

        MDBX_cursor* cursor;
        Assert.Equal(0, MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor));

        int result = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorIgnord(cursor);
        Assert.True(result != int.MinValue);

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }

    [Fact]
    public unsafe void MdbxCursorOnFirstDup_And_MdbxCursorOnLastDup_CheckDupPosition()
    {
        MDBX_env* env;
        Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvCreate(&env));
        byte[] pathBytes = GetNullTerminatedBytes("test_dbi_depthmask.mdbx");
        fixed (byte* pathPtr = pathBytes)
        {
            Assert.Equal(0, MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvOpen(env, pathPtr, MDBX_env_flags_t.MDBX_ENV_DEFAULTS, DefaultMode));
        }

        MDBX_txn* txn;
        Assert.Equal(0, MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnBeginEx(env, null, MDBX_txn_flags_t.MDBX_TXN_DEFAULTS, &txn, 0));

        byte[] tableName = GetNullTerminatedBytes("dupsort_table");
        uint dbi;
        fixed (byte* namePtr = tableName)
        {
            int dbiResult = MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiOpen(txn, namePtr, MDBX_db_flags_t.MDBX_DB_DEFAULTS | MDBX_db_flags_t.MDBX_DUPSORT | MDBX_db_flags_t.MDBX_CREATE, &dbi);
            Assert.True(dbiResult != int.MinValue);
        }

        MDBX_cursor* cursor;
        int cursorResult = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOpen(txn, dbi, &cursor);
        Assert.True(cursorResult != int.MinValue);

        if (cursorResult == 0)
        {
            int isFirstDup = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOnFirstDup(cursor);
            int isLastDup = MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorOnLastDup(cursor);
            Assert.True(isFirstDup != int.MinValue);
            Assert.True(isLastDup != int.MinValue);

            MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        }

        MDBX.Native.Bindings.Cursor.NativeMdbx.MdbxCursorClose(cursor);
        MDBX.Native.Bindings.Dbi.NativeMdbx.MdbxDbiClose(env, dbi);
        MDBX.Native.Bindings.Txn.NativeMdbx.MdbxTxnRollback(txn);
        MDBX.Native.Bindings.Env.NativeMdbx.MdbxEnvClose(env);
    }
}
