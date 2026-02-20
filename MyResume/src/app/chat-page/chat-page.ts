import {
  Component, ElementRef
  , Renderer2, ViewChild
  , AfterViewInit
  , AfterViewChecked
  , OnInit
  , OnDestroy
} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment, AppConfigService } from '../../environment';
import { Router } from '@angular/router';

import { RequestModel } from '../models/request';
import { ResponseModel } from '../models/response';
import { ChatRequestModel } from '../models/chatRequest';
import { ChatResponseModel } from '../models/chatResponse';



@Component({
  selector: 'app-chat-page',
  standalone: false,
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
})
export class ChatPage implements AfterViewInit, AfterViewChecked, OnInit, OnDestroy {
  @ViewChild('parentDiv') parentDiv!: ElementRef;
  @ViewChild('mainDiv') mainDiv!: ElementRef;

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
  }

  ngOnDestroy() {
    this.stopTimer();
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

  logoutSession(payload: RequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ResponseModel>(environment.logoffFunctionUrl, payload, {
      headers,
    });
  }

  saveChatSession(payload: ChatRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post<ChatResponseModel>(environment.saveChatDataFunctionUrl, payload, {
      headers,
    });
  }

  callAgentChat(payload: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'text/plain',
      'x-functions-key': '',

    });

    return this.http.post(environment.aiFunctionUrl, payload, {
      headers,
      responseType: 'text'
    }
    );
  }

  convertToMMSS(totalSeconds: number): string {
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;

    const mm = minutes.toString().padStart(2, '0');
    const ss = seconds.toString().padStart(2, '0');

    return `${mm}:${ss}`;
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

  addRequestDiv(request: string) {
    var fakeInput = this.parentDiv.nativeElement.querySelector('.fake-input');
    const newDiv = this.renderer.createElement('div');

    const text = this.renderer.createText(request);
    this.renderer.appendChild(newDiv, text);

    this.renderer.setStyle(newDiv, 'text-align', 'right');
    this.renderer.setStyle(newDiv, 'max-width', '70%');
    this.renderer.setStyle(newDiv, 'color', '#1B0233');
    this.renderer.setStyle(newDiv, 'align-self', 'flex-end');
    this.renderer.setStyle(newDiv, 'margin', '2vh');

    this.renderer.setStyle(newDiv, 'background-color', 'rgb(255,255,255,0.3)');
    this.renderer.setStyle(newDiv, 'padding', '2.5vh');
    this.renderer.setStyle(newDiv, 'border-start-end-radius', '5vh');
    this.renderer.setStyle(newDiv, 'border-end-start-radius', '5vh');
    this.renderer.setStyle(newDiv, 'border-start-start-radius', '5vh');
    this.renderer.setStyle(newDiv, 'box-shadow', '10px 10px 5px rgba(0,0,0,0.5)');
    this.renderer.setStyle(newDiv, 'width', 'fit-content');

    this.renderer.insertBefore(this.parentDiv.nativeElement, newDiv
      , fakeInput);

    this.isInputFade = false;
  }

  addResponseDiv(request: string) {
    var lines = request.split('\r\n');
    var fakeInput = this.parentDiv.nativeElement.querySelector('.fake-input');
    const newDiv = this.renderer.createElement('div');

    this.renderer.setStyle(newDiv, 'text-align', 'left');
    this.renderer.setStyle(newDiv, 'max-width', '70%');
    this.renderer.setStyle(newDiv, 'min-width', '10%');
    this.renderer.setStyle(newDiv, 'color', '#ffffff');
    this.renderer.setStyle(newDiv, 'margin', '2vh');

    this.renderer.setStyle(newDiv, 'background-color', 'rgb(0,0,0,0.3)');
    this.renderer.setStyle(newDiv, 'padding', '2.5vh');
    this.renderer.setStyle(newDiv, 'border-start-end-radius', '5vh');
    this.renderer.setStyle(newDiv, 'border-end-end-radius', '5vh');
    this.renderer.setStyle(newDiv, 'border-start-start-radius', '5vh');
    this.renderer.setStyle(newDiv, 'box-shadow', '10px 10px 5px rgba(0,0,0,0.5)');
    this.renderer.setStyle(newDiv, 'width', 'fit-content');

    this.renderer.insertBefore(this.parentDiv.nativeElement
      , newDiv, fakeInput);

    let delay = 0;

    lines.forEach(line => {
      var chars = line.split('');
      chars.forEach(char => {
        setTimeout(() => {
          newDiv.innerHTML += char;
        }, delay);
        delay += 2;
      });

      setTimeout(() => {
        newDiv.innerHTML += '<br>';
      }, delay);
      delay += 2;
    });


  }

  addRequest(element: HTMLTextAreaElement) {
    this.addRequestDiv(element.value);
    const request = element.value;
    element.value = '';

    var req: RequestModel = {
      emailId: this.config.globalEmail,
      name: this.config.globalUsername,
      loginId: this.config.globalLoginId
    };
    this.getActiveSession(req)
      .subscribe({
        next: res => {
          console.log(res);
          if (res.isSuccessful && res.returnCode > 0) {

            this.saveActiveSession(req)
              .subscribe({
                next: res => {
                  if (res.isSuccessful && res.returnCode > 0) {
                    this.config.globalSessionTime = 1800;
                    this.callAgentChat(request)
                      .subscribe({
                        next: res => {
                          this.isInputFade = true;
                          this.addResponseDiv(res);

                          var chatReq: ChatRequestModel = {
                            emailId: this.config.globalEmail,
                            name: this.config.globalUsername,
                            loginId: this.config.globalLoginId,
                            aiResponseMessage: res,
                            userRequestPromt: request,
                          };

                          this.saveChatSession(chatReq)
                            .subscribe({
                              next: res => {
                                console.log(res);
                              },
                              error: err => {
                                console.log(err);
                              },
                            });
                        },
                        error: err => {
                          this.isInputFade = true;
                          console.log(err);
                          this.addResponseDiv('I beg your pardon. There is something not right at my end. Could you please tell me again?');
                        },
                      });
                  }
                  else {
                    this.isInputFade = true;
                    console.log(res);
                    this.addResponseDiv('I beg your pardon. There is something not right at my end. Could you please tell me again?');
                  }
                },
                error: err => {
                  console.log(err);
                },
              });
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

    //this.addResponseDiv('Hi! I am Sourav.\r\n\r\nHow can I help you?');
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.myNameClass = 'my-name-class-last';
      this.nameClass = 'name-original-end';
      this.logoCLass = 'logo-original-end';
      this.headerClass = 'header-container-end';
      setTimeout(() => {
        this.addResponseDiv('Hi! I am Sourav Roy Choudhury.\r\nNice to meet you.\r\nYou can ask me anything about my career, education and skill set.');
        this.isUserNameFade = false;
      }, 1500);
    }, 500);
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    const el = this.mainDiv.nativeElement;
    el.scrollTop = el.scrollHeight;
  }

}
