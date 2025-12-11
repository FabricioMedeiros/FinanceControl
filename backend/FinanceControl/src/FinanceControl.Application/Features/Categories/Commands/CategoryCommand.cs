using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(CategoryDto CategoryDto) : IRequest<CategoryDto?>;
public sealed record UpdateCategoryCommand(CategoryDto CategoryDto) : IRequest<Unit>;
public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;


