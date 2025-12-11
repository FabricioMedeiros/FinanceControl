namespace FinanceControl.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
