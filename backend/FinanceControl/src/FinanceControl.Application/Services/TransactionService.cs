using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinanceControl.Application.Services
{
    public class TransactionService : GenericService<Transaction, TransactionDto>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(
            IUnitOfWork uow,
            ITransactionRepository transactionRepository,
            IMapper mapper,
            INotificator notificator)
            : base(uow, transactionRepository, mapper, notificator)
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
            Expression<Func<Transaction, bool>> filter = t =>
                t.Date >= startDate && t.Date <= endDate &&
                (!categoryId.HasValue || t.CategoryId == categoryId.Value) &&
                (!paymentMethodId.HasValue || t.PaymentMethodId == paymentMethodId.Value);

            int? skip = null;
            int? take = null;
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                skip = (pageNumber.Value - 1) * pageSize.Value;
                take = pageSize.Value;
            }

            Func<IQueryable<Transaction>, IQueryable<Transaction>> includes = q =>
                q.Include(t => t.Category)
                 .Include(t => t.PaymentMethod);

            var (items, totalRecords) = await _transactionRepository.GetAllAsync(
                filter: filter,
                includes: includes,
                skip: skip,
                take: take
            );

            return new PagedResult<TransactionDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords,
                TotalRecords = totalRecords,
                Items = _mapper.Map<List<TransactionDto>>(items)
            };
        }
    }
}
