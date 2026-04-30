using System.Runtime.InteropServices;

namespace MDBX.Interop
{
    /// <summary>
    /// Представляет значение базы данных (пару ключ/значение) для передачи между managed и unmanaged кодом.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DataBaseValue
    {
        /// <summary>
        /// Адрес данных в памяти.
        /// </summary>
        internal IntPtr Address { get; }

        /// <summary>
        /// Длина данных в байтах.
        /// </summary>
        private readonly IntPtr _size;

        /// <summary>
        /// Получает длину данных.
        /// </summary>
        internal int Length { get { return _size.ToInt32(); } }

        /// <summary>
        /// Инициализирует новый экземпляр структуры DbValue.
        /// </summary>
        /// <param name="addr">Адрес данных.</param>
        /// <param name="length">Длина данных в байтах.</param>
        internal DataBaseValue(IntPtr addr, int length)
        {
            this.Address = addr;
            this._size = IntPtr.Add(IntPtr.Zero, length);
        }
    }
}
