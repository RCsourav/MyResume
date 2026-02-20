import {
  Component, ElementRef
  , Renderer2, ViewChild
  , AfterViewInit
  , AfterViewChecked
  , OnInit
  , OnDestroy,
  input
} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment, AppConfigService } from '../../environment';
import { Router } from '@angular/router';

import { RequestModel } from '../models/request';
import { ResponseModel } from '../models/response';
import { ChatRequestModel } from '../models/chatRequest';
import { ChatResponseModel } from '../models/chatResponse';

@Component({
  selector: 'app-resume-page',
  standalone: false,
  templateUrl: './resume-page.html',
  styleUrl: './resume-page.css',
})
export class ResumePage implements AfterViewInit, AfterViewChecked, OnInit, OnDestroy {
  nameClass = 'name-original-start';
  logoCLass = 'logo-original-start';
  myNameClass = 'my-name-class';
  headerClass = 'header-container-start';
  isInputFade = true;
  isUserNameFade = true;
  userName = '';
  sessionTime = '';
  timer: any;

  constructor(private renderer: Renderer2, private http: HttpClient
    , private router: Router, private config: AppConfigService) { }

  ngAfterViewChecked() {

  }
  ngAfterViewInit() {
    setTimeout(() => {
      this.myNameClass = 'my-name-class-last';
      this.nameClass = 'name-original-end';
      this.logoCLass = 'logo-original-end';
      this.headerClass = 'header-container-end';
      setTimeout(() => {
        this.isUserNameFade = false;
      }, 1500);
    }, 500);
  }
  ngOnInit() {
    this.startTimer();
    this.userName = this.config.globalUsername;
    //this.userName = 'Sourav Roy Choudhury';
    var request: RequestModel = {
      emailId: this.config.globalEmail,
      name: this.config.globalUsername,
      loginId: this.config.globalLoginId
    };
    this.getActiveSession(request)
      .subscribe({
        next: res => {
          console.log(res);
          if (res.isSuccessful && res.returnCode > 0) {
            this.config.globalSessionTime
          }
          else {
            this.router.navigate(['/login']);
          }
        },
        error: err => {
          this.router.navigate(['/login']);
          console.log(err);
        },
      });

    this.renderer.listen('document', 'click', (event) => this.handle(event));
    this.renderer.listen('document', 'keydown', (event) => this.handle(event));
    this.renderer.listen('document', 'scroll', (event) => this.handle(event));
  }
  ngOnDestroy() {
    this.startTimer();
  }

  startTimer() {
    this.timer = setInterval(() => {
      this.config.globalSessionTime--;
      this.sessionTime = this.convertToMMSS(this.config.globalSessionTime);
    }, 1000);
  }

  stopTimer() {
    clearInterval(this.timer);
  }

  convertToMMSS(totalSeconds: number): string {
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;

    const mm = minutes.toString().padStart(2, '0');
    const ss = seconds.toString().padStart(2, '0');

    return `${mm}:${ss}`;
  }

  openChat() {
    this.myNameClass = 'my-name-class';
    this.nameClass = 'name-original-start';
    this.logoCLass = 'logo-original-start';
    this.headerClass = 'header-container-start';
    this.isUserNameFade = true;

    setTimeout(() => {
      this.router.navigate(['/chat']);
    }, 1000);
  }

  logout() {
    var req: RequestModel = {
      emailId: this.config.globalEmail,
      name: this.config.globalUsername,
      loginId: this.config.globalLoginId
    };
    this.logoutSession(req)
      .subscribe({
        next: res => {
          if (res.isSuccessful && res.returnCode > 0) {
            this.myNameClass = 'my-name-class';
            this.nameClass = 'name-original-start';
            this.logoCLass = 'logo-original-start';
            this.headerClass = 'header-container-start';
            this.isUserNameFade = true;
          }
        },
        error: err => {
          console.log(err);
        },
      });
  }

  logoutSession(payload: RequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ResponseModel>(environment.logoffFunctionUrl, payload, {
      headers,
    });
  }

  getActiveSession(payload: RequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ResponseModel>(environment.getActiveSessionFunctionUrl, payload, {
      headers,
    });
  }

  saveActiveSession(payload: RequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ResponseModel>(environment.updatedActivityFunctionUrl, payload, {
      headers,
    });
  }

  handle(event: Event) {
    var req: RequestModel = {
      emailId: this.config.globalEmail,
      name: this.config.globalUsername,
      loginId: this.config.globalLoginId
    };
    console.log(req);

    this.saveActiveSession(req)
      .subscribe({
        next: res => {
          if (res.isSuccessful && res.returnCode > 0) {
            this.config.globalSessionTime = 1800;
            console.log(res);
          }
          else {
            console.log(res);
          }
        },
        error: err => {
          console.log(err);
        },
      });
  }
}
