using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.PaymentMethods.Commands;
using FinanceControl.Application.Features.PaymentMethods.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentMethodController : MainController
    {
        private readonly IMediator _mediator;

        public PaymentMethodController(
            IMediator mediator,
            INotificator notificator) : base(notificator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var result = await _mediator.Send(new GetAllPaymentMethodsQuery(filters,
                                                                            pageNumber,
                                                                            pageSize));
            return CustomResponse(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPaymentMethodByIdQuery(id));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        [ValidationContext(typeof(PaymentMethodCreateValidator))]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDto paymentMethodDto)
        {
            var result = await _mediator.Send(new CreatePaymentMethodCommand(paymentMethodDto));

            return CustomResponse(result);
        }

        [HttpPut("{id:Guid}")]
        [ValidationContext(typeof(PaymentMethodUpdateValidator))]
        public async Task<IActionResult> Update(Guid id, [FromBody] PaymentMethodDto paymentMethodDto)
        {
            if (id != paymentMethodDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdatePaymentMethodCommand(paymentMethodDto));
            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletePaymentMethodCommand(id));
            return CustomResponse();
        }
    }
}