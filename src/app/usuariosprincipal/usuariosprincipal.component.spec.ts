import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuariosprincipalComponent } from './usuariosprincipal.component';

describe('UsuariosprincipalComponent', () => {
  let component: UsuariosprincipalComponent;
  let fixture: ComponentFixture<UsuariosprincipalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UsuariosprincipalComponent]
    });
    fixture = TestBed.createComponent(UsuariosprincipalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
