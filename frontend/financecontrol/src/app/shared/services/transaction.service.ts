import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { GenericCrudService } from "src/app/core/services/generic-crud.service";
import { Transaction } from "src/app/features/transaction/models/transaction";
import { catchError, Observable } from "rxjs";

@Injectable()
export class TransactionService extends GenericCrudService<Transaction> {

    constructor(protected override http: HttpClient) {
        super(http, 'Transaction', 'transaction');
    }

    getTransactionsByPeriod(
        startDate: string,
        endDate: string,
        categoryId?: string,
        paymentMethodId?: string,
        page?: number,
        pageSize?: number
    ): Observable<any> {
        const headers = this.getAuthHeaderJson();

        let url = `${this.UrlServiceV1}transaction/by-period?startDate=${startDate}&endDate=${endDate}`;

        if (categoryId) {
            url += `&categoryId=${categoryId}`;
        }

        if (paymentMethodId) {
            url += `&paymentMethodId=${paymentMethodId}`;
        }

        if (page !== undefined && pageSize !== undefined) {
            url += `&pageNumber=${page}&pageSize=${pageSize}`;
        }

        return this.http.get<any>(url, headers).pipe(
            catchError(super.serviceError)
        );
    }

    saveTransactionSearchFilters(pageSize: number, category: string, paymentMethod: string, startDate: Date, endDate: Date): void {
        if (pageSize) localStorage.setItem('pageSizeTransactionList', pageSize.toString());
        if (startDate) localStorage.setItem('startDateFilterTransactionList', startDate.toString());
        if (endDate) localStorage.setItem('endDateFilterTransactionList', endDate.toString());
        if (category) localStorage.setItem('categoryFilterTransactionList', category);
        if (paymentMethod) localStorage.setItem('paymentMethodFilterTransactionList', paymentMethod);
    }

    getTransactionSearchFilters(): { pageSize?: number, category?: string, paymentMethod?: string, startDate?: Date, endDate?: Date } {
        const pageSizeValue = localStorage.getItem('pageSizeTransactionList');
        const categoryValue = localStorage.getItem('categoryFilterTransactionList');
        const paymentMethodValue = localStorage.getItem('paymentMethodFilterTransactionList');
        const startDateValue = localStorage.getItem('startDateFilterTransactionList');
        const endDateValue = localStorage.getItem('endDateFilterTransactionList');

        return {
            pageSize: pageSizeValue ? parseInt(pageSizeValue, 10) : undefined,
            category: categoryValue ? categoryValue : undefined,
            paymentMethod: paymentMethodValue ? paymentMethodValue : undefined,
            startDate: startDateValue ? new Date(startDateValue) : undefined,
            endDate: endDateValue ? new Date(endDateValue) : undefined
        };
    }

    clearTransactionSearchFilters(): void {
        localStorage.removeItem('pageSizeTransactionList');
        localStorage.removeItem('startDateFilterTransactionList');
        localStorage.removeItem('endDateFilterTransactionList');
        localStorage.removeItem('categoryFilterTransactionList');
        localStorage.removeItem('paymentMethodFilterTransactionList');
    }
}