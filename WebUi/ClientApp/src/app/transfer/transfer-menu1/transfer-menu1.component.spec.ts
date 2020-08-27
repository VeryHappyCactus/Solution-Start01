import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferMenu1Component } from './transfer-menu1.component';

describe('TransferMenu1Component', () => {
  let component: TransferMenu1Component;
  let fixture: ComponentFixture<TransferMenu1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferMenu1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferMenu1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
