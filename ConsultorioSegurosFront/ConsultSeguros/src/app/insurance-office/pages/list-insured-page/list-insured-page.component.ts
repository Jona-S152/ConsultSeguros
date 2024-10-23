import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { Insured, InsuredDTO, ResponseJSON } from '../../interfaces/insured';
import { MatTableDataSource } from '@angular/material/table';
import { InsuredService } from '../../services/insured.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import Swal from 'sweetalert2';
import { InsuranceService } from '../../services/insurance.service';
import { Insurance } from '../../interfaces/insurance';

@Component({
  selector: 'app-list-insured-page',
  templateUrl: './list-insured-page.component.html',
  styles: ``
})
export class ListInsuredPageComponent implements OnInit{
  private insuredService = inject(InsuredService);
  private insuranceService = inject(InsuranceService)
  
  public response? : ResponseJSON;
  public insureds = this.insuredService.$myInsuredList
  public insurances = this.insuranceService.myInsuranceLst;
  public insurancesBidimentional : Insurance[][] = []

  public insuredDTO = this.insuredService.$myInsuredDTOList;

  public selectedElement : InsuredDTO | null = null;
  public initialValue : string = '';

  public hasLoaded : boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;


  public insuredDTOEditForm = new FormGroup({
    id : new FormControl<number | null>(0),
    identification : new FormControl<string>('', [Validators.required]),
    insuredName : new FormControl<string>('', [Validators.required]),
    phoneNumber : new FormControl<string | null>('', [Validators.required]),
    age : new FormControl<number | null>(0, [Validators.required]),
    insurances : new FormControl<string[] | null>(null, [Validators.required])
  })

  public get currentInsuredEditForm() : InsuredDTO {
    return this.insuredDTOEditForm.value as InsuredDTO;
  }
  
  public displayedColumns: string[] = ['Identificación', 'Nombre', 'N° de teléfono', 'Edad', 'Seguros', 'Acciones'];
  public dataSource = new MatTableDataSource<InsuredDTO>(this.insuredService.myInsuredDTOLst);
  
  ngOnInit(): void {
    this.insuredService.getAllInsureds()
      .subscribe(
        {
          next: (res) => {
            this.response = res;
            this.insuredService.addList(res.data);
            this.hasLoaded = true;
            this.insuredService.setCopyInsuredList();
          }
        }
      )

    this.insureds.subscribe({
      next: (i) => {
        this.insuranceService.getAllInsurances()
          .subscribe({
            next: (resp) => {
              this.insuranceService.addList(resp.data)
              this.insurances = this.insuranceService.myInsuranceLst;
            }
          })
        i.forEach( (j) => {
          this.insuranceService.getAllInsurancesByInsured(j.identification)
            .pipe(
              
            )
            .subscribe({
              next: (res) => {
                const insuredsCode : string[] = []
                res.data.forEach( (k) => {
                  insuredsCode.push(k.insuranceCode)
                })
                const insured : InsuredDTO = {
                  id: j.id,
                  identification: j.identification,
                  insuredName: j.insuredName,
                  phoneNumber: j.phoneNumber,
                  age: j.age,
                  insurances: insuredsCode
                }
                
                this.insuredService.addDTOList(insured)
              },
              error: (err) => {
                const insured : InsuredDTO = {
                  id: j.id,
                  identification: j.identification,
                  insuredName: j.insuredName,
                  phoneNumber: j.phoneNumber,
                  age: j.age,
                  insurances: []
                }

                this.insuredService.addDTOList(insured)
              },
            })
        })
      }
    })
  }  

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ChangeEditSave( element : InsuredDTO ) {
    
    
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
          this.insuredService.updateInsured(this.currentInsuredEditForm)
          .subscribe(
            {
              next: (res) => {
                this.insuredService.updateInsuredToList(this.currentInsuredEditForm);
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
      this.insuredDTOEditForm.reset(element);
      this.selectedElement = element;
    }
  }

  deleteInsured( element : InsuredDTO ) {
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
          this.insuredService.deleteInsured(element.id)
          .subscribe(
            {
              next: (res) => {
                if (res.error){
                  Swal.fire({
                    icon: 'error',
                    text: res.message
                  })
                } else {
                  this.insuredService.deleteInsuredToList(element.id);
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
    this.insuredService.searchInsuranceByCodeLst(code);
  }
}
