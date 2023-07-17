using Microsoft.EntityFrameworkCore;

namespace Soda.Pineapple.Sample.Data;

public class SampleDbContext:PineappleDbContext<SampleDbContext>
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }
}