import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Insurance } from '../../interfaces/insurance';
import { InsuranceService } from '../../services/insurance.service';
import Swal from 'sweetalert2';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-new-insurance',
  templateUrl: './new-insurance.component.html',
  styles: ``
})
export class NewInsuranceComponent {

  private insuranceService = inject(InsuranceService);

  public insuranceForm = new FormGroup({
    id : new FormControl<number | null>(0),
    insuranceName : new FormControl<string>('', [Validators.required]),
    insuranceCode : new FormControl<string>('', [Validators.required]),
    insuranceAmount : new FormControl<number | null>(0, [Validators.required]),
    prima : new FormControl<number | null>(0, [Validators.required])
  })

  
  public get currentInsuranceForm() : Insurance {
    return this.insuranceForm.value as Insurance;
  }

  addInsurance(){

    if (this.insuranceForm.invalid) return;

    this.insuranceService.addInsurance(this.currentInsuranceForm)
      .subscribe({
        next: (res) => {
          this.insuranceService.addInsuranceToList(this.currentInsuranceForm);
            Swal.fire({
              icon: 'success',
              text: res.message
            });
            this.insuranceForm.reset();
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
