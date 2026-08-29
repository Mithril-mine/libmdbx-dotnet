using System.Runtime.InteropServices;

namespace MDBX.Native.Types;

/// <summary>
/// Структура-маяк (canary) для отслеживания изменений окружения.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MdbxCanary
{
    /// <summary>
    /// Первое значение маркера.
    /// </summary>
    public ulong X;

    /// <summary>
    /// Второе значение маркера.
    /// </summary>
    public ulong Y;

    /// <summary>
    /// Третье значение маркера.
    /// </summary>
    public ulong Z;

    /// <summary>
    /// Четвертое значение маркера (ID транзакции).
    /// </summary>
    public ulong V;
}
