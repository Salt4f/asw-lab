import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from "./material.modules";
import { HeaderComponent } from './components/header/header.component';

import { MatDialogModule } from "@angular/material/dialog";
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NewComponent } from './pages/new/new.component';
import { ThreadComponent } from './pages/thread/thread.component';
import { SubmitComponent } from './pages/submit/submit.component';
import { DialogPopUpComponent } from './dialogs/dialog-pop-up/dialog-pop-up.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { registerLocaleData } from '@angular/common';


import es from '@angular/common/locales/es';
import { AccountComponent } from './pages/account/account.component';
import { ContributionComponent } from './pages/contribution/contribution.component';
registerLocaleData(es);

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    NewComponent,
    ThreadComponent,
    SubmitComponent,
    DialogPopUpComponent,
    AccountComponent,
    ContributionComponent
  ],
  imports: [
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    FlexLayoutModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    MatTableModule,
    MatDialogModule
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'es-ES' }],
  bootstrap: [AppComponent]
})
export class AppModule { }
