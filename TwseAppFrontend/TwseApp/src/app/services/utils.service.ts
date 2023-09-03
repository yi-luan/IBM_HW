import { Injectable } from '@angular/core';
import { EVENT_CLOSE_LOADING, EVENT_SHOW_LOADING } from 'src/utils/loadingSpinner/loadingspinner.component';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  constructor() { }

  public static dispatchEvent(event: string) {
    window.dispatchEvent(new CustomEvent(event));
  }

  public static showLoading(){
    UtilsService.dispatchEvent(EVENT_SHOW_LOADING);
  }

  public static closeLoading(){
    setTimeout(() => {
      UtilsService.dispatchEvent(EVENT_CLOSE_LOADING);
    },500)
  }


}
