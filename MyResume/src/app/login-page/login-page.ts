import { Component, Renderer2, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { RequestModel } from '../models/request';
import { ResponseModel } from '../models/response';

import { environment, AppConfigService } from '../../environment';
import { Router } from '@angular/router';
import { config } from 'rxjs';


@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage implements OnInit {
  @ViewChild('userName') userNameRef!: ElementRef;
  @ViewChild('emailId') emailIdRef!: ElementRef;

  logoCLass = 'logo-original-start';
  nameClass = 'name-original-start';
  loginModule = 'login-module-start';
  buttonClass = 'login-button-high';
  spinnerClass = 'spinner-low';
  isFade = true;
  isFadeModule = true;
  isButtonFade = false;
  isSpinnerFade = true;

  constructor(private renderer: Renderer2, private http: HttpClient, private router: Router, private config: AppConfigService) { }

  loginCall(payload: RequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ResponseModel>(environment.loginFunctionUrl, payload, {
      headers,
    }
    );
  }

  ngOnInit() {
    setTimeout(() => {
      this.nameClass = 'name-original-end';
      this.isFade = false;
      this.logoCLass = 'logo-original-end';
    }, 1000);

    setTimeout(() => {
      this.nameClass = 'name-original-last';
      this.logoCLass = 'logo-original-last';
    }, 1500);
    setTimeout(() => {
      this.isFadeModule = false;
      this.loginModule = 'login-module-end';
    }, 2000);
  }

  login() {
    setTimeout(() => {
      this.isButtonFade = true;
      this.buttonClass = 'login-button-low';

      setTimeout(() => {
        this.isSpinnerFade = false;
        this.spinnerClass = 'spinner-high';

        var userNameEl: HTMLInputElement;
        var emailIdEl: HTMLInputElement;

        userNameEl = this.userNameRef.nativeElement;
        emailIdEl = this.emailIdRef.nativeElement;

        var request: RequestModel = {
          emailId: emailIdEl.value,
          name: userNameEl.value,
          loginId: 0
        };
        this.loginCall(request)
          .subscribe({
            next: res => {
              if (res.isSuccessful && res.returnCode > 0) {
                setTimeout(() => {
                  this.isSpinnerFade = true;
                  this.spinnerClass = 'spinner-low';
                  this.config.globalUsername = res.name;
                  this.config.globalEmail = res.emailId;
                  this.config.globalLoginId = res.loginId;

                  this.router.navigate(['/chat']);
                }, 200);
              }
            },
            error: err => {
              console.log(err);
            },
          });
      }, 200);
    }, 200);
  }

}
