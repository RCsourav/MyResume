import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'chatpage', pathMatch: 'full' },
  { path: 'chatpage', loadChildren: () => import('./chat-page/chat-page-module').then(m => m.ChatPageModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
