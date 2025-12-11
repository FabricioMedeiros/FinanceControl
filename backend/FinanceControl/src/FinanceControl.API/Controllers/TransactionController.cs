using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Transactions.Commands;
using FinanceControl.Application.Features.Transactions.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using FinanceControl.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionController : MainController
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator, INotificator notificator) : base(notificator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var result = await _mediator.Send(new GetAllTransactionsQuery(filters,
                                                                          pageNumber,
                                                                          pageSize,
                                                                          Includes: IncludeTransactionRelations()));
            return CustomResponse(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(id,
                                                                          Includes: IncludeTransactionRelations()));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpGet("by-period")]
        public async Task<IActionResult> GetTransactionsByPeriod(
                          [FromQuery] DateTime startDate,
                          [FromQuery] DateTime endDate,
                          [FromQuery] Guid? categoryId = null,
                          [FromQuery] Guid? paymentMethodId = null,
                          [FromQuery] int? pageNumber = null,
                          [FromQuery] int? pageSize = null)
        {

            var result = await _mediator.Send(new GetTransactionsByPeriodQuery(startDate,
                                                                               endDate,
                                                                               categoryId,
                                                                               paymentMethodId,
                                                                               pageNumber,
                                                                               pageSize));
            return CustomResponse(result);
        }


        [HttpPost]
        [ValidationContext(typeof(TransactionCreateValidator))]
        public async Task<IActionResult> Create([FromBody] TransactionDto transactionDto)
        {
            var result = await _mediator.Send(new CreateTransactionCommand(transactionDto,
                                                                           Includes: IncludeTransactionRelations()));

            return CustomResponse(result);
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

            await _mediator.Send(new UpdateTransactionCommand(transactionDto));
            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTransactionCommand(id));
            return CustomResponse();
        }

        private static Func<IQueryable<Transaction>, IQueryable<Transaction>> IncludeTransactionRelations()
        {
            return query => query.Include(t => t.Category)
                                 .Include(t => t.PaymentMethod);
        }
    }
}
