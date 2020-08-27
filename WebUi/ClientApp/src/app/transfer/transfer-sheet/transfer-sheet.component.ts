import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { KeyValue } from '../Models';
import { AuthorizeService, IUser } from '../../../api-authorization/authorize.service';

@Component({
  selector: 'app-transfer-sheet',
  templateUrl: './transfer-sheet.component.html',
  styleUrls: [
    './transfer-sheet.component.css',
    '../../../assets/css/style.css',
    '../../../assets/css/custom.css'
  ],

})
export class TransferSheetComponent implements OnInit {

  public transferSheetForm: FormGroup;
  public developers: Array<KeyValue>;
  public companies: Array<KeyValue>;
  public agents: Array<KeyValue>;
  public transferDate: Date;
  public user: IUser;


  constructor(private authorizeService: AuthorizeService) {
  }


  ngOnInit() {

    this.transferDate = new Date();

    this.developers = [new KeyValue(null, "developer 1"), new KeyValue(null, "developer 2"), new KeyValue(null, "developer 3")];
    this.companies = [new KeyValue(null, "company 1"), new KeyValue(null, "company 2")];
    this.agents = [new KeyValue(null, "agent 1"), new KeyValue(null, "agent 2")];

    this.transferSheetForm = new FormGroup({
      isDeedOwnerTimeShare: new FormControl(""),
      isOnlyOneDeed: new FormControl(""),
      isMakeMorgatePayment: new FormControl(""),  
      isCurrentPayments: new FormControl(""),
      isAcceptCase: new FormControl("")
    });

    this.authorizeService.getUser().subscribe((u) => this.user = u);

  }

  public get isOnlyOneDeed() { return this.transferSheetForm.get('isOnlyOneDeed'); }

 }
