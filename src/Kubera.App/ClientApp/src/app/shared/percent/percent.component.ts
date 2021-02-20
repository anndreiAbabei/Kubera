import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-percent',
    templateUrl: './percent.component.html',
    styleUrls: ['./percent.component.scss']
})
/** percent component*/
export class PercentComponent {
  @Input()
  public percent: number;

  public getIncreaseIndicatorClass(increase: number): string {
    if (increase < -100) {
      return 'bad-100';
    } else if (increase < -50) {
      return 'bad-50';
    } else if (increase < -10) {
      return 'bad-10';
    } else if (increase > 100) {
      return 'good-100';
    } else if (increase > 50) {
      return 'good-50';
    } else if (increase > 10) {
      return 'good-10';
    }

    return 'usual';
  }
}
