import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResumePage }from './resume-page/resume-page'

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', loadChildren: () => import('./login-page/login-page-module').then(m => m.LoginPageModule) },
  { path: 'chat', loadChildren: () => import('./chat-page/chat-page-module').then(m => m.ChatPageModule) },
  { path: 'resume', component: ResumePage },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
