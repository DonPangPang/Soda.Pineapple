using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Soda.Pineapple.Generators;

namespace Soda.Pineapple.Actions;

internal class CreateTableAction
{
    private Lazy<PineappleDbContext> PineappleDbContext => PineappleBuilder.GetService<PineappleDbContext>();
    private Lazy<CreateTableGenerator> CreateTableGenerator => PineappleBuilder.GetService<CreateTableGenerator>();
    public async Task Create(IEntityType entityType)
    {
        var service = PineappleDbContext.Value.GetService<IMigrationsSqlGenerator>();

        var operation = CreateTableGenerator.Value.Create(entityType);

        var cmds = service.Generate(new[] { operation }).Select(x=>x.CommandText);

        await using var command = PineappleDbContext.Value.Database.GetDbConnection().CreateCommand();
        await PineappleDbContext.Value.Database.OpenConnectionAsync();

        foreach (var cmd in cmds)
        {
            command.CommandText = cmd;
            await command.ExecuteScalarAsync();
        }
    }
}