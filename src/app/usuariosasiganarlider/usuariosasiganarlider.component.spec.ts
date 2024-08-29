import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuariosasiganarliderComponent } from './usuariosasiganarlider.component';

describe('UsuariosasiganarliderComponent', () => {
  let component: UsuariosasiganarliderComponent;
  let fixture: ComponentFixture<UsuariosasiganarliderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UsuariosasiganarliderComponent]
    });
    fixture = TestBed.createComponent(UsuariosasiganarliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
