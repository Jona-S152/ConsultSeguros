import { Component, inject, OnInit } from '@angular/core';
import { Insured, ResponseJSON } from '../../interfaces/insured';
import { MatTableDataSource } from '@angular/material/table';
import { InsuredService } from '../../services/insured.service';
import { delay } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-insured-page',
  templateUrl: './list-insured-page.component.html',
  styles: ``
})
export class ListInsuredPageComponent implements OnInit{
  
  private insuredService = inject(InsuredService);
  private route = inject(Router);
  
  public response? : ResponseJSON;

  public hasLoaded : boolean = false;
  
  public displayedColumns: string[] = ['Identificación', 'Nombre', 'Teléfono', 'Edad', 'Acciones'];
  public dataSource = new MatTableDataSource<Insured>();
  
  ngOnInit(): void {
    this.insuredService.getAllInsureds()
      .subscribe(
        {
          next: (res) => {
            this.response = res;
            console.log(res);
            this.hasLoaded = true;
          }
        }
      )
  }  

  getDetails( id : number ) {
    this.route.navigateByUrl(`consultorio-seguros/asegurado/${id}`);
  }
}
