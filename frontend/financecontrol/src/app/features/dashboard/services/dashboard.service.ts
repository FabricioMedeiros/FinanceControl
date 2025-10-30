import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";

import { BaseService } from "src/app/core/services/base.service";

@Injectable()
export class DashboardService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getDashboardData(year: number, month?: number): Observable<any> {
        const headers = this.getAuthHeaderJson();

        let url = `${this.UrlServiceV1}dashboard?year=${year}`;
        if (month) {
            url += `&month=${month}`;
        }

        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }
}
