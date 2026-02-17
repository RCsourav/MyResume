import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { ChatPageRoutingModule } from './chat-page-routing-module';
import { HttpClientModule } from '@angular/common/http';


import { ChatPage } from './chat-page';


@NgModule({
  declarations: [ChatPage],
  imports: [
    CommonModule,
    ChatPageRoutingModule,
    HttpClientModule
  ]
})
export class ChatPageModule { }
