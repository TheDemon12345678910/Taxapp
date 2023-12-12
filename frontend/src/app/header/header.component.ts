import {Component, OnDestroy, OnInit} from '@angular/core';
import {UserHandler} from '../userHandler';
import {Subscription} from "rxjs";
import {text} from "ionicons/icons";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  private subscription: Subscription;
  dynamicText: string = '';
  loginText: string = 'login';

  constructor(private userHandler: UserHandler) {
    //Subscribe to this userhandler, so this reacts, when it is changed
    this.subscription = this.userHandler.textValue$.subscribe((value) => {
      this.dynamicText = 'Welcome ' + value;
    })
    this.subscription = this.userHandler.loginValue$.subscribe((value) => {
      this.loginText = value;
    })

  }

  ngOnInit() {
  }

}
