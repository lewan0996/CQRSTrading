import { Component, HostBinding } from '@angular/core';
import { ScreenService } from './layout/layouts/screen.service';
import { AppInfoService } from './shared/services/app-info.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent  {
  @HostBinding('class') get getClass() {
    return Object.keys(this.screen.sizes).filter(cl => this.screen.sizes[cl]).join(' ');
  }

  constructor(private screen: ScreenService, public appInfo: AppInfoService) { }
}
