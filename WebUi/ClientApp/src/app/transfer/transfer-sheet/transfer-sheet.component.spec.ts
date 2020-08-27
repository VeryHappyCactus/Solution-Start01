import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferSheetComponent } from './transfer-sheet.component';

describe('TransferSheetComponent', () => {
  let component: TransferSheetComponent;
  let fixture: ComponentFixture<TransferSheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferSheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
