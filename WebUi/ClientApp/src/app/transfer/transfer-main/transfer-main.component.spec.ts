import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferMainComponent } from './transfer-main.component';

describe('TransferMainComponent', () => {
  let component: TransferMainComponent;
  let fixture: ComponentFixture<TransferMainComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferMainComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
