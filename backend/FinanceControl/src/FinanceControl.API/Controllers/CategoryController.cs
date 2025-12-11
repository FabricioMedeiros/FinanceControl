using FinanceControl.API.Extensions;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Categories.Commands;
using FinanceControl.Application.Features.Categories.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : MainController
    {
        private readonly IMediator _mediator;

        public CategoryController(
            IMediator mediator,
            INotificator notificator) : base(notificator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string>? filters, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(filters,
                                                                        pageNumber,
                                                                        pageSize));
            return CustomResponse(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result == null) return NotFound();

            return CustomResponse(result);
        }

        [HttpPost]
        [ValidationContext(typeof(CategoryCreateValidator))]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(categoryDto));

            return CustomResponse(result);
        }

        [HttpPut("{id:Guid}")]
        [ValidationContext(typeof(CategoryUpdateValidator))]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                NotifyError("O ID informado não é o mesmo que foi passado na query.");
                return CustomResponse();
            }

            await _mediator.Send(new UpdateCategoryCommand(categoryDto));
            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return CustomResponse();
        }
    }
}
