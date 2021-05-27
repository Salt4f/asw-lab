import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Submisions } from "./Interface";

/*
AQUI HAREMOS LAS LLAMADAS A LA API

EJEMPLO:
email es de tipo string, se puede poner ": any" i tan panxos
obtenerContributionsByEmail(email: string){
    return this.http.get<any[]>(environmet.apiUrl + environment.users + '/' + email );
}
//devolvemos un array de cosas (any). en enviroment tendremos el nombre de variables de las url


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

    createAuthorizationHeader(headers: HttpHeaders) : HttpHeaders {
      return headers.append('x-api-key', environment.apikey)//.append('hola-hola', 'hola');
    }

    obtenerNewsByVote(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);      
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution);// + "?usermail=" + environment.usermail, {headers: header});

    }
    obtenerAsksByVote(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.asks + "?usermail=" + environment.usermail, {headers: header});
    }
    obtenerNewsByCreation(){
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<Submisions[]>(environment.apiUrl + environment.contribution + environment.news + "?usermail=" + environment.usermail, {headers: header});
    }
    modificarAbout(usermail : string, about : string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      const body = JSON.stringify({about});
      return this.http.post(environment.apiUrl + environment.users + "/" + usermail + environment.about, body, { headers: header });
    }
    obtenerInfoUser(usermail: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.get<any>(environment.apiUrl + environment.users + "/" + usermail );
    }

    submit(usermail: string, title: string, url: string, content: string) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      const body = JSON.stringify({ title, url, content });
      return this.http.post(environment.apiUrl + environment.contribution, body, { headers: header });
    }



    obtenerInfoContribution(id: number){
      return this.http.get<any>(environment.apiUrl + environment.contribution + '/' +id);
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
      return this.http.post(environment.apiUrl + environment.contribution + '/' + id + '/upvote', null, {headers: header});
    }

    downvoteContribution(id: number) {
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.delete(environment.apiUrl + environment.contribution + '/' + id + '/upvote', {headers: header});
    }

    reply(comment: string, id: number) {
      const body = JSON.stringify({comment, id});
      let header = new HttpHeaders();
      header = this.createAuthorizationHeader(header);
      return this.http.post(environment.apiUrl + environment.contribution+ '/' + id, body, {headers: header} );
    }

    
}

