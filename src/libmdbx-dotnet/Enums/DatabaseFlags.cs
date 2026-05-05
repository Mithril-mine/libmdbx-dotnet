namespace Libmdbx
{
    /// <summary>
    /// Flags for <see cref="MdbxTransaction.OpenDatabase"/>.
    /// Maps to <c>MDBX_db_flags_t</c> in the C header.
    /// </summary>
    [Flags]
    public enum DatabaseFlags : uint
    {
        /// <summary>No special flags.</summary>
        None = 0,

        /// <summary>Use reverse string comparison for keys.</summary>
        ReverseKey = 0x02,

        /// <summary>
        /// Allow duplicate data items for a single key
        /// (<c>MDBX_DUPSORT</c>).
        /// </summary>
        DupSort = 0x04,

        /// <summary>
        /// Keys are binary integers in native byte order
        /// (<c>MDBX_INTEGERKEY</c>).
        /// </summary>
        IntegerKey = 0x08,

        /// <summary>
        /// With <see cref="DupSort"/>, sort duplicate data as binary integers
        /// (<c>MDBX_DUPFIXED</c>).
        /// </summary>
        DupFixed = 0x10,

        /// <summary>
        /// With <see cref="DupSort"/>, duplicate data items are binary integers
        /// (<c>MDBX_INTEGERDUP</c>).
        /// </summary>
        IntegerDup = 0x20,

        /// <summary>
        /// With <see cref="DupSort"/>, use reverse string comparison for data
        /// (<c>MDBX_REVERSEDUP</c>).
        /// </summary>
        ReverseDup = 0x40,

        /// <summary>
        /// Create the named database if it doesn't exist
        /// (<c>MDBX_CREATE</c>).
        /// </summary>
        Create = 0x40000,
    }
}
