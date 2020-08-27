import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferMenu2Component } from './transfer-menu2.component';

describe('TransferMenu2Component', () => {
  let component: TransferMenu2Component;
  let fixture: ComponentFixture<TransferMenu2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferMenu2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferMenu2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
