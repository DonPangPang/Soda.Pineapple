using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Soda.Pineapple.Attributes;

/// <summary>
/// 虚拟表
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class VirtualTableAttribute : TableAttribute
{
    public VirtualTableAttribute(string name) : base(name)
    {
    }
}