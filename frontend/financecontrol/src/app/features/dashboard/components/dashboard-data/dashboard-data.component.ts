import { Component, OnInit } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { DashboardService } from '../../services/dashboard.service';
import { Dashboard } from '../../models/dashboard';

@Component({
  selector: 'app-dashboard-data',
  templateUrl: './dashboard-data.component.html',
  styleUrls: ['./dashboard-data.component.css']
})
export class DashboardDataComponent implements OnInit {
  dashboardData: Dashboard | null = null;

  months = [
    { value: 1, name: 'Janeiro' }, { value: 2, name: 'Fevereiro' }, { value: 3, name: 'Março' },
    { value: 4, name: 'Abril' }, { value: 5, name: 'Maio' }, { value: 6, name: 'Junho' },
    { value: 7, name: 'Julho' }, { value: 8, name: 'Agosto' }, { value: 9, name: 'Setembro' },
    { value: 10, name: 'Outubro' }, { value: 11, name: 'Novembro' }, { value: 12, name: 'Dezembro' }
  ];

  years: number[] = [];
  selectedMonth: number | null = null;
  selectedYear: number = new Date().getFullYear();

  incomeChartData: ChartData<'bar'> = { labels: [], datasets: [{ data: [] }] };
  expenseChartData: ChartData<'bar'> = { labels: [], datasets: [{ data: [] }] };
  paymentMethodChartData: ChartData<'bar'> = { labels: [], datasets: [{ data: [] }] };
  expenseByPaymentMethodChartData: ChartData<'bar'> = { labels: [], datasets: [{ data: [] }] };
  
  chartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      tooltip: {
        callbacks: {
          label: (tooltipItem) => {
            const value = tooltipItem.raw as number;
            return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
          }
        }
      }
    },
    scales: {
      y: {
        ticks: {
          callback: (value) => {
            return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value as number);
          }
        }
      }
    }
  };

  constructor(private dashboardService: DashboardService) { }

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: currentYear - 1999 + 1 }, (_, i) => currentYear - i);

    this.selectedMonth = new Date().getMonth() + 1;
    this.selectedYear = currentYear;

    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.dashboardService.getDashboardData(this.selectedYear, this.selectedMonth ?? undefined)
      .subscribe(response => {
        if (response.success && response.data) {
          this.dashboardData = response.data;
          this.prepareCharts(); 
        } else {
          console.error("Erro: Resposta inesperada da API", response);
        }
      });
  }

  onMonthChange(selectedValue: string | number | null): void {
    this.selectedMonth = selectedValue === null || selectedValue === "" ? null : Number(selectedValue);
    this.loadDashboardData();
  }
  onYearChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedYear = Number(target.value);
    this.loadDashboardData();
  }

  prepareCharts(): void {
  if (!this.dashboardData) return;

  const incomebyCategoryData = this.dashboardData.incomeByCategory ?? [];
  const expensebyCategoryData = this.dashboardData.expenseByCategory ?? [];
  const expenseByMethodData = this.dashboardData.expensesByPaymentMethod ?? [];
  const paymentData = this.dashboardData.paymentMethodBalances ?? [];

  this.incomeChartData = {
    labels: incomebyCategoryData.map(i => i.category),
    datasets: [{
      data: incomebyCategoryData.map(i => i.amount),
      label: "Receitas por Categoria",
      backgroundColor: ["#4CAF50", "#8BC34A", "#CDDC39"],
      borderColor: "#388E3C",
      borderWidth: 2
    }]
  };

  this.expenseChartData = {
    labels: expensebyCategoryData.map(e => e.category),
    datasets: [{
      data: expensebyCategoryData.map(e => e.amount),
      label: "Despesas por Categoria",
      backgroundColor: ["#F44336", "#E57373", "#FFCDD2"], 
      borderColor: "#D32F2F",
      borderWidth: 2
    }]
  };

  this.paymentMethodChartData = {
    labels: paymentData.map(p => p.paymentMethod),
    datasets: [{
      data: paymentData.map(p => p.balance),
      label: "Saldo por Método de Pagamento",
      backgroundColor: ["#2196F3", "#64B5F6", "#BBDEFB"], 
      borderColor: "#1976D2",
      borderWidth: 2
    }]
  };

  this.expenseByPaymentMethodChartData = {
    labels: expenseByMethodData.map(e => e.paymentMethod),
    datasets: [{
      data: expenseByMethodData.map(e => e.totalExpense),
      label: "Despesas por Método de Pagamento",
      backgroundColor: ["#FF9800", "#FFB74D", "#FFE0B2"],
      borderColor: "#F57C00",
      borderWidth: 2
    }]
  };
}

}