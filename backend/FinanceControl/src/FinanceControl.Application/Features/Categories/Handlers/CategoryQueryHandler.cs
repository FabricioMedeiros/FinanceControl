using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Categories.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.Categories.Handlers
{

    public sealed class CategoryQueryHandler :
        IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryDto>>,
        IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryService _service;

        public CategoryQueryHandler(ICategoryService service)
        {
            _service = service;
        }

        public async Task<PagedResult<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(
                request.Filters,
                request.PageNumber,
                request.PageSize
            );
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id);
        }
    }
}
