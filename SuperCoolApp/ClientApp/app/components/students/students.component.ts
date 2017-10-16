import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http'; //modulo angular necessario per utilizzare http

@Component({
    selector: 'students',
    templateUrl: './students.component.html'
})
export class StudentsComponent {
    public students: Student[]; //array di studenti che viene valorizzato con le cose che prendo dal server

	//async refresh data
	//http e' un modulo angular 
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/students').subscribe(result => {
            this.students = result.json() as Student[];
        }, error => console.error(error));
    }
}

//nasce la necessità di creare una classe o un'interfaccia di tipo student
interface Student {
    name: string;
    dateOfBirth: Date;
}
