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
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + "?usermail=" + environment.usermail, {headers : header});

    }
    obtenerAsksByVote(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.asks + "?usermail=" + environment.usermail, {headers : header});
    }
    obtenerNewsByCreation(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.news + "?usermail=" + environment.usermail, {headers : header});
    }
    modificarAbout(usermail : string, about : string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      let body = new FormData();
      body.append("about", about);
      console.log(body);
      return this.http.put(environment.apiUrl + environment.users + "/" + usermail + environment.about, body, {headers : header});
    }
    obtenerInfoUser(usermail: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<any>(environment.apiUrl + environment.users + "/" + usermail, {headers : header} );
    }

    submit( title: string, url: string, content: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      let body = new FormData();
      body.append("title", title);
      body.append("content", content);
      body.append("url", url);
      
      return this.http.post(environment.apiUrl + environment.contribution, body, {headers : header});
    }

    obtenerInfoContribution(id: number){
      return this.http.get<any>(environment.apiUrl + environment.contribution + '/' +id);
    }

    obtenirThreadsByUser(usermail: string){
      let header  = new HttpHeaders();
      this.createAuthorizationHeader(header);
      return this.http.get<any[]>(environment.apiUrl + environment.users + '/' + usermail + environment.threads, {headers : header});
    }

    obtenerSubmissionsByMail(usermail: any){
      let header  = new HttpHeaders();
      this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.contribution, {headers : header});
    }
    

    obtenerCommentsByMail(usermail: any){
      let header  = new HttpHeaders();
      this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.comments, {headers : header});
    }

    obtenerUpbotedContributionPrivate(usermail: string){
      let header  = new HttpHeaders();
      this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.upvotedContributions, {headers : header});

    }
    obtenerUpbotedCommentsPrivate(usermail: string){
      let header  = new HttpHeaders();
      this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.users +'/' + usermail + environment.upvotedComments, {headers : header} );
    }
  

    upvoteContribution(id: number) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.post(environment.apiUrl + environment.contribution + '/' + id + '/upvote', null, {headers : header});
    }

    downvoteContribution(id: number) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.delete(environment.apiUrl + environment.contribution + '/' + id + '/upvote', {headers : header});
    }

    reply(comment: string, id: number) {
      let body = new FormData();
      body.append("comment", comment);
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.post(environment.apiUrl + environment.contribution+ '/' + id, body, {headers : header} );
    }

}

