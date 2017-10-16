import { Component } from '@angular/core';
//elemento principale della nostra app
@Component({ //decoratore
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent { //si tratta di una cosa JavaScript che rende disponibile al di fuori del mio file
							//l'oggetto app component che verrà preso da bootstrap.
}
