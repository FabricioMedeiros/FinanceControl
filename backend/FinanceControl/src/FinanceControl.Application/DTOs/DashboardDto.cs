namespace FinanceControl.Application.DTOs
{
    public class DashboardDto
    {
        public IEnumerable<CategoryAmountDto> IncomeByCategory { get; set; } = new List<CategoryAmountDto>();
        public IEnumerable<CategoryAmountDto> ExpenseByCategory { get; set; } = new List<CategoryAmountDto>();
        public IEnumerable<PaymentMethodExpenseDto> ExpensesByPaymentMethod { get; set; } = new List<PaymentMethodExpenseDto>();
        public IEnumerable<PaymentMethodBalanceDto> PaymentMethodBalances { get; set; } = new List<PaymentMethodBalanceDto>();
    }

    public class CategoryAmountDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class PaymentMethodExpenseDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalExpense { get; set; }
    }

    public class PaymentMethodBalanceDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
