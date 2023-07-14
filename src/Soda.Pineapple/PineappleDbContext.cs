using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Soda.Pineapple.Domain;
using Soda.Pineapple.Services;

namespace Soda.Pineapple;

public class PineappleDbContext<T> : DbContext where T : DbContext
{
    private Lazy<IMultipleTypeBuilderService> MultipleTypeBuilderService => PineappleBuilder.GetService<IMultipleTypeBuilderService>();
    internal DbSet<VirtualTable> VirtualTables { get; set; } = null!;

    public PineappleDbContext(DbContextOptions<T> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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