namespace Libmdbx
{
    /// <summary>
    /// Cursor-positioning operations for <see cref="MdbxCursor.Get"/>.
    /// Maps to <c>MDBX_cursor_op</c> in the C header.
    /// </summary>
    public enum CursorOp : int
    {
        /// <summary>Position at first key/data item.</summary>
        First = 0,

        /// <summary>
        /// With <c>DUPSORT</c>: position at first data item of current key.
        /// </summary>
        FirstDup,

        /// <summary>
        /// With <c>DUPSORT</c>: position at key, nearest data.
        /// </summary>
        GetBoth,

        /// <summary>
        /// With <c>DUPSORT</c>: position at nearest data greater-than-or-equal to
        /// specified key.
        /// </summary>
        GetBothRange,

        /// <summary>Return key/data at current cursor position.</summary>
        GetCurrent,

        /// <summary>
        /// With <c>DUPFIXED</c>: return all duplicate data items at current
        /// cursor position.
        /// </summary>
        GetMultiple,

        /// <summary>Position at last key/data item.</summary>
        Last,

        /// <summary>
        /// With <c>DUPSORT</c>: position at last data item of current key.
        /// </summary>
        LastDup,

        /// <summary>Move to next key/data item.</summary>
        Next,

        /// <summary>
        /// With <c>DUPSORT</c>: move to next data item of current key.
        /// </summary>
        NextDup,

        /// <summary>
        /// With <c>DUPFIXED</c>: return all duplicate data items at next
        /// cursor position.
        /// </summary>
        NextMultiple,

        /// <summary>
        /// Move to next key; skip any duplicate data items.
        /// </summary>
        NextNoDup,

        /// <summary>Move to previous key/data item.</summary>
        Prev,

        /// <summary>
        /// With <c>DUPSORT</c>: move to previous data item of current key.
        /// </summary>
        PrevDup,

        /// <summary>
        /// Move to previous key; skip any duplicate data items.
        /// </summary>
        PrevNoDup,

        /// <summary>Position at specified key.</summary>
        Set,

        /// <summary>Position at specified key, return both key and data.</summary>
        SetKey,

        /// <summary>
        /// Position at first key greater-than-or-equal to specified key.
        /// </summary>
        SetRange,

        /// <summary>
        /// With <c>DUPFIXED</c>: position at previous page and return all
        /// duplicate data items.
        /// </summary>
        PrevMultiple,

        /// <summary>
        /// Position at first key-value pair greater-than or equal to the
        /// specified pair.
        /// </summary>
        SetLowerBound,

        /// <summary>
        /// Position at first key-value pair greater-than the specified pair.
        /// </summary>
        SetUpperBound,
    }
}
