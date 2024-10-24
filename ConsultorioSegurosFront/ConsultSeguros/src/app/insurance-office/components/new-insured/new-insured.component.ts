import { Component, inject, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { InsuredService } from '../../services/insured.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Insured } from '../../interfaces/insured';
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
    age : new FormControl<number | null>(0, [Validators.required])
  })
  
  public get currentInsuredForm() : Insured {
    return this.insuredForm.value as Insured;
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

    this.insuredService.addInsured(this.currentInsuredForm)
      .subscribe({
        next: (res) => {
          this.insuredService.addInsuredToList(this.currentInsuredForm);
            Swal.fire({
              icon: 'success',
              text: res.message
            });
            this.insuredForm.reset();
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
