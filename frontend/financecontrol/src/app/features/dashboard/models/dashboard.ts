export interface Dashboard {
  incomeByCategory: CategoryAmount[];
  expenseByCategory: CategoryAmount[];
  expensesByPaymentMethod: PaymentMethodExpense[];
  paymentMethodBalances: PaymentMethodBalance[];
}

export interface CategoryAmount {
  category: string;
  amount: number;
}

export interface PaymentMethodExpense {
  paymentMethod: string;
  totalExpense: number;
}

export interface PaymentMethodBalance {
  paymentMethod: string;
  balance: number;
}
