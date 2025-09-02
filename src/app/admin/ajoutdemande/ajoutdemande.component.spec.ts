import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjoutdemandeComponent } from './ajoutdemande.component';

describe('AjoutdemandeComponent', () => {
  let component: AjoutdemandeComponent;
  let fixture: ComponentFixture<AjoutdemandeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AjoutdemandeComponent]
    });
    fixture = TestBed.createComponent(AjoutdemandeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
