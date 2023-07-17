using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Soda.Pineapple.Generators;

internal class CreateTableGenerator
{
    public CreateTableOperation Create(IEntityType entityType)
    {
        var creator = new CreateTableOperation()
        {
            Name = entityType.GetTableName() ?? entityType.ClrType.Name,
        };
        creator.Columns.AddRange(entityType.GetProperties()
            .Select(p => new AddColumnOperation
            {
                Name = p.GetColumnName(),
                ColumnType = p.GetColumnType(),
                DefaultValue = p.GetDefaultValue(),
                IsNullable = p.IsNullable,
                MaxLength = p.GetMaxLength(),
                IsUnicode = p.IsUnicode(),
                Comment = p.GetComment()
            }));

        creator.PrimaryKey = new AddPrimaryKeyOperation()
        {
            Name = $"PK_{entityType.GetTableName() ?? entityType.ClrType.Name}",
            Columns = entityType.GetProperties().Where(x=>x.IsPrimaryKey()).Select(x=>x.Name).ToArray(),
        };
        return creator;
    }
}