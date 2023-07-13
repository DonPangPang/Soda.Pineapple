using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Soda.Pineapple.Domain;
using Soda.Pineapple.Services;

namespace Soda.Pineapple;

public class PineappleDbContext<T> : DbContext where T : DbContext
{
    private readonly IMultipleTypeBuilderService _multipleTypeBuilderService;
    internal DbSet<VirtualTable> VirtualTables { get; set; } = null!;

    public PineappleDbContext(DbContextOptions<T> options, IMultipleTypeBuilderService multipleTypeBuilderService) : base(options)
    {
        _multipleTypeBuilderService = multipleTypeBuilderService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var type in _multipleTypeBuilderService.GetTypes())
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