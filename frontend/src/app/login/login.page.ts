import {Component, OnInit} from '@angular/core';
import {State} from 'src/state';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {ResponseDto, User} from 'src/models';
import {FormBuilder, Validators} from "@angular/forms";
import {environment} from 'src/environments/environment';
import {ToastController} from "@ionic/angular";
import {UserHandler} from '../userHandler';
import {TokenService} from 'src/services/token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  currentUser: User | undefined;
  //This is the formbuilder, it is important to SPELL the items as they are spelled in the dto in the API
  loginForm = this.fb.group({
    email: ['', Validators.minLength(2)],
    password: ['', Validators.minLength(8)],
  })

  constructor(private tokenService: TokenService, public state: State, private userHandler: UserHandler, public http: HttpClient, public fb: FormBuilder, public toastcontroller: ToastController) {
  }

  ngOnInit() {
  }

  async login() {
    //const observable = this.http.post<ResponseDto<User>>(environment.baseURL + '/account/login', this.loginForm.getRawValue());
    var observable = this.http.post<ResponseDto<{
      token: string
    }>>(environment.baseURL + '/account/login', this.loginForm.getRawValue());
    const response = await firstValueFrom(observable);
    this.tokenService.setToken(response.responseData!.token);
    (await this.toastcontroller.create({
      message: response.messageToClient,
      color: "sucess",
      duration: 5000
    })).present();

    //Setting the current user.
    //this.currentUser = response.responseData;
    //Securing that the logged in user accually has the information, and not just an empty object
    if (this.currentUser !== undefined) {
      this.state.setCurrentUser(this.currentUser);
      this.changeNameOfCurrentUser(this.state.getCurrentUser().username);
    }
  }

  async logout() {
    this.tokenService.clearToken();

    (await this.toastcontroller.create({
      message: 'Successfully logged out',
      duration: 5000,
      color: 'success',
    })).present()
  }

  changeNameOfCurrentUser(name: any): void {
    this.userHandler.updateCurrentUser(name);
  }


}
