using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceControl.Application.Services
{
    public class CategoryService : GenericService<Category, CategoryDto>, ICategoryService
    {
        public CategoryService(
            IUnitOfWork uow,
            IGenericRepository<Category> repository,
            IMapper mapper,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor)
            : base(uow, repository, mapper, notificator, httpContextAccessor)
        {
        }
    }
}
