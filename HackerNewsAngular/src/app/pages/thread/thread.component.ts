import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { DateToString } from 'src/app/services/genericFunctions';
import { Submisions } from 'src/app/services/Interface';

@Component({
  selector: 'app-thread',
  templateUrl: './thread.component.html',
  styleUrls: ['./thread.component.css']
})
export class ThreadComponent implements OnInit {

  lista: any[] = [];
  displayedColumns: string[] = ["title", "upvotes","dateCreated","author"];
  usermail = "marc.cortadellas@estudiantat.upc.edu";
  
  constructor(
    private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.obtenirThreads();
    
  }

  obtenirThreads(){
    console.log(this.usermail);
    this.apiservice.obtenirThreadsByUser(this.usermail).subscribe(data => {
      console.log(data);
      //this.lista = data[0].map( (thread: { Id: any; Content: any; DateCreated: any; Title: any; Upvotes: any; pId: any; pTitle: any; }) =>({Id: thread.Id, Content: thread.Content, Upvotes: thread.Upvotes, DateCreated: DateToString(thread.DateCreated), Title: thread.Title, pId: thread.Parent.Id, pTitle: thread.Parent.Title }));
      console.log(data);
      console.log(this.lista[0]);
      this.lista.sort((a, b) => (a.Id < b.Id ? -1 : 1));
    });
  }
  muestraSubmission(item : any){
    console.log(item);
    this.router.navigate(['contribution/'+item.Id]);
   }


}
