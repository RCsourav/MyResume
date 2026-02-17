import {
  Component, ElementRef
  , Renderer2, ViewChild
  , AfterViewInit
  , AfterViewChecked
} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment } from '../../environment';

interface AgentReply {
  threadId: string;
  reply: string;
}

@Component({
  selector: 'app-chat-page',
  standalone: false,
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
})
export class ChatPage implements AfterViewInit, AfterViewChecked {
  @ViewChild('parentDiv') parentDiv!: ElementRef;
  @ViewChild('mainDiv') mainDiv!: ElementRef;

  imageClass = 'image-initial';
  byClass = 'by-extended';
  nameClass = 'name-extended';
  headerClass = 'header-class-extended';
  headerText = 'MY RESUME';
  isFade = true;
  isFadeBy = true;
  isInputFade = true;

  constructor(private renderer: Renderer2, private http: HttpClient) { }

  callAgentChat(payload: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'x-functions-key': environment.functionKey
    });

    return this.http.post<AgentReply>(environment.functionUrl, payload, { headers });
  }

  addRequestDiv(request: string) {
    var fakeInput = this.parentDiv.nativeElement.querySelector('.fake-input');
    const newDiv = this.renderer.createElement('div');

    const text = this.renderer.createText(request);
    this.renderer.appendChild(newDiv, text);

    this.renderer.setStyle(newDiv, 'text-align', 'right');
    this.renderer.setStyle(newDiv, 'width', '60%');
    this.renderer.setStyle(newDiv, 'color', '#E3C62D');
    this.renderer.setStyle(newDiv, 'align-self', 'flex-end');
    this.renderer.setStyle(newDiv, 'margin', '2vh');

    this.renderer.insertBefore(this.parentDiv.nativeElement, newDiv
      , fakeInput);

    this.isInputFade = false;
  }

  addResponseDiv(request: string) {
    var lines = request.split('\r\n');
    var fakeInput = this.parentDiv.nativeElement.querySelector('.fake-input');
    const newDiv = this.renderer.createElement('div');

    this.renderer.setStyle(newDiv, 'text-align', 'left');
    this.renderer.setStyle(newDiv, 'width', '60%');
    this.renderer.setStyle(newDiv, 'color', '#ffffff');
    this.renderer.setStyle(newDiv, 'margin', '2vh');

    this.renderer.insertBefore(this.parentDiv.nativeElement
      , newDiv, fakeInput);

    let delay = 0;

    lines.forEach(line => {
      var chars = line.split('');
      chars.forEach(char => {
        setTimeout(() => {
          newDiv.innerHTML += char;
        }, delay);
        delay += 30;
      });

      setTimeout(() => {
        newDiv.innerHTML += '<br>';
      }, delay);
      delay += 30;
    });


  }

  addRequest(element: HTMLTextAreaElement) {
    this.addRequestDiv(element.value);
    const request = { promt: element.value, treadId: '' };

    this.callAgentChat(request)
      .subscribe({
        next: res => {
          this.isInputFade = true;
          this.addResponseDiv(res.reply)
        },
        error: err => {
          this.isInputFade = true;
          console.log(err);
          this.addResponseDiv('I beg your pardon. There is something not right at my end. Could you please tell me again?');
        },
      });

    //this.addResponseDiv('Hi! I am Sourav.\r\n\r\nHow can I help you?');
    element.value = '';
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.imageClass = 'image-extended';
      this.isFade = false;
      this.isFadeBy = false;
    }, 1000);
    setTimeout(() => {
      this.headerClass = 'header-class';
      this.imageClass = 'image';
      this.nameClass = 'name';
      this.isFadeBy = true;
      this.headerText = 'MR';
    }, 2000);
    setTimeout(() => {
      this.addResponseDiv('Hi! I am Sourav Roy Choudhury.\r\nNice to meet you.\r\nYou can ask me anything about my career, education and skill set.')
    }, 2500);
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    const el = this.mainDiv.nativeElement;
    el.scrollTop = el.scrollHeight;
  }

}
