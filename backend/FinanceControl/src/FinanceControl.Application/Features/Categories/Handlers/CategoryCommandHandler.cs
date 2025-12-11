using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Categories.Commands;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using MediatR;

namespace FinanceControl.Application.Features.Categories.Handlers;

public sealed class CategoryCommandHandler :
    IRequestHandler<CreateCategoryCommand, CategoryDto?>,
    IRequestHandler<UpdateCategoryCommand, Unit>,
    IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryService _service;
    private readonly INotificator _notificator;
    private readonly IMapper _mapper;

    public CategoryCommandHandler(ICategoryService service, INotificator notificator, IMapper mapper)
    {
        _service = service;
        _notificator = notificator;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _service.ExistsAsync(c => c.Name.Trim() == request.CategoryDto.Name!.Trim());
        if (exists)
        {
            _notificator.AddNotification(new Notification("Já existe uma categoria com esse nome."));
            return null!;
        }

        return await _service.AddAsync(request.CategoryDto);
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _service.GetByIdAsync(request.CategoryDto.Id.Value, returnEntity: true);
        
        if (category == null)
        {
            _notificator.AddNotification(new Notification("Categoria não encontrada."));
            return Unit.Value;
        }

        _mapper.Map(request.CategoryDto, category);
        await _service.UpdateAsync(category);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _service.GetByIdAsync(request.Id, returnEntity: true);
        if (entity == null)
        {
            _notificator.AddNotification(new Notification("Categoria não encontrada."));
            return Unit.Value;
        }

        await _service.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
