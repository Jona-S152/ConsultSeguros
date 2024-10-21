import { Component, inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { InsuranceService } from '../../services/insurance.service';
import { Insurance, ResponseJSON } from '../../interfaces/insurance';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-list-insurance-page',
  templateUrl: './list-insurance-page.component.html',
  styles: ``
})
export class ListInsurancePageComponent {
  private insuranceService = inject(InsuranceService);
  private route = inject(Router);
  
  public response? : ResponseJSON;
  public insurances : Insurance[] = []

  public selectedElement : Insurance | null = null;

  public hasLoaded : boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  public displayedColumns: string[] = ['Nombre', 'Código', 'Suma asegurada', 'Prima', 'Acciones'];
  public dataSource = new MatTableDataSource<Insurance>(this.insurances);
  
  ngOnInit(): void {
    this.insuranceService.getAllInsurances()
      .subscribe(
        {
          next: (res) => {
            this.response = res;
            this.insurances = res.data;
            this.hasLoaded = true;
            
          }
        }
      )
  }  

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ChangeEditSave( element : Insurance ) {

    if ( this.selectedElement === element) {
      // Guardar
      this.selectedElement = null;
    } else {
      this.selectedElement = element;
    }
  }

  deleteInsurance( id : number ) {
    this.insuranceService.deleteInsurance(id)
      .subscribe(
        {
          next: (res) => {
            if (res.error){
              Swal.fire({
                icon: 'error',
                text: res.message
              })
            } else {
              this.insurances = this.insurances.filter( item => item.id !== id );
              Swal.fire({
                icon: 'success',
                text: res.message
              })
            }
          }
        }
      )
  }

}
