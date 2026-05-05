namespace Libmdbx
{
    /// <summary>
    /// Flags for <see cref="MdbxDatabase.Put"/> and
    /// <see cref="MdbxCursor.Put"/>.
    /// Maps to <c>MDBX_put_flags_t</c> in the C header.
    /// </summary>
    [Flags]
    public enum PutFlags : uint
    {
        /// <summary>No special behaviour.</summary>
        None = 0,

        /// <summary>
        /// Don't write if the key already exists
        /// (<c>MDBX_NOOVERWRITE</c>).
        /// </summary>
        NoOverwrite = 0x10,

        /// <summary>
        /// Only for <see cref="DatabaseFlags.DupSort"/>: don't write if the
        /// key/data pair already exists (<c>MDBX_NODUPDATA</c>).
        /// </summary>
        NoDupData = 0x20,

        /// <summary>
        /// Overwrite the current key/data pair at the cursor position
        /// (<c>MDBX_CURRENT</c>).
        /// </summary>
        Current = 0x40,

        /// <summary>
        /// Store the key but reserve space for the data, return a pointer to
        /// the reserved space (<c>MDBX_RESERVE</c>).
        /// </summary>
        Reserve = 0x10000,

        /// <summary>
        /// Data is being appended; the caller guarantees keys are in sort
        /// order (<c>MDBX_APPEND</c>).
        /// </summary>
        Append = 0x20000,

        /// <summary>
        /// Duplicate data is being appended in sort order
        /// (<c>MDBX_APPENDDUP</c>).
        /// </summary>
        AppendDup = 0x40000,
    }
}
