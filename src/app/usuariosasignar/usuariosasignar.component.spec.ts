import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuariosasignarComponent } from './usuariosasignar.component';

describe('UsuariosasignarComponent', () => {
  let component: UsuariosasignarComponent;
  let fixture: ComponentFixture<UsuariosasignarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UsuariosasignarComponent]
    });
    fixture = TestBed.createComponent(UsuariosasignarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
