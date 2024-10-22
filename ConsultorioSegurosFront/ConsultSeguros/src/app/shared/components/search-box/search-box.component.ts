import { Component, EventEmitter, Input, Output } from '@angular/core';
import { debounceTime, Subject, Subscription } from 'rxjs';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styles: ``
})
export class SearchBoxComponent {
  private debouncer: Subject<string> = new Subject<string>();
  private debouncerSubscription? : Subscription;

  // Este input espera la ultima busqueda para mostrarse en el input
  @Input()
  public initialValue : string = '';

  // Este input se usa para cuando se reutiliza el componente se pueda editar el
  // placeholder como quiera sin modificar el componente directamente
  @Input()
  public placeholder: string = '';

  // Emite el valor que se ingresa en el input
  @Output()
  public onValue: EventEmitter<string> = new EventEmitter();

  @Output()
  public onDebounce: EventEmitter<string> = new EventEmitter();

  emitSearch( value: string ): void {
    this.onValue.emit( value );
  }

  onKeyPress( searchTerm : string ) : void {
    this.debouncer.next( searchTerm );
  }

  ngOnInit(): void {
    this.debouncer
    .pipe(
      debounceTime( 300 )
    )
    .subscribe( value => {
      this.onDebounce.emit( value );
    })
  }

  ngOnDestroy(): void {
    this.debouncerSubscription?.unsubscribe();
  }
}
