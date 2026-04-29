namespace MDBX
{
    using Interop;

    /// <summary>
    /// CursorDelOption.
    /// </summary>
    [Flags]
    public enum CursorDelOption : int
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Нет специальных опций.
        /// </summary>
        Unspecific = None,

        /// <summary>
        /// Enter the new key/data pair only if it does not already appear in the
        /// database. This flag may only be specified if the database was opened
        /// with MDBX_DUPSORT. The function will return MDBX_KEYEXIST if the
        /// key/data pair already appears in the database.
        /// </summary>
        NoDupData = Constant.MDBX_NODUPDATA,
    }
}
