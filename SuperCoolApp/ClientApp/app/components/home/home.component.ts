import { Component } from '@angular/core'; //importante e fondamentale per dichiarare un componente
@Component({ //questo e' un decoratore per definire un componente
    selector: 'home',
    templateUrl: './home.component.html'
}) //questi sono i metadati del mio componente
export class HomeComponent { //grazie al decoratore indichiamo che HomeCOmponent è un componente che puo
							 //essere gestito da Angular
}
