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
  lastParam = "";
  selectedItem :any;
  displayedColumns: string[] = ["buttons","title", "upvotes","comments","dateCreated","author"]  
  

  constructor(private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.obtenerlastParam(this.router.url);
    console.log(this.router.url);
    this.obtenerItems();
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

  private obtenerSubmissionByUser(){
    
    this.apiservice.obtenerSubmissionsByMail(this.usermail).subscribe(data =>{

      console.log(data);
      this.lista = data;
    });
  }

  private obtenerCommentsByUser(){
    this.apiservice.obtenerCommentsByMail(this.usermail).subscribe(data =>{
      console.log(data);
      this.lista = data;
    });
  }

  private obtenermyUpvotedComments(){
    this.apiservice.obtenerUpbotedCommentsPrivate(this.usermail).subscribe(data =>{
      this.lista = data;
    });

  }

  private obtenermyUpvotedContributions(){
    this.apiservice.obtenerUpbotedContributionPrivate(this.usermail).subscribe(data =>{
      this.lista = data;
    });
  }


  obtenerlastParam(url: string){
    let pos = url.lastIndexOf("/");
    this.lastParam = url.substring(pos +1);
  }

  
  votar(e: Event,sub : Submisions){
    e.stopPropagation();
    this.apiservice.upvoteContribution(sub.Id).subscribe(data =>{
      sub.UpvotedByUser = true;
      this.obtenerItems();
    });
  }

  desvotar(e: Event,sub : Submisions)  {
    e.stopPropagation();
    this.apiservice.downvoteContribution(sub.Id).subscribe(data =>{
      sub.UpvotedByUser = true;
      this.obtenerItems();
    });
  }

  muestraSubmission(item : any){
    console.log(item);
    this.selectedItem = item;
    this.router.navigate(['contribution/'+item.Id]);
  }
  muestraAuthor(e:Event, item: Submisions){
    e.stopPropagation();
    console.log(item);
    this.router.navigate(['account/'+item.Author.Email]);
  }
  obtenerItems(){
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

}
