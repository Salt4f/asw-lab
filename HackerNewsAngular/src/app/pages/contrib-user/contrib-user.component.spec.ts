import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContribUserComponent } from './contrib-user.component';

describe('ContribUserComponent', () => {
  let component: ContribUserComponent;
  let fixture: ComponentFixture<ContribUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContribUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContribUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
