import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  lista: any;
  about = "";
  usermail = "marc.cortadellas@estudiantat.upc.edu";

  constructor(private apiservice: ApiService,
    private router: Router) { }


  ngOnInit(): void {
    this.obtenerInformacionUsuario();
  }
  private obtenerInformacionUsuario() {
    this.apiservice.obtenerInfoUser(this.usermail).subscribe((data: any)=> {
      this.lista = data[0];
      console.log(data[0])
    });
  }

  private modificarAbout() {
    this.apiservice.modificarAbout(this.usermail, this.about).subscribe();
  }

}
