import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DialogPopUpComponent } from 'src/app/dialogs/dialog-pop-up/dialog-pop-up.component';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  usermail = environment.usermail;
  title ="";
  
  constructor(
    private router: Router,
    public dialog: MatDialog
  ) { }


  ngOnInit(): void {
  }

  irALaRuta(ruta: string, item: string){
    this.router.navigate([ruta,item]);
  }

  
  redirect(url: string){
    this.redirectTo(url);
  }
  redirectTo(uri:string){
    console.log(uri);
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(()=>
    this.router.navigate([uri]));
 }


}
