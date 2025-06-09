using FinanceControl.Application.DTOs;

namespace FinanceControl.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(int year, int? month = null);
    }
}
