import { Component, OnInit, Input } from '@angular/core';
export const EVENT_SHOW_LOADING = 'EVENT_SHOW_LOADING';
export const EVENT_CLOSE_LOADING = 'EVENT_CLOSE_LOADING';
@Component({
  selector: 'app-loadingspinner',
  templateUrl: './loadingspinner.component.html',
  styleUrls: ['./loadingspinner.component.scss'],
})
export class LoadingspinnerComponent implements OnInit {
  @Input() show = false;
  constructor() {}
  ngOnInit(): void {}
}
