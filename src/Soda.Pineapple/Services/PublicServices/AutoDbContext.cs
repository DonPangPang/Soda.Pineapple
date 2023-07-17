using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Soda.Pineapple.Attributes;
using Soda.Pineapple.Options;

namespace Soda.Pineapple.Services.PublicServices;

public sealed class AutoDbContext<TDbContext> where TDbContext : PineappleDbContext<TDbContext>
{
    private readonly PineappleDbContext<TDbContext> _dbContext;
    
    private Lazy<IOptions<PineappleOptions>> Options => PineappleBuilder.GetService<IOptions<PineappleOptions>>();

    private Lazy<IMultipleTypeBuilderService> MultipleTypeBuilderService =>
        PineappleBuilder.GetService<IMultipleTypeBuilderService>();
    
    
    public AutoDbContext(PineappleDbContext<TDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    public DbContext Db => _dbContext;
    
    public IQueryable<T> Table<T>() where T:class
    {
        if (!typeof(T).GetCustomAttributes<VirtualTableAttribute>().Any())
        {
            return _dbContext.Set<T>();
        }
        
        IQueryable<T>? unionQuery = null;
        
        foreach (var type in MultipleTypeBuilderService.Value.GetTypes<T>())
        {
            if (unionQuery is null)
            {
                unionQuery ??= (GetDbSet(type) as IQueryable<T>);
            }
            else
            {
                unionQuery = unionQuery.Union(GetDbSet(type) as IQueryable<T> ?? throw new InvalidOperationException());
            }
            
        }
        
        return unionQuery ?? _dbContext.Set<T>();
    }

    private object GetDbSet(Type tableType)
    {
        return _dbContext.GetType().GetTypeInfo().GetMethod("Set", Type.EmptyTypes)?.MakeGenericMethod(tableType)
            .Invoke(_dbContext, null) ?? throw new ArgumentNullException(nameof(tableType), $"找不到表 {nameof(tableType)}");
    }
}