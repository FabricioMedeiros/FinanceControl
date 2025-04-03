using FinanceControl.Domain.Entities;
using FinanceControl.Infrastructure.Contexts;

namespace FinanceControl.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
