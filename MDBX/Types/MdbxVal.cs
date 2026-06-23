using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Структура для передачи ключей и данных в таблицах MDBX.
/// </summary>
/// <remarks>
/// Значения, возвращаемые из таблицы, действительны только до следующей
/// операции обновления или конца транзакции. Не изменяйте и не освобождайте их,
/// так как они обычно указывают прямо в базу данных.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MdbxVal
{
    /// <summary>
    /// Указатель на данные.
    /// </summary>
    public void* IovBase;

    /// <summary>
    /// Длина данных в байтах.
    /// </summary>
    public nuint IovLen;

    /// <summary>
    /// Инициализирует новую структуру MdbxVal.
    /// </summary>
    /// <param name="pointer">Указатель на данные.</param>
    /// <param name="length">Длина данных в байтах.</param>
    public MdbxVal(void* pointer, nuint length)
    {
        IovBase = pointer;
        IovLen = length;
    }

    /// <summary>
    /// Инициализирует новую структуру MdbxVal из управляемого массива байт.
    /// </summary>
    /// <param name="data">Массив байт.</param>
    public MdbxVal(byte[] data)
    {
        if (data == null)
        {
            IovBase = null;
            IovLen = 0;
            return;
        }

        fixed (byte* ptr = data)
        {
            IovBase = ptr;
            IovLen = (nuint)data.Length;
        }
    }

    /// <summary>
    /// Получает данные в видеSpan byte.
    /// </summary>
    public Span<byte> AsSpan()
    {
        return new Span<byte>((byte*)IovBase, (int)IovLen);
    }
}
