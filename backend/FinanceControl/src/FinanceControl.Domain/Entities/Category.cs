using FinanceControl.Domain.Enums;

namespace FinanceControl.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryType Type { get; set; }  
    }
}
