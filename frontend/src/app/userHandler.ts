// text.service.ts
import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class UserHandler {
  //Subject is a special type of Observable that allows values to be multicasted to many Observers
  //It is observing, for things like changes, by using the .next()
  private textValue = new Subject<string>(); //the subject is with strings
  textValue$ = this.textValue.asObservable(); //It is observing on the textvalue

  private loginbtnValue = new Subject<string>();
  loginValue$ = this.loginbtnValue.asObservable();

  /**
   * When the value is passed into this method, it is going to observe
   * a change and all the classes that subscribed will react to it
   * @param value
   */
  updateCurrentUser(value: string): void {
    this.textValue.next(value);
    this.loginbtnValue.next("logout");
  }
}
