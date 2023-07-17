using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Soda.Pineapple.Domain;

[Table("__VirtualTable")]
[Index(nameof(MainTableName))]
public class VirtualTable
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 主表
    /// </summary>
    [MaxLength(500)]
    [Required]
    public string MainTableName { get; set; } = string.Empty;

    /// <summary>
    /// 虚拟表
    /// </summary>
    [MaxLength(500)]
    [Required]
    public string VirtualTableName { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; } = DateTime.Now;
}