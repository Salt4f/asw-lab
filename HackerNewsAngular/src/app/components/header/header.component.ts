import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DialogPopUpComponent } from 'src/app/dialogs/dialog-pop-up/dialog-pop-up.component';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  usermail = "marc.cortadellas";
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
  hazAlgo(titulo: string): void{
    this.usermail = "";
    
    
    const dialogRef = this.dialog.open(DialogPopUpComponent, {
      width: '500px',
      data: { title: titulo }
    });
    dialogRef.afterClosed().subscribe(result =>{
      if(result) alert("Cerrado el dialog con boton aceptar");  
    });
  }

  login(){
    this.usermail = "marc.cortadellas";
  }


}
