import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
  styleUrls: ['./submit.component.css']
})
export class SubmitComponent implements OnInit {

  lista: any;

  title = "";
  url = "";
  content = "";

  usermail = "marc.cortadellas@estudiantat.upc.edu";

  constructor(private apiservice: ApiService,
    private router: Router) { }

  ngOnInit(): void {

  }

  submit() {
    this.apiservice.submit( this.title, this.url, this.content).subscribe((data: any) => {
     this.router.navigate(["contribution/" + data]);

    });
  }
}
