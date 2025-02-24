namespace FinanceControl.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; }  
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }  
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } 
        public Guid UserId { get; set; }  
    }
}
