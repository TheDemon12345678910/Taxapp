import {Component} from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {State} from 'src/state';
import {firstValueFrom} from 'rxjs';
import {TaxiFare, ResponseDto, TaxiPricesDto, ConfirmPriceDTO} from 'src/models'
import {environment} from 'src/environments/environment';
import {ModalController} from "@ionic/angular";
import {ConfirmPriceComponent} from '../confirm-price/confirm-price.component';


@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['home.page.scss'],
})


export class HomePage {
  date: string | undefined;
  datetimeValue: string | undefined;
  persons: number | undefined;


  constructor(public state: State, public http: HttpClient, public modalController: ModalController) {
  }

  async searchForPrices(km: number, min: number, persons: number | undefined) {
    const result = await firstValueFrom(this.http.get<ResponseDto<TaxiPricesDto>>(environment.baseURL + "/TaxaApis/GetTaxaPrices/" + km + "," + min + "," + persons))
    this.state.TaxiPrices = result.responseData!;

  }

  async clickedCard(taxiFare: TaxiFare) {
    const confirmPriceDTO: ConfirmPriceDTO = this.convertToConfirmPriceDTO(taxiFare);
    const modal = await this.modalController.create({
      component: ConfirmPriceComponent,
      componentProps: { //Videregiver dataen fra denne modal til den nye der popper op
        data: confirmPriceDTO,
      },
    });
    modal.present();
  }

  convertToConfirmPriceDTO(taxiFare: TaxiFare): ConfirmPriceDTO {  //Konventere en TaxiFare til en ConfirmPriceDTO
    return {
      companyName: taxiFare.companyName,
      km: 5, //Skal ændres til  googleAPI's distance
      min: 5, //Skal ændres til googleAPI's minutter
      persons: this.persons,
      price: taxiFare.price,
    };
  }

}
