import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { environments } from '../../../environments/environments';
import { InsuredGet, InsuredDTO, ResponseJSON, Insured } from '../interfaces/insured';

@Injectable({
  providedIn: 'root'
})
export class InsuredService {

  private baseUrl : string = environments.baseUrl;
  
  private http = inject(HttpClient);

  private insuredList : Insured[] = [];

  private myInsuredList = new BehaviorSubject<Insured[]>([]);
  $myInsuredList = this.myInsuredList.asObservable();

  private originalInsuredList : Insured[] = [];

  public get myInsuredLst() : Insured[] {
    return this.insuredList;
  }
  

  getAllInsureds() : Observable<ResponseJSON>{
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insured/GetAll`)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  addList( insureds : Insured[] ){
    this.insuredList = insureds;
    this.myInsuredList.next(this.insuredList);
  }

  setCopyInsuredList(){
    this.originalInsuredList = [...this.myInsuredLst];
  }

  updateInsured( insured : Insured ) : Observable<ResponseJSON>{
    return this.http.put<ResponseJSON>(`${this.baseUrl}/api/Insured/Update/${insured.id}`, insured)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  updateInsuredToList( insured : Insured ){
    this.insuredList[this.insuredList.findIndex(i => i.id === insured.id)] = insured;
    this.myInsuredList.next(this.insuredList);
  }

  deleteInsured( id : number ) : Observable<ResponseJSON> {
    return this.http.delete<ResponseJSON>(`${this.baseUrl}/api/Insured/DeleteInsured/${id}`)
      .pipe(
        catchError( err => throwError( () => err.error ) )
      )
  }

  deleteInsuredToList( id : number ){
    this.insuredList = this.insuredList.filter( (i) => {
      return i.id !== id
    });
    this.myInsuredList.next(this.insuredList);
  }

  searchInsuredByIdentificationLst( identification : string ){
    if ( identification === ''){
      this.setCopyInsuredList();
    }
    else {
      this.insuredList = this.originalInsuredList.filter( i => i.identification.startsWith(identification))
      this.myInsuredList.next(this.insuredList);
    }
  }

  searchInsuredByIdentification( identification : string ) : Observable<ResponseJSON> {
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insured/GetByIdentification/${identification}`)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  addInsured( insured : Insured ) : Observable<ResponseJSON> {
    return this.http.post<ResponseJSON>(`${this.baseUrl}/api/Insured/Add`, insured )
      .pipe(
        catchError( err => throwError( () => err.error ) ),
      )
  }

  addInsuredToList( insured : Insured ){
    this.insuredList.push(insured);
    this.myInsuredList.next(this.insuredList);
  }

  uploadFile( file : File ) : Observable<ResponseJSON> {
    const formData = new FormData();

    formData.append('file', file, file.name);

    return this.http.post<ResponseJSON>(`${this.baseUrl}/api/Insured/UploadFile`, formData)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  assignInsurancesToInsured( InsurancesIds : string ) : Observable<ResponseJSON> {
    console.log({InsurancesIds})
    return this.http.post<ResponseJSON>(`${this.baseUrl}/api/Insured/AssignInsurances`, {InsurancesIds})
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }
}
