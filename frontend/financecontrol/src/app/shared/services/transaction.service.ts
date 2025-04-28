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

    getTransactionById(id: string): Observable<Transaction> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Transaction>(`${this.UrlServiceV1}transaction/${id}`,  headers)
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

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermTransactionList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermTransactionList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermTransactionList');
    }
}
