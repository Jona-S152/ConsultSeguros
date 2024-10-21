import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environments } from '../../../environments/environments';
import { ResponseJSON } from '../interfaces/insured';

@Injectable({
  providedIn: 'root'
})
export class InsuredService {

  private baseUrl : string = environments.baseUrl;
  
  private http = inject(HttpClient);

  getAllInsureds() : Observable<ResponseJSON>{
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insured/GetAll`)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }
}
