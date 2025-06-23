using FinanceControl.Application.DTOs;

namespace FinanceControl.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(Guid userId, int year, int? month = null);
    }
}
