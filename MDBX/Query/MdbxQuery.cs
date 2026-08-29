using System.Collections;

namespace MDBX;

/// <summary>
/// Высокоуровневая отложенная (lazy) абстракция над курсором MDBX.
/// Представляет собой <see cref="IEnumerable{MdbxEntry}"/>, перебирающий все записи таблицы
/// поверх нативного курсора. Каждый вызов <see cref="GetEnumerator"/> открывает новый курсор,
/// а перебор выполняется лениво, что делает тип полностью совместимым с LINQ
/// (Where, Select, First, ToList и т.д.).
/// </summary>
public sealed class MdbxQuery : IEnumerable<MdbxEntry>
{
    private readonly MdbxTransaction _txn;
    private readonly MdbxDatabase _db;

    internal MdbxQuery(MdbxTransaction txn, MdbxDatabase db)
    {
        _txn = txn ?? throw new ArgumentNullException(nameof(txn));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    /// <inheritdoc />
    public IEnumerator<MdbxEntry> GetEnumerator() => new Enumerator(_txn, _db);

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class Enumerator : IEnumerator<MdbxEntry>
    {
        private readonly MdbxCursor _cursor;
        private MdbxEntry? _current;
        private bool _started;

        public Enumerator(MdbxTransaction txn, MdbxDatabase db)
        {
            _cursor = MdbxCursor.Create(txn, db);
        }

        public MdbxEntry Current => _current!;

        object IEnumerator.Current => _current!;

        public bool MoveNext()
        {
            byte[]? key;
            byte[]? value;

            if (!_started)
            {
                _started = true;
                value = _cursor.GetFirst(out key);
            }
            else
            {
                value = _cursor.GetNext(out key);
            }

            if (value == null)
            {
                _current = null;
                return false;
            }

            _current = new MdbxEntry(key!, value);
            return true;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() => _cursor.Dispose();
    }
}
