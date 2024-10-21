import { inject, Injectable } from '@angular/core';
import { environments } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ResponseJSON } from '../interfaces/insurance';


@Injectable({
  providedIn: 'root'
})
export class InsuranceService {

  private baseUrl : string = environments.baseUrl;
  
  private http = inject(HttpClient);

  getAllInsurances() : Observable<ResponseJSON> {
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insurance/GetAllInsurances`)
      .pipe(
        catchError( err => throwError( () => err.error ) )
      )
  }

  deleteInsurance( id : number ) {
    return this.http.delete<ResponseJSON>(`${this.baseUrl}/api/Insurance/Delete/${id}`)
      .pipe(
        catchError( err => throwError( () => err.error ) )
      )
  }
}
