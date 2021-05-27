import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-contribution',
  templateUrl: './contribution.component.html',
  styleUrls: ['./contribution.component.css']
})
export class ContributionComponent implements OnInit {

  idUrl = 0;
  comment = "";
  lista: any;

  constructor(
    private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.obtenerId(this.router.url);
    this.obtenerInfoContribution(this.idUrl);
  }

  obtenerId(url: string){
    let pos = url.lastIndexOf("/");
    this.idUrl = parseInt(url.substring(pos +1));
  }
  obtenerInfoContribution(idUrl: number){
    this.apiservice.obtenerInfoContribution(idUrl).subscribe(data=> {
      console.log(data);
      this.lista = data;
    })

  }

  reply(){
    this.apiservice.reply(this.comment, this.idUrl).subscribe();
  }



}
