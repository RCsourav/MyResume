import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from './environment';

export interface AgentReply {
  threadId: string;
  reply: string;
}


@Injectable({
  providedIn: 'root'
})


export class FunctionService {

  constructor(private http: HttpClient) { }

  callAgentChat(payload: any) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'x-functions-key': environment.functionKey
    });

    return this.http.post<AgentReply>(environment.functionUrl, payload, { headers });
  }
}
