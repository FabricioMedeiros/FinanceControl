using FinanceControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : MainController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService, INotificator notificator)
            : base(notificator)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] int year, [FromQuery] int? month = null)
        {
            var result = await _dashboardService.GetDashboardDataAsync(year, month);
            return CustomResponse(result);
        }
    }
}
