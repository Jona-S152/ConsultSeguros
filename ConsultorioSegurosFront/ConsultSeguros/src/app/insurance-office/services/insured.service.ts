import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { environments } from '../../../environments/environments';
import { Insured, InsuredDTO, ResponseJSON } from '../interfaces/insured';

@Injectable({
  providedIn: 'root'
})
export class InsuredService {

  private baseUrl : string = environments.baseUrl;
  
  private http = inject(HttpClient);

  private insuredList : Insured[] = [];

  private myInsuredList = new BehaviorSubject<Insured[]>([]);
  $myInsuredList = this.myInsuredList.asObservable();

  private insuredDTOList : InsuredDTO[] = [];

  private myInsuredDTOList = new BehaviorSubject<InsuredDTO[]>([]);
  $myInsuredDTOList = this.myInsuredDTOList.asObservable();

  private originalInsuredList : Insured[] = [];

  public get myInsuredLst() : Insured[] {
    return this.insuredList;
  }

  
  public get myInsuredDTOLst() : InsuredDTO[] {
    return this.insuredDTOList;
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

  addDTOList( insureds : InsuredDTO ){
    this.insuredDTOList.push(insureds);
    this.myInsuredDTOList.next(this.insuredDTOList);
  }

  setCopyInsuredList(){
    this.originalInsuredList = [...this.insuredList]
  }

  updateInsured( insured : Insured ) : Observable<ResponseJSON>{
    return this.http.put<ResponseJSON>(`${this.baseUrl}/api/Insured/Update/${insured.id}`, insured)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  updateInsuredToList( insured : Insured ){
    console.log(insured)
    console.log(this.insuredList[this.insuredList.findIndex(i => i.id === insured.id)])
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

  searchInsuranceByCodeLst( identification : string ){
    this.insuredList = this.originalInsuredList.filter( i => i.identification.toUpperCase().startsWith(identification.toUpperCase()))
    this.myInsuredList.next(this.insuredList);
  }

  searchInsuranceByCode( identification : string ) : Observable<ResponseJSON> {
    return this.http.get<ResponseJSON>(`${this.baseUrl}/api/Insured/GetByIdentification/${identification}`)
      .pipe(
        catchError( err => throwError( () => err.error ))
      )
  }

  addInsured( insurance : Insured ) : Observable<ResponseJSON> {
    return this.http.post<ResponseJSON>(`${this.baseUrl}/api/Insured/Add`, insurance )
      .pipe(
        catchError( err => throwError( () => err.error ) ),
      )
  }

  addInsuredToList( insurance : Insured ){
    this.insuredList.push(insurance);
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
}
