import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ResponseJSON, Insured } from '../../interfaces/insured';
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

  public selectedInsurances: number[] = [];

  public selectedElement : Insured | null = null;
  public initialValue : string = '';

  public hasLoaded : boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;


  public insuredEditForm = new FormGroup({
    id : new FormControl<number | null>(0),
    identification : new FormControl<string>('', [Validators.required]),
    insuredName : new FormControl<string>('', [Validators.required]),
    phoneNumber : new FormControl<string | null>('', [Validators.required]),
    age : new FormControl<number | null>(0, [Validators.required]),
    insurances : new FormControl<string[] | null>(null, [Validators.required])
  })

  public get currentInsuredEditForm() : Insured {
    return this.insuredEditForm.value as Insured;
  }

  public getCurrentInsuredUpdateForm( insurances : string ) : Insured {
    const currentInsuredForm : Insured = {
      id : this.currentInsuredEditForm.id,
      identification : this.currentInsuredEditForm.identification,
      insuredName : this.currentInsuredEditForm.insuredName,
      phoneNumber: this.currentInsuredEditForm.phoneNumber,
      age: this.currentInsuredEditForm.age,
      insurancesIds: insurances
    }

    return currentInsuredForm;
  }
  
  public displayedColumns: string[] = ['Identificación', 'Nombre', 'N° de teléfono', 'Edad', 'Seguros', 'Acciones'];
  public dataSource = new MatTableDataSource<Insured>(this.insuredService.myInsuredLst);
  
  ngOnInit(): void {
    this.insuredService.getAllInsureds()
      .subscribe(
        {
          next: (res) => {
            this.response = res;
            res.data.forEach( (j) => {
              this.insuranceService.getAllInsurancesByInsured(j.identification)
              .subscribe({
                next: (resp) => {
                  const insuredsCode : string[] = []
                    resp.data.forEach( (k) => {
                      insuredsCode.push(k.insuranceCode)
                    })
                    const insured : Insured = {
                      id: j.id,
                      identification: j.identification,
                      insuredName: j.insuredName,
                      phoneNumber: j.phoneNumber,
                      age: j.age,
                      insurancesIds: insuredsCode
                    }
                    
                    this.insuredService.addInsuredToList(insured);
                  },
                  error: () => {
                    const insured : Insured = {
                      id: j.id,
                      identification: j.identification,
                      insuredName: j.insuredName,
                      phoneNumber: j.phoneNumber,
                      age: j.age,
                      insurancesIds: []
                    }
        
                    this.insuredService.addInsuredToList(insured);
                  },
                })
            })
            this.hasLoaded = true;
            
          }
        }
      )

    this.insureds.subscribe({
      next: () => {
        this.insuredService.setCopyInsuredList();
      }
    });
      
    this.insuranceService.getAllInsurances()
      .subscribe({
        next: (resp) => {
          this.insuranceService.addList(resp.data)
          this.insurances = this.insuranceService.myInsuranceLst;
        }
      })
  }  

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ChangeEditSave( element : Insured ) {

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

          const selectedValues : string[] = this.insuredEditForm.get('insurances')?.value ?? [];
          const selectedValuesConcat : string = selectedValues.join();

          const currentInsured = this.getCurrentInsuredUpdateForm(selectedValuesConcat);

          this.insuredService.updateInsured(currentInsured)
          .subscribe(
            {
              next: (res) => {

                let selectedTexts = selectedValues.map(value => {
                  const insurance = this.insurances.find(ins => ins.id === Number(value));
                  return insurance ? insurance.insuranceCode : '';
                });
      
                selectedTexts = selectedTexts.filter(t => t !== '');
      
                currentInsured.insurancesIds = selectedTexts.join();

                this.insuredService.updateInsuredToList(currentInsured);
                
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
      
        const selectedInsurances = this.insurances.filter( i => element.insurancesIds.includes(i.insuranceCode));
        let selectedValues = selectedInsurances.map(value => {
          const insurance = this.insurances.find(ins => ins.insuranceCode === value.insuranceCode);
          return insurance ? insurance.id : 0;
        })

        this.selectedInsurances = selectedValues;
        this.insuredEditForm.get('insurances')?.setValue(selectedValues.map(s => s.toString()));

        this.insuredEditForm.reset(element);
        this.selectedElement = element;
      }
  }

  deleteInsured( element : Insured ) {
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

  searchByIdentification( identification: string ){
    this.insuredService.searchInsuredByIdentificationLst(identification);
  }
}
