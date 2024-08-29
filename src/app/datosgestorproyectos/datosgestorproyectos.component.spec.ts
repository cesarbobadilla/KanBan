import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosgestorproyectosComponent } from './datosgestorproyectos.component';

describe('DatosgestorproyectosComponent', () => {
  let component: DatosgestorproyectosComponent;
  let fixture: ComponentFixture<DatosgestorproyectosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DatosgestorproyectosComponent]
    });
    fixture = TestBed.createComponent(DatosgestorproyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
