import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';


const modulosMaterial = [
  MatToolbarModule,
  MatIconModule,
  MatMenuModule,
  MatFormFieldModule,
  MatInputModule,
  FormsModule,
]


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    modulosMaterial
  ],
  exports: [
    modulosMaterial
  ],
  providers:[
    {provide: MAT_DATE_LOCALE, useValue: 'es-ES'},
  ]
})
export class MaterialModule { }
