using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;

namespace FinanceControl.Application.Services
{
    public class CategoryService : GenericService<Category, CategoryDto>, ICategoryService
    {
        public CategoryService(
            IGenericRepository<Category> repository,
            IMapper mapper,
            INotificator notificator)
            : base(repository, mapper, notificator)
        {
        }
    }
}
