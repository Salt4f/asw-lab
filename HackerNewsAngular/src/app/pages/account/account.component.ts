import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit{

  lista: any;
  about = "";
  user ="";
  usermail = "marc.cortadellas@estudiantat.upc.edu";
  apikey = environment.apikey;
  constructor(private apiservice: ApiService,
    private router: Router) { }

  

  ngOnInit(): void {
    this.obtenerlastParam(this.router.url);
    this.obtenerInformacionUsuario();
    console.log(this.usermail);
  }
  private obtenerInformacionUsuario() {
    this.apiservice.obtenerInfoUser(this.usermail).subscribe((data: any)=> {
      this.lista = data[0];
      console.log(data[0])
      this.obteneruser();
    });
  }

  modificarAbout() {
    this.apiservice.modificarAbout(this.usermail, this.lista.About).subscribe(data=>{
      //this.obtenerInformacionUsuario();
    });
  }
  obtenerlastParam(url: string){
    let pos = url.lastIndexOf("/");
    this.usermail = url.substring(pos +1);
  }

  obteneruser(){
    let pos = this.usermail.lastIndexOf("@");
    this.user = this.usermail.substring(0 ,pos);
    
  }
}
