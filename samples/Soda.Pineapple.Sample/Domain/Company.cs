using Soda.Pineapple.Attributes;

namespace Soda.Pineapple.Sample.Domain;

[VirtualTable(nameof(Company))]
public class Company : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Description { get; set; }

    public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
}