import { Component, inject, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { InsuredService } from '../../services/insured.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Insured, InsuredGet } from '../../interfaces/insured';
import { InsuranceService } from '../../services/insurance.service';
import { Insurance } from '../../interfaces/insurance';

@Component({
  selector: 'app-new-insured',
  templateUrl: './new-insured.component.html',
  styles: ``
})
export class NewInsuredComponent implements OnInit {
  private insuredService = inject(InsuredService);
  private insuranceService = inject(InsuranceService)
  
  public insurances : Insurance[] = []
  
  public insuredForm = new FormGroup({
    id : new FormControl<number | null>(0),
    identification : new FormControl<string>('', [Validators.required]),
    insuredName : new FormControl<string>('', [Validators.required]),
    phoneNumber : new FormControl<string | null>('', [Validators.required]),
    age : new FormControl<number | null>(0, [Validators.required]),
    insurances : new FormControl<string[] | null>(null, [Validators.required])
  })
  
  public get currentInsuredForm() : InsuredGet {
    return this.insuredForm.value as InsuredGet;
  }

  
  public getCurrentInsuredInsertForm( insurances : string ) : Insured {
    const currentInsuredForm : Insured = {
      id : this.currentInsuredForm.id,
      identification : this.currentInsuredForm.identification,
      insuredName : this.currentInsuredForm.insuredName,
      phoneNumber: this.currentInsuredForm.phoneNumber,
      age: this.currentInsuredForm.age,
      insurancesIds: insurances
    }

    return currentInsuredForm;
  }
  
  
  ngOnInit(): void {
    this.insuranceService.getAllInsurances()
      .subscribe({
        next: (res) => {
          this.insurances = res.data
        }
      })
  }

  uploadFile( event : any ){
    const file = event.target.files[0];

    this.insuredService.uploadFile(file)
      .subscribe({
        next: (res) => {
          Swal.fire({
            icon: 'success',
            text: res.message
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            text: err.message
          });
        }
      })
  }

  addInsurance(){

    if (this.insuredForm.invalid) return;

    const selectedValues : string[] = this.insuredForm.get('insurances')?.value ?? [];
    const selectedValuesConcat : string = selectedValues.join();

    const currentInsured = this.getCurrentInsuredInsertForm(selectedValuesConcat);

    console.log(currentInsured);

    this.insuredService.addInsured(currentInsured)
      .subscribe({
        next: (res) => {
          this.insuredService.addInsuredToList(this.currentInsuredForm);

          Swal.fire({
            icon: 'success',
            text: res.message
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            text: err.message
          })
        }
      })
  }

}
