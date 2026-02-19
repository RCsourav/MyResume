import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  OnInit, Component, signal
  , Renderer2
  , HostListener
} from '@angular/core';
import { environment,AppConfigService} from '../environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('MyResume');

  constructor(private renderer: Renderer2, private config: AppConfigService) { }

  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload(event: BeforeUnloadEvent) {
    const url = environment.logoffFunctionUrl;

    const payload = JSON.stringify({
      loginId: this.config.globalLoginId,
      name: this.config.globalUsername,
      emailId: this.config.globalEmail,
    });

    const blob = new Blob([payload], { type: 'application/json' });

    navigator.sendBeacon(url, blob);

    this.config.globalUsername = '';
    this.config.globalEmail = '';
    this.config.globalLoginId = 0;
  }


  ngOnInit() {
    this.renderer.listen('document', 'click', (event) => this.handle(event));
    this.renderer.listen('document', 'keydown', (event) => this.handle(event));
    this.renderer.listen('document', 'scroll', (event) => this.handle(event));
  }

  handle(event: Event) {
    //call function
  }
}
