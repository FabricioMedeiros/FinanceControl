namespace FinanceControl.Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
