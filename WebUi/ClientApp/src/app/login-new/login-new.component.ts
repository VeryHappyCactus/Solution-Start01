import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Router, ActivatedRoute } from "@angular/router"
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ApplicationPaths } from '../../api-authorization/api-authorization.constants';

@Component({
  selector: 'app-login-new',
  templateUrl: './login-new.component.html',
  styleUrls: [
    './login-new.component.css',
   
  ],
  //encapsulation: ViewEncapsulation.None,
})
export class LoginNewComponent implements OnInit {

  public loginForm: FormGroup;
  public isLoginInvalid: boolean;
  private user: User;

  constructor(private httpClient: HttpClient,
    private router: Router,
    private activatedRoute: ActivatedRoute) {

    this.isLoginInvalid = false;
  }

  public get name() { return this.loginForm.get('name'); }

  public get password() { return this.loginForm.get('password'); }

  ngOnInit() {

    this.loginForm = new FormGroup({
      name: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    });
  }


  public onSubmit() {

    if (this.loginForm.valid) {

      this.user = new User();
      this.user.name = this.loginForm.get("name").value;
      this.user.password = this.loginForm.get("password").value;
      //this.user.name = "admin@admin.net";
      //this.user.password = "Z2030r###";

      this.httpClient.post<ResponseData>("/api/Authentication/Login", this.user)
      .pipe(catchError(this.handleError))
      .subscribe(resp => {

        if (resp.status == ResponseStatusTypes.Success) {

          let returnUrl = (this.activatedRoute.snapshot.queryParams).returnUrl;

          if (returnUrl == "/")
            returnUrl = undefined;

          this.router.navigate(['/authentication/login'], { queryParams: { returnUrl: returnUrl } });
        }
        else {
          this.isLoginInvalid = true;
        }
      });
    }

  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    return throwError(
      'Something bad happened; please try again later.');
  }

}

enum ResponseStatusTypes {
  Success,
  Error
}

class ResponseData {

  public errorMessage: string;
  public status: ResponseStatusTypes;
}

class User {
  public name: string;
  public password: string;

}
