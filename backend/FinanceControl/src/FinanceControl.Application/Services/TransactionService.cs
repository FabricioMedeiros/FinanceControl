using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Application.Services
{
    public class TransactionService : GenericService<Transaction, TransactionDto>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper,
            INotificator notificator)
            : base(transactionRepository, mapper, notificator)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PagedResult<TransactionDto>> GetTransactionsByPeriodAsync(
                          DateTime startDate,
                          DateTime endDate,
                          Guid? categoryId = null,
                          Guid? paymentMethodId = null,
                          int? pageNumber = null,
                          int? pageSize = null)
        {
            var query = _transactionRepository.Query()
                .Include(t => t.Category)
                .Include(t => t.PaymentMethod)
                .Where(t => t.Date >= startDate && t.Date <= endDate);

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            if (paymentMethodId.HasValue)
            {
                query = query.Where(t => t.PaymentMethodId == paymentMethodId.Value);
            }

            var totalCount = await query.CountAsync();
            var transactions = await query.Skip((pageNumber - 1) * pageSize ?? 0)
                                          .Take(pageSize ?? totalCount)
                                          .ToListAsync();

            return new PagedResult<TransactionDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalCount,
                TotalRecords = totalCount,
                Items = _mapper.Map<List<TransactionDto>>(transactions)
            };
        }
    }
}
