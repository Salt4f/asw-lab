import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContribUserComponent } from './pages/contrib-user/contrib-user.component';
import { ContributionComponent } from './pages/contribution/contribution.component';
import { NewComponent } from './pages/new/new.component'
import { SubmitComponent } from './pages/submit/submit.component';
import { ThreadComponent } from './pages/thread/thread.component';
import { AccountComponent } from './pages/account/account.component';

const routes: Routes = [
  {path: '', component: NewComponent},
  {path: 'news', component: NewComponent},
  {path: 'asks', component: NewComponent},
  {path: 'submit', component: SubmitComponent},
  {path: 'threads', component: ThreadComponent},
  {path: 'contribution/:id', component: ContributionComponent},
  {path: 'submissions/:usermail', component: ContribUserComponent},
  {path: 'comments/:usermail', component: ContribUserComponent},
  {path: 'myUpvotedContributions', component: NewComponent},
  {path: 'myUpvotedComments', component: NewComponent},
  { path: 'threads', component: ThreadComponent },
  { path: 'account', component: AccountComponent },
  {path: 'contribution/:id', component: ContributionComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
