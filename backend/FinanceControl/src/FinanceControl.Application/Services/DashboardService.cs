using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Enums;
using FinanceControl.Domain.Interfaces;

namespace FinanceControl.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly INotificator _notificator;

        public DashboardService(IDashboardRepository dashboardRepository, INotificator notificator)
        {
            _dashboardRepository = dashboardRepository;
            _notificator = notificator;
        }

        public async Task<DashboardDto> GetDashboardDataAsync(int year, int? month = null)
        {
            var transactions = await _dashboardRepository.GetTransactionsAsync(year, month);

            var income = transactions
                .Where(t => t.Category.Type == CategoryType.Income)
                .ToList();

            var expense = transactions
                .Where(t => t.Category.Type == CategoryType.Expense)
                .ToList();

            var totalIncome = income.Sum(t => t.Amount);
            var totalExpense = expense.Sum(t => t.Amount);

            var categoryIncome = income
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryAmountDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToList();

            var categoryExpense = expense
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryAmountDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToList();

            var paymentMethodBalances = transactions
                .GroupBy(t => t.PaymentMethod.Name)
                .Select(g => new PaymentMethodBalanceDto
                {
                    PaymentMethod = g.Key,
                    Balance = g.Sum(t =>
                        t.Category.Type == CategoryType.Income ? t.Amount : -t.Amount)
                })
                .ToList();

            return new DashboardDto
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                ByCategory = new CategoryBreakdownDto
                {
                    Income = categoryIncome,
                    Expense = categoryExpense
                },
                ByPaymentMethod = paymentMethodBalances
            };
        }
    }
}
