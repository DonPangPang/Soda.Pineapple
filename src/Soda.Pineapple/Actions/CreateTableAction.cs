using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Soda.Pineapple.Actions;

internal class CreateTableAction
{
    public TableInfo Create(IEntityType entityType)
    {
        var tableName = entityType.GetTableName() ?? entityType.ClrType.Name;

        // 获取列信息
        var columns = entityType.GetProperties()
            .Select(p => new ColumnInfo
            {
                Name = p.GetColumnName(),
                Type = p.GetColumnType(),
                IsNullable = p.IsNullable,
                MaxLength = p.GetMaxLength(),
                IsPrimaryKey = p.IsPrimaryKey(),
                Comment = p.GetComment()
            })
            .ToList();

        return new TableInfo()
        {
            Name = tableName,
            ColumnInfos = columns
        };
    }
}

internal class TableInfo
{
    public required string Name { get; set; }
    
    public required List<ColumnInfo> ColumnInfos { get; set; }
}

internal class ColumnInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsNullable { get; set; } = true;
    public int? MaxLength { get; set; }
    public bool IsPrimaryKey { get; set; }
    public string? Comment { get; set; }
}