import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

import { BaseService } from "src/app/core/services/base.service";
import { PaymentMethod } from "src/app/features/payment-method/models/payment-method";


@Injectable()
export class PaymentMethodService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAll(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}paymentmethod`;
    
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

    getPaymentMethodById(id: string): Observable<PaymentMethod> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<PaymentMethod>(`${this.UrlServiceV1}paymentmethod/${id}`,  headers)
            .pipe(catchError(super.serviceError));
    }
   
    registerPaymentMethod(paymentMethod: PaymentMethod): Observable<PaymentMethod> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<PaymentMethod>(`${this.UrlServiceV1}paymentmethod`, paymentMethod, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updatePaymentMethod(paymentMethod: PaymentMethod): Observable<PaymentMethod> {
        const headers = this.GetAuthHeaderJson();
        const httpOptions = {
            headers: headers
        };
    
        return this.http
            .put<PaymentMethod>(`${this.UrlServiceV1}paymentmethod/${paymentMethod.id}`, paymentMethod, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }
    
    delete(id: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}paymentmethod/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPagePaymentMethodList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPagePaymentMethodList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPagePaymentMethodList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermPaymentMethodList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermPaymentMethodList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermPaymentMethodList');
    }
}
