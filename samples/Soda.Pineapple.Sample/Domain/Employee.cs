using System.ComponentModel.DataAnnotations.Schema;

namespace Soda.Pineapple.Sample.Domain;

public class Employee:EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? Born { get; set; }

    [NotMapped] public int Age => (DateTime.Now.Year - Born?.Year) ?? 0;
    
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}