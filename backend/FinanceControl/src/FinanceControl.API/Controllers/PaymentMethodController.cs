using AutoMapper;
using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using FinanceControl.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentMethodController : MainController
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper, INotificator notificator) : base(notificator)
        {
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var paymentMethods = await _paymentMethodService.GetAllAsync(filters, pageNumber, pageSize, Guid.Parse(UserId));
            return CustomResponse(paymentMethods);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(id);

            if (paymentMethod == null) return NotFound();

            return CustomResponse(paymentMethod);
        }

        [HttpPost]
        [ValidationContext(typeof(PaymentMethodCreateValidator))]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDto paymentMethodDto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);
            paymentMethod.UserId = Guid.Parse(UserId);

            var createdPaymentMethod = await _paymentMethodService.AddAsync(paymentMethod);
            return CustomResponse(createdPaymentMethod);
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

            var paymentMethod = await _paymentMethodService.GetByIdAsync(paymentMethodDto.Id ?? Guid.Empty, true);

            if (paymentMethod == null)
                return NotFound();

            _mapper.Map(paymentMethodDto, paymentMethod);

            await _paymentMethodService.UpdateAsync(paymentMethod);

            return CustomResponse(paymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _paymentMethodService.GetByIdAsync(id);

            if (category == null) return NotFound();

            await _paymentMethodService.DeleteAsync(id);
            return CustomResponse();
        }
    }
}