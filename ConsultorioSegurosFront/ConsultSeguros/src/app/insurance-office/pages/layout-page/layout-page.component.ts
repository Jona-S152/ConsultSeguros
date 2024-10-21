import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout-page',
  templateUrl: './layout-page.component.html',
  styles: ``
})
export class LayoutPageComponent {
  public sideBarItems = [
    { label: 'Asegurados', icon: 'face_6', url: '/consultorio-seguros/asegurados/'},
    { label: 'Añadir asegurado', icon: 'add', url: '/consultorio-seguros/asegurados/'},
    { label: 'Seguros', icon: 'shield_with_heart', url: '/consultorio-seguros/seguros/'},
    { label: 'Añadir seguro', icon: 'add', url: '/consultorio-seguros/seguros/'},
  ]

  public userName? : string;
  public isAuthorize : boolean = true;
}
