import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { LoginPage } from './login-page';

import { LoginPageRoutingModule } from './login-page-routing-module';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [LoginPage],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    LoginPageRoutingModule
  ]
})
export class LoginPageModule { }
