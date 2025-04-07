using AutoMapper;
using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using FinanceControl.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionController : MainController
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var transactions = await _transactionService.GetAllAsync(filters,
                                                                     pageNumber,
                                                                     pageSize,
                                                                     Guid.Parse(UserId),
                                                                     includes: new Expression<Func<Transaction, object>>[]
                                                                     {
                                                                        x => x.Category,
                                                                        x => x.PaymentMethod
                                                                     });

            return CustomResponse(transactions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);

            if (transaction == null) return NotFound();

            return CustomResponse(transaction);
        }

        [HttpPost]
        [ValidationContext(typeof(TransactionCreateValidator))]
        public async Task<IActionResult> Create([FromBody] TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.UserId = Guid.Parse(UserId);

            var createdTransaction = await _transactionService.AddAsync(transaction);
            return CustomResponse(createdTransaction);
        }
        
        [HttpPut("{id:Guid}")]
        [ValidationContext(typeof(TransactionUpdateValidator))]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionDto transactionDto)
        {
            if (id != transactionDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            var transaction = await _transactionService.GetByIdAsync(transactionDto.Id ?? Guid.Empty, true);

            if (transaction == null)
                return NotFound();

            _mapper.Map(transactionDto, transaction);

            await _transactionService.UpdateAsync(transaction);

            return CustomResponse(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);

            if (transaction == null) return NotFound();

            await _transactionService.DeleteAsync(id);
            return CustomResponse();
        }
    }
}
