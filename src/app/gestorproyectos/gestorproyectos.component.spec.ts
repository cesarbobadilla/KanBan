import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestorproyectosComponent } from './gestorproyectos.component';

describe('GestorproyectosComponent', () => {
  let component: GestorproyectosComponent;
  let fixture: ComponentFixture<GestorproyectosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GestorproyectosComponent]
    });
    fixture = TestBed.createComponent(GestorproyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
