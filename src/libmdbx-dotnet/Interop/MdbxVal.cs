using System;
using System.Runtime.InteropServices;

namespace Libmdbx.Interop
{
    /// <summary>
    /// Maps to the <c>MDBX_val</c> / <c>iov</c> C struct used for all
    /// key/value data passed to and from the native library.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MdbxVal
    {
        /// <summary>Pointer to the data bytes.</summary>
        internal IntPtr iov_base;

        /// <summary>Length of the data in bytes.</summary>
        internal UIntPtr iov_len;

        internal int Length => (int)iov_len;

        internal MdbxVal(IntPtr address, int length)
        {
            iov_base = address;
            iov_len = (UIntPtr)length;
        }
    }
}
