import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('MyResume');
}
