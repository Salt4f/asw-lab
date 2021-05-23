import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Submision } from "./Interface";

/*
AQUI HAREMOS LAS LLAMADAS A LA API

EJEMPLO:
email es de tipo string, se puede poner ": any" i tan panxos
obtenerContributionsByEmail(email: string){
    return this.http.get<any[]>(environmet.apiUrl + environment.users + '/' + email );
}
//devolvemos un array de cosas (any). en enviroment tendremos el nombre de variables de las url

*/
const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      Authorization: 'f6e205fda57d593885b06558834952d4cd2149f584e6d8b265438e0edfcefe0ef8e15f8cf343797370e8686166ae6bf14efea275cc206dbde35826c0d637e176'
    })
};

@Injectable({
    providedIn: 'root'
  })
export class ApiService {
  
    constructor(
      private http: HttpClient
    ) { }


    obtenerNewsByVote(){
        return this.http.get<Submision[]>(environment.apiUrl + '/contributions');
    }
}

  