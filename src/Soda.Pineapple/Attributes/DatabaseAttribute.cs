using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Soda.Pineapple.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DatabaseAttribute<TDbContext> : Attribute where TDbContext : DbContext
{
    internal Type Table { get; private set; }

    public DatabaseAttribute(Type table)
    {
        Table = table;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DatabaseAttribute<TDbContext, TTable> : Attribute where TDbContext : DbContext where TTable : class, new()
{
    internal Type DbContext => typeof(TDbContext);
    public Type Table => typeof(TTable);
}