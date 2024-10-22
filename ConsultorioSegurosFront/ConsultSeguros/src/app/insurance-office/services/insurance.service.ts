import { inject, Injectable } from '@angular/core';
import { environments } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { Insurance, ResponseJSON } from '../interfaces/insurance';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';


@Injectable({
  providedIn: 'root'
})
export class InsuranceService {

  private baseUrl : string = environments.baseUrl;
  
  private http = inject(HttpClient);

  private insuranceList : Insurance[] = [];

  private myInsuranceList = new BehaviorSubject<Insurance[]>([]);
  $myInsuranceList = this.myInsuranceList.asObservable();

  private originalInsuranceList : Insurance[] = [];

  getAllInsurances() : Observable<ResponseJSON> {
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insurance/GetAllInsurances`)
      .pipe(
        catchError( err => throwError( () => err.error ) )
      )
  }

  setCopyInsuranceList(){
    this.originalInsuranceList = [...this.insuranceList]
  }

  deleteInsurance( id : number ) : Observable<ResponseJSON> {
    return this.http.delete<ResponseJSON>(`${this.baseUrl}/api/Insurance/Delete/${id}`)
      .pipe(
        catchError( err => throwError( () => err.error ) )
      )
  }

  addInsuranceToList( insurance : Insurance ){
    this.insuranceList.push(insurance);
    this.myInsuranceList.next(this.insuranceList);
  }

  addList( insurances : Insurance[] ){
    this.insuranceList = insurances;
    this.myInsuranceList.next(this.insuranceList);
  }

  
  public get myInsuranceLst() : Insurance[] {
    return this.insuranceList;
  }
  
  deleteInsuranceToList( id : number ){
    this.insuranceList = this.insuranceList.filter( (i) => {
      return i.id !== id
    });
    this.myInsuranceList.next(this.insuranceList);
  }

  updateInsuranceToList( insurance : Insurance ){
    this.insuranceList[this.insuranceList.findIndex(i => i.id === insurance.id)] = insurance;
    this.myInsuranceList.next(this.insuranceList);
  }

  addInsurance( insurance : Insurance ) : Observable<ResponseJSON> {
    return this.http.post<ResponseJSON>(`${this.baseUrl}/api/Insurance/Add`, insurance )
      .pipe(
        catchError( err => throwError( () => err.error ) ),
      )
  }

  updateInsurance( insurance : Insurance ) : Observable<ResponseJSON>{
    return this.http.put<ResponseJSON>(`${this.baseUrl}/api/Insurance/Update/${insurance.id}`, insurance)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  searchInsuranceByCodeLst( code : string ){
    
    if (code === '') {
      this.insuranceList = [...this.originalInsuranceList];
      this.myInsuranceList.next(this.insuranceList);
    }
    else {
      this.insuranceList = this.insuranceList.filter( i => i.insuranceCode.toUpperCase().includes(code.toUpperCase()))
      this.myInsuranceList.next(this.insuranceList);
    }
  }

  searchInsuranceByCode( code : string ) : Observable<ResponseJSON> {
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insurance/GetByCode/${code}`)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }
}
