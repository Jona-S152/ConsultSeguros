import { Component, inject, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { InsuranceService } from '../../services/insurance.service';
import { Insurance, ResponseJSON } from '../../interfaces/insurance';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { MatPaginator } from '@angular/material/paginator';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-list-insurance-page',
  templateUrl: './list-insurance-page.component.html',
  styles: ``
})
export class ListInsurancePageComponent {
  private insuranceService = inject(InsuranceService);
  private route = inject(Router);
  
  public response? : ResponseJSON;
  public insurances = this.insuranceService.$myInsuranceList

  public selectedElement : Insurance | null = null;
  public initialValue : string = '';

  public hasLoaded : boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public insuranceEditForm = new FormGroup({
    id : new FormControl<number | null>(0),
    insuranceName : new FormControl<string>('', [Validators.required]),
    insuranceCode : new FormControl<string>('', [Validators.required]),
    insuranceAmount : new FormControl<number | null>(0, [Validators.required]),
    prima : new FormControl<number | null>(0, [Validators.required])
  })

  public get currentInsuranceEditForm() : Insurance {
    return this.insuranceEditForm.value as Insurance;
  }
  
  public displayedColumns: string[] = ['Nombre', 'Código', 'Suma asegurada', 'Prima', 'Acciones'];
  public dataSource = new MatTableDataSource<Insurance>(this.insuranceService.myInsuranceLst);
  
  ngOnInit(): void {
    this.insuranceService.getAllInsurances()
      .subscribe(
        {
          next: (res) => {
            this.response = res;
            this.insuranceService.addList(res.data);
            this.hasLoaded = true;
            this.insuranceService.setCopyInsuranceList();
          }
        }
      )
  }  

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ChangeEditSave( element : Insurance ) {
    

    if ( this.selectedElement === element) {
      // Guardar cambios
      Swal.fire({
        title: "Estás seguro/a de actualizar este registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
      }).then((result) => { 
        if (result.isConfirmed) {
          this.insuranceService.updateInsurance(this.currentInsuranceEditForm)
          .subscribe(
            {
              next: (res) => {
                this.insuranceService.updateInsuranceToList(this.currentInsuranceEditForm);
                Swal.fire({
                  icon: 'success',
                  text: res.message
                })
                this.selectedElement = null;
              },
              error: (err) => {
                Swal.fire({
                  icon: 'error',
                  text: err.message
                })
              }
            }
          )
        }
      });
      
    } else {
      this.insuranceEditForm.reset(element);
      this.selectedElement = element;
    }
  }

  deleteInsurance( element : Insurance ) {
    if ( this.selectedElement === element) {
      this.selectedElement = null;
    } else {
      Swal.fire({
        title: "Estás seguro/a de eliminar este registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si",
        cancelButtonText: "No"
      }).then((result) => {
        if (result.isConfirmed) {
          this.insuranceService.deleteInsurance(element.id)
          .subscribe(
            {
              next: (res) => {
                if (res.error){
                  Swal.fire({
                    icon: 'error',
                    text: res.message
                  })
                } else {
                  this.insuranceService.deleteInsuranceToList(element.id);
                  Swal.fire({
                    icon: 'success',
                    text: res.message
                  })
                }
              }
            }
          )
        }
      });
      this.selectedElement = element;
    }

    
  }

  searchByCode(code: string){
    this.insuranceService.searchInsuranceByCodeLst(code);
  }
}
