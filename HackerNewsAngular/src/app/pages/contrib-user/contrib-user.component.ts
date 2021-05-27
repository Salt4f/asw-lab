import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from 'lodash';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-contrib-user',
  templateUrl: './contrib-user.component.html',
  styleUrls: ['./contrib-user.component.css']
})
export class ContribUserComponent implements OnInit {

  
  lista :any= [];
  usermail = "marc.cortadellas@estudiantat.upc.edu";
  estaVotada = false;
  lastParam = "";
  displayedColumns: string[] = ["buttons","title", "upvotes","comments","dateCreated"];  
  

  constructor(private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.obtenerlastParam(this.router.url);
    console.log(this.router.url);
    if(this.router.url =="/submissions/" + this.lastParam){
      this.obtenerSubmissionByUser();
    }
    else if(this.router.url =="/comments/" + this.lastParam){
      this.obtenerCommentsByUser();
    }
  }

  private obtenerSubmissionByUser(){ 
    this.apiservice.obtenerSubmissionsByMail(this.lastParam).subscribe(data =>{
      console.log(data);
      this.lista = data;
    });
  }

  private obtenerCommentsByUser(){
    this.apiservice.obtenerCommentsByMail(this.lastParam).subscribe(data =>{
      console.log(data);
        let data2 = data[0];
        console.log(data2);
      //data[0]
      this.lista = data2;
      console.log(this.lista);
    });
  }

  obtenerlastParam(url: string){
    let pos = url.lastIndexOf("/");
    this.lastParam = url.substring(pos +1);
  }

  
  
  votar(){  }

  desvotar(){  }

  muestraSubmission(item : any){
    console.log(item);
    this.router.navigate(['contribution/'+item.Id]);
  }

}
  