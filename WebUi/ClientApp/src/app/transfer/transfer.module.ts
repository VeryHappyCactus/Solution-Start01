import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap'

import { AuthorizeGuard } from '../../api-authorization/authorize.guard';
import { TransferMainComponent } from './transfer-main/transfer-main.component';
import { TransferSheetComponent } from './transfer-sheet/transfer-sheet.component';
import { EditComponent } from './edit/edit.component';
import { TransferMenu1Component } from './transfer-menu1/transfer-menu1.component';
import { TransferMenu2Component } from './transfer-menu2/transfer-menu2.component';

const transferChildrenRoutes: Routes = [
  { path: '', component: TransferSheetComponent, pathMatch: 'full' },
  { path: 'transferMenu1', component: TransferMenu1Component },
  { path: 'transferMenu2', component: TransferMenu2Component }
];


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    NgbDatepickerModule,
    RouterModule.forRoot(
      [
        { path: '', component: TransferMainComponent, canActivate: [AuthorizeGuard], children: transferChildrenRoutes }
      ]
    )
  ],
  declarations: [TransferMainComponent, TransferSheetComponent, EditComponent, TransferMenu1Component, TransferMenu2Component],
  exports: [TransferMainComponent, TransferSheetComponent]
})
export class TransferModule { }
