import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

import { BaseService } from "src/app/core/services/base.service";
import { Transaction } from "src/app/features/transaction/models/transaction";

@Injectable()
export class TransactionService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAll(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        let url = `${this.UrlServiceV1}transaction`;

        if (page !== undefined && pageSize !== undefined) {
            url += `?pageNumber=${page}&pageSize=${pageSize}`;
        }

        if (field && value) {
            url += `${(page !== undefined && pageSize !== undefined) ? '&' : '?'}filters[${field}]=${value}`;
        }

        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }

    getTransactionsByPeriod(
        startDate: string,
        endDate: string,
        categoryId?: string,
        paymentMethodId?: string,
        page?: number,
        pageSize?: number
    ): Observable<any> {
        const headers = this.GetAuthHeaderJson();

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

    getTransactionById(id: string): Observable<Transaction> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Transaction>(`${this.UrlServiceV1}transaction/${id}`, headers)
            .pipe(catchError(super.serviceError));
    }

    registerTransaction(transaction: Transaction): Observable<Transaction> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Transaction>(`${this.UrlServiceV1}transaction`, transaction, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateTransaction(transaction: Transaction): Observable<Transaction> {
        const headers = this.GetAuthHeaderJson();
        const httpOptions = {
            headers: headers
        };

        return this.http
            .put<Transaction>(`${this.UrlServiceV1}transaction/${transaction.id}`, transaction, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    delete(id: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}transaction/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageTransactionList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageTransactionList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageTransactionList');
    }

    saveLocalSearchTerm(pageSize: number, category: string, paymentMethod: string, startDate: Date, endDate: Date): void {
        if (pageSize) localStorage.setItem('pageSizeTransactionList', pageSize.toString());
        if (startDate) localStorage.setItem('startDateFilterTransactionList', startDate.toString());
        if (endDate) localStorage.setItem('endDateFilterTransactionList', endDate.toString());
        if (category) localStorage.setItem('categoryFilterTransactionList', category);
        if (paymentMethod) localStorage.setItem('paymentMethodFilterTransactionList', paymentMethod);
    }

    getLocalSearchTerm(): { pageSize?: number, category?: string, paymentMethod?: string, startDate?: Date, endDate?: Date } {
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

    clearLocalSearchTerm(): void {
        localStorage.removeItem('pageSizeTransactionList');
        localStorage.removeItem('startDateFilterTransactionList');
        localStorage.removeItem('endDateFilterTransactionList');
        localStorage.removeItem('categoryFilterTransactionList');
        localStorage.removeItem('paymentMethodFilterTransactionList');
    }
}
