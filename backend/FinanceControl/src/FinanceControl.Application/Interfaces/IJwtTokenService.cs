using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
