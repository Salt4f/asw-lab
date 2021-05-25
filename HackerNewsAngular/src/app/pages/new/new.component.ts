import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { Submisions } from 'src/app/services/Interface';

@Component({
  selector: 'app-new',
  templateUrl: './new.component.html',
  styleUrls: ['./new.component.css']
})
export class NewComponent implements OnInit {

  lista : Submisions[] = [];
  usermail = "marc.cortadellas@estudiantat.upc.edu";
  estaVotada = false;

  displayedColumns: string[] = ["buttons","title", "upvotes","comments","dateCreated","author"]  


  constructor(private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    console.log(this.router.url);
    if(this.router.url =="/"){
      this.obtenerNews();
    }
    else if(this.router.url =="/asks"){
      this.obtenerAsks();
    }
    else if(this.router.url =="/news"){
      this.obtenerContributions();
    }
  }

  private obtenerAsks(){
    this.apiservice.obtenerAsksByVote().subscribe(data => {
      this.lista = data;
    });
  }
  private obtenerNews(){
    this.apiservice.obtenerNewsByVote().subscribe(data => {
      console.log(data);
      this.lista = data;
    });
  }
  private obtenerContributions(){
    this.apiservice.obtenerNewsByCreation().subscribe(data =>{
      this.lista = data;
    });
  }
  votar(){  }

  desvotar(){  }

  muestraSubmission(item : any){
    console.log(item);
    this.router.navigate(['contribution/'+item.Id]);
   }

}
