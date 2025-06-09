namespace FinanceControl.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
        public CategoryBreakdownDto ByCategory { get; set; } = new();
        public IEnumerable<PaymentMethodBalanceDto> ByPaymentMethod { get; set; } = new List<PaymentMethodBalanceDto>();
    }

    public class CategoryBreakdownDto
    {
        public IEnumerable<CategoryAmountDto> Income { get; set; } = new List<CategoryAmountDto>();
        public IEnumerable<CategoryAmountDto> Expense { get; set; } = new List<CategoryAmountDto>();
    }

    public class CategoryAmountDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class PaymentMethodBalanceDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }

}
