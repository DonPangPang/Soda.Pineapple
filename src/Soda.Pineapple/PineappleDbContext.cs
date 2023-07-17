using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Soda.Pineapple.Attributes;
using Soda.Pineapple.Domain;
using Soda.Pineapple.Options;
using Soda.Pineapple.Services;

namespace Soda.Pineapple;

internal class PineappleDbContext : PineappleDbContext<PineappleDbContext>
{
    internal DbSet<VirtualTable> VirtualTables { get; set; } = null!;
    public PineappleDbContext(DbContextOptions<PineappleDbContext> options) : base(options)
    {
    }
}

public class PineappleDbContext<T> : DbContext where T : DbContext
{
    private Lazy<IMultipleTypeBuilderService> MultipleTypeBuilderService => PineappleBuilder.GetService<IMultipleTypeBuilderService>();
    
    private Lazy<IOptions<PineappleOptions>> Options => PineappleBuilder.GetService<IOptions<PineappleOptions>>();

    public PineappleDbContext(DbContextOptions<T> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var item in modelBuilder.Model.GetEntityTypes()
                     .Where(x=>x.ClrType.GetCustomAttributes<VirtualTableAttribute>().Any()))
        {
            modelBuilder.Entity(item.Name).ToTable($"{item.Name}_{Options.Value.Value.SplittingRule.GetSuffix()}");
        }
        
        foreach (var type in MultipleTypeBuilderService.Value.GetTypes())
        {
            var entityType = modelBuilder.Model.FindEntityType(type);
            if (entityType is null)
            {
                modelBuilder.Model.AddEntityType(type);
            }
            modelBuilder.Entity(type).HasBaseType((Type)null!);
        }
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

        base.OnConfiguring(optionsBuilder);
    }
}