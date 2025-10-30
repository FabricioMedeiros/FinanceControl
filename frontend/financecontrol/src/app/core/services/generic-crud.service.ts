import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from './base.service';

export abstract class GenericCrudService<TModel> extends BaseService {
    protected apiUrl: string;

    constructor(
        protected http: HttpClient,
        protected entityName: string, 
        endpoint: string
    ) {
        super();
        this.apiUrl = `${this.UrlServiceV1}${endpoint}`;
    }

    getAll(pageNumber?: number, pageSize?: number, filters?: Record<string, string>): Observable<any> {
        const query = this.buildQueryParams(filters, pageNumber, pageSize);
        const url = `${this.apiUrl}${query}`;
        return this.http.get<any>(url, this.getAuthHeaderJson())
            .pipe(catchError(err => this.serviceError(err)));
    }

    getById(id: number): Observable<TModel> {
        return this.http.get<TModel>(`${this.apiUrl}/${id}`, this.getAuthHeaderJson())
            .pipe(catchError(err => this.serviceError(err)));
    }

   create(model: any): Observable<TModel> {
    const sanitizedModel = Object.fromEntries(
        Object.entries(model).filter(([key, value]) => key !== 'id' || (value && value !== ''))
    );   

    return this.http.post<TModel>(this.apiUrl, sanitizedModel, this.getAuthHeaderJson())
        .pipe(
            map(res => this.extractData(res)),
            catchError(err => this.serviceError(err))
        );
}

    update(id: number, model: any): Observable<TModel | void> {
        console.log('url: '+ this.apiUrl);
        console.log('obj: '+ JSON.stringify(model));
        return this.http.put<TModel>(`${this.apiUrl}/${id}`, model, this.getAuthHeaderJson())
            .pipe(
                map(res => this.extractData(res)),
                catchError(err => this.serviceError(err))
            );
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiUrl}/${id}`, this.getAuthHeaderJson())
            .pipe(catchError(err => this.serviceError(err)));
    }

    save(model: any): Observable<TModel | void> {
        if (model && model.id && model.id != '') return this.update(model.id, model);
        return this.create(model);
    }

    saveLocalCurrentPageList(value: number | string) {
        this.saveLocal(this.entityName, 'currentPage', value.toString());
    }

    getLocalCurrentPageList(): string | null {
        return this.getLocal(this.entityName, 'currentPage');
    }

    clearLocalCurrentPageList() {
        this.clearLocal(this.entityName, 'currentPage');
    }

    saveLocalSearchTerm(value: string) {
        this.saveLocal(this.entityName, 'searchTerm', value);
    }

    getLocalSearchTerm(): string | null {
        return this.getLocal(this.entityName, 'searchTerm');
    }

    clearLocalSearchTerm() {
        this.clearLocal(this.entityName, 'searchTerm');
    }
}
