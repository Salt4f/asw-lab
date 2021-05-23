import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { Submision } from 'src/app/services/Interface';

@Component({
  selector: 'app-new',
  templateUrl: './new.component.html',
  styleUrls: ['./new.component.css']
})
export class NewComponent implements OnInit {

  lista : Submision[] = [];

  displayedColumns: string[] = ["buttons","title", "upvotes","comments","dateCreated","author"]  


  constructor(private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.apiservice.obtenerNewsByVote().subscribe(data => {
      this.lista = data;
      console.log(this.lista);
    });
  }

  votar(){  }

  desvotar(){  }

  muestraSubmission(item : any){
    console.log(item);
    this.router.navigate(['contribution/'+item.Id],{queryParams: item.Id});
   }

}
