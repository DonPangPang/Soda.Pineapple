using Microsoft.EntityFrameworkCore;
using Soda.Pineapple.Domain;

namespace Soda.Pineapple.Services;

internal interface IVirtualTableService
{
    Task<IEnumerable<VirtualTable>> Get();
    Task<IEnumerable<VirtualTable>> Get(Type type);
    Task<bool> Set(Type mainTableType, string virtualTableName);
    Task<bool> Set(Type mainTableType, Type virtualTableType);
}

internal class VirtualTableService:IVirtualTableService
{
    private Lazy<PineappleDbContext> PineappleDbContext => PineappleBuilder.GetService<PineappleDbContext>();

    public VirtualTableService()
    {
    }

    public async Task<IEnumerable<VirtualTable>> Get()
    {
        return await PineappleDbContext.Value.VirtualTables.ToListAsync();
    }

    public async Task<IEnumerable<VirtualTable>> Get(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        return await PineappleDbContext.Value.VirtualTables.Where(x => x.MainTableName == nameof(type)).ToListAsync();
    }

    public async Task<bool> Set(Type mainTableType, string virtualTableName)
    {
        if (mainTableType == null) throw new ArgumentNullException(nameof(mainTableType));
        if(string.IsNullOrWhiteSpace(virtualTableName)) throw new ArgumentNullException(nameof(virtualTableName));
        PineappleDbContext.Value.Add(new VirtualTable()
        {
            MainTableName = nameof(mainTableType),
            VirtualTableName = virtualTableName
        });
        
        return await PineappleDbContext.Value.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> Set(Type mainTableType, Type virtualTableType)
    {
        if (mainTableType == null) throw new ArgumentNullException(nameof(mainTableType));
        if (virtualTableType == null) throw new ArgumentNullException(nameof(virtualTableType));

        PineappleDbContext.Value.Add(new VirtualTable()
        {
            MainTableName = nameof(mainTableType),
            VirtualTableName = nameof(virtualTableType),
        });
        
        return await PineappleDbContext.Value.SaveChangesAsync() > 0;
    }
}