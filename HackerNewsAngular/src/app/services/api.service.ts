import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Submisions } from "./Interface";


/*
const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      Authorization: environment.apikey
    })
};
*/

@Injectable({
    providedIn: 'root'
  })
export class ApiService {
    
    constructor(
      private http: HttpClient
    ) { }
/*
    createAuthorizationHeader(headers: Headers) {
      headers.append('Content-Type', 'application/json');
      headers.append('x-api-key', 'f6e205fda57d593885b06558834952d4cd2149f584e6d8b265438e0edfcefe0ef8e15f8cf343797370e8686166ae6bf14efea275cc206dbde35826c0d637e176');
    }
*/
    createAuthorizationHeader(headers: HttpHeaders) : HttpHeaders {
      return headers.append('x-api-key', environment.apikey)//.append('hola-hola', 'hola');
    }

    obtenerNewsByVote(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);      
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + "?usermail=" + environment.usermail);

    }
    obtenerAsksByVote(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.asks + "?usermail=" + environment.usermail);
    }
    obtenerNewsByCreation(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.news + "?usermail=" + environment.usermail);
    }
    modificarAbout(usermail : string, about : string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      let body = new FormData();
      body.append("about", about);
      return this.http.post(environment.apiUrl + environment.users + "/" + usermail + environment.about, body, {headers : header});
    }
    obtenerInfoUser(usermail: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<any>(environment.apiUrl + environment.users + "/" + usermail );
    }

    submit(usermail: string, title: string, url: string, content: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      let body = new FormData();
      body.append("title", title);
      body.append("content", content);
      body.append("url", url);
      console.log("algo");
      return this.http.post(environment.apiUrl + environment.contribution, body);
    }



    obtenerInfoContribution(id: number){
      return this.http.get<any>(environment.apiUrl + environment.contribution + '/' +id);
    }


    obtenirThreadsByUser(usermail: string){
      //const header = new Headers();
      //this.createAuthorizationHeader(header);
      return this.http.get<any[]>(environment.apiUrl + environment.users + '/' + usermail + environment.threads);
    }

    obtenerSubmissionsByMail(usermail: any){
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.contribution);
    }
    

    obtenerCommentsByMail(usermail: any){
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.comments);
    }

    obtenerUpbotedContributionPrivate(usermail: string){
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.upvotedContributions);

    }
    obtenerUpbotedCommentsPrivate(usermail: string){
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.upvotedComments );
    }
  

    upvoteContribution(id: number) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.post(environment.apiUrl + environment.contribution + '/' + id + '/upvote', null);
    }

    downvoteContribution(id: number) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.delete(environment.apiUrl + environment.contribution + '/' + id + '/upvote');
    }

    reply(comment: string, id: number) {
      let body = new FormData();
      body.append("comment", comment);
      //const body = JSON.stringify({comment, id});
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.post(environment.apiUrl + environment.contribution+ '/' + id, body );
    }

}

