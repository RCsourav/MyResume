import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  OnInit, Component, signal
  , Renderer2
  , HostListener
} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('MyResume');

  constructor(private renderer: Renderer2) { }

  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload(event: BeforeUnloadEvent) {
    debugger;
    //const url = 'https://your-function.azurewebsites.net/api/AgentChat?code=YOUR_KEY';

    //const payload = JSON.stringify({
    //  message: 'User closed the browser'
    //});

    //const blob = new Blob([payload], { type: 'application/json' });

    //navigator.sendBeacon(url, blob);
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
