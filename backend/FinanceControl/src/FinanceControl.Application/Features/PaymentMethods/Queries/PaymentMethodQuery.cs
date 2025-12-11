using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.PaymentMethods.Queries;

public sealed record GetAllPaymentMethodsQuery(
Dictionary<string, string>? Filters,
int? PageNumber,
int? PageSize,
Func<IQueryable<PaymentMethod>, IQueryable<PaymentMethod>>? Includes = null
) : IRequest<PagedResult<PaymentMethodDto>>;

public sealed record GetPaymentMethodByIdQuery(
    Guid Id,
    Func<IQueryable<PaymentMethod>, IQueryable<PaymentMethod>>? Includes = null
) : IRequest<PaymentMethodDto?>;
