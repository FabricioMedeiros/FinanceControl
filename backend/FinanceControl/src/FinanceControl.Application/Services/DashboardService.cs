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

        public async Task<DashboardDto> GetDashboardDataAsync(Guid userId, int year, int? month = null)
        {
            var periodTransactions = await _dashboardRepository.GetTransactionsByPeriodAsync(userId, year, month);

            var income = periodTransactions
                .Where(t => t.Category.Type == CategoryType.Income)
                .ToList();

            var expense = periodTransactions
                .Where(t => t.Category.Type == CategoryType.Expense)
                .ToList();

            var incomeByCategory = income
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryAmountDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToList();

            var expenseByCategory = expense
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryAmountDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .ToList();

            var expensesByPaymentMethod = expense
                .GroupBy(t => t.PaymentMethod.Name)
                .Select(g => new PaymentMethodExpenseDto
                {
                    PaymentMethod = g.Key,
                    TotalExpense = g.Sum(t => t.Amount)
                })
                .ToList();

            var allTransactions = await _dashboardRepository.GetTransactionsForPaymentMethodBalanceAsync(userId);

            var paymentMethodBalances = allTransactions
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
                IncomeByCategory = incomeByCategory,
                ExpenseByCategory = expenseByCategory,
                ExpensesByPaymentMethod = expensesByPaymentMethod,
                PaymentMethodBalances = paymentMethodBalances
            };
        }
    }
}
