using System;
using System.Runtime.InteropServices;
using Xunit;

namespace MDBX.UnitTest;

/// <summary>
/// Помечает тест как предназначенный только для Windows. На других платформах
/// (Linux/macOS) тест пропускается, поскольку использует Windows-only нативные
/// функции (варианты с суффиксом W и ANSI2OEM), которые отсутствуют в сборке libmdbx.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class WindowsOnlyFactAttribute : FactAttribute
{
    public WindowsOnlyFactAttribute()
    {
        if (!OperatingSystem.IsWindows())
            Skip = "Пропущено: тест использует Windows-only нативные функции (W/ANSI2OEM), недоступные на данной платформе.";
    }
}

/// <summary>
/// Аналог <see cref="WindowsOnlyFactAttribute"/> для теоретических тестов.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class WindowsOnlyTheoryAttribute : TheoryAttribute
{
    public WindowsOnlyTheoryAttribute()
    {
        if (!OperatingSystem.IsWindows())
            Skip = "Пропущено: тест использует Windows-only нативные функции (W/ANSI2OEM), недоступные на данной платформе.";
    }
}
