using System.Reflection;
using Soda.Pineapple.Attributes;

namespace Soda.Pineapple.Tests.Services;

public class VirtualTableAttributeTests
{
    [VirtualTable(nameof(VirtualTableA))]
    public class VirtualTableA
    {
    }

    [Fact]
    public void Test()
    {
        var name = typeof(VirtualTableA).GetCustomAttribute<VirtualTableAttribute>()?.Name;

        Assert.Equal(nameof(VirtualTableA), name, StringComparer.OrdinalIgnoreCase);
    }
}