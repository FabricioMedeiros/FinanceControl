using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.Features.PaymentMethods.Commands;

public sealed record CreatePaymentMethodCommand(PaymentMethodDto PaymentMethodDto) : IRequest<PaymentMethodDto?>;
public sealed record UpdatePaymentMethodCommand(PaymentMethodDto PaymentMethodDto) : IRequest<Unit>;
public sealed record DeletePaymentMethodCommand(Guid Id) : IRequest<Unit>;
