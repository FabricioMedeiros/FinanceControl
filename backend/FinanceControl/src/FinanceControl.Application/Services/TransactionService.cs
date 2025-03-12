using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;

namespace FinanceControl.Application.Services
{
    public class TransactionService : GenericService<Transaction, TransactionDto>, ITransactionService
    {
        public TransactionService(
            IGenericRepository<Transaction> repository,
            IMapper mapper,
            INotificator notificator)
            : base(repository, mapper, notificator)
        {
        }
    }
}
