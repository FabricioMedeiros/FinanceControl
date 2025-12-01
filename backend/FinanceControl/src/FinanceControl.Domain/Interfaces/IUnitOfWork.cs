namespace FinanceControl.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        ITransactionRepository Transactions { get; }
        Task<int> CommitAsync();
    }
}
