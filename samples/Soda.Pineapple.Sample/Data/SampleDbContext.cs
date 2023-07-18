using Microsoft.EntityFrameworkCore;
using Soda.Pineapple.Sample.Domain;

namespace Soda.Pineapple.Sample.Data;

public class SampleDbContext:PineappleDbContext<SampleDbContext>
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }
}