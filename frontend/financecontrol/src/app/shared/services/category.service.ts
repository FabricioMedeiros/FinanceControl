import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

import { BaseService } from "src/app/core/services/base.service";
import { Category } from "src/app/features/category/models/category";

@Injectable()
export class CategoryService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAll(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}category`;
    
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

    getSpecialtyById(id: number): Observable<Category> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Category>(`${this.UrlServiceV1}category/${id}`,  headers)
            .pipe(catchError(super.serviceError));
    }

    registerSpecialty(category: Category): Observable<Category> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Category>(`${this.UrlServiceV1}category`, category, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateSpecialty(category: Category): Observable<Category> {
        const headers = this.GetAuthHeaderJson();
        const httpOptions = {
            headers: headers
        };
    
        return this.http
            .put<Category>(`${this.UrlServiceV1}category/${category.id}`, category, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }
    
    delete(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}category/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageCategoryList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageCategoryList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageCategoryList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermCategoryList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermCategoryList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermCategoryList');
    }
}
