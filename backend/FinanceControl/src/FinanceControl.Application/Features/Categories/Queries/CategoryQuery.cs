using MediatR;
using FinanceControl.Domain.Entities;
using FinanceControl.Application.DTOs;

namespace FinanceControl.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(
    Dictionary<string, string>? Filters,
    int? PageNumber,
    int? PageSize,
    Func<IQueryable<Category>, IQueryable<Category>>? Includes = null
) : IRequest<PagedResult<CategoryDto>>;

public sealed record GetCategoryByIdQuery(
    Guid Id,
    Func<IQueryable<Category>, IQueryable<Category>>? Includes = null
) : IRequest<CategoryDto?>;