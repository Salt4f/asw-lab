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
    console.log(this.idUrl);
  }
  obtenerInfoContribution(idUrl: number){
    this.apiservice.obtenerInfoContribution(idUrl).subscribe(data=> {
      console.log(data);
      this.lista = data;
    })

  }

  reply(){
    this.apiservice.reply(this.comment, this.idUrl).subscribe( data =>{
      this.obtenerInfoContribution(this.idUrl);
    });
  }
  muestraAuthor(item: any){
    console.log(item);
    this.router.navigate(['account/'+item.Author.Email]);
  }
  replyComment(item:any){
    console.log(item);
    this.comment ="";
    this.router.navigate(['contribution/'+item.Id]);
    this.obtenerInfoContribution(item.Id);
  }

  votar(e: Event,sub : any){
    e.stopPropagation();
    this.apiservice.upvoteContribution(sub.Id).subscribe(data =>{
      sub.UpvotedByUser = true;
      this.obtenerInfoContribution(this.idUrl);
    });
  }

  desvotar(e: Event,sub : any)  {
    e.stopPropagation();
    console.log(sub);
    this.apiservice.downvoteContribution(sub.Id).subscribe(data =>{
      sub.UpvotedByUser = true;
      this.obtenerInfoContribution(this.idUrl);
    });
  }


}
