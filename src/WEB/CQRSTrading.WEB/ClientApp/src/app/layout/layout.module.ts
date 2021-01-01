import { NgModule } from '@angular/core';
import { SideNavOuterToolbarComponent } from './layouts/side-nav-outer-toolbar/side-nav-outer-toolbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { SideNavigationMenuComponent } from './components/side-navigation-menu/side-navigation-menu.component';
import { UserPanelComponent } from './components/user-panel/user-panel.component';
import { AuthModule } from '../auth/auth.module';
import { SharedModule } from '../shared/shared.module';
import { ScreenService } from './layouts/screen.service';

@NgModule({
  declarations: [
    SideNavOuterToolbarComponent,
    FooterComponent,
    HeaderComponent,
    SideNavigationMenuComponent,
    UserPanelComponent
  ],
  imports: [
    SharedModule,
    AuthModule
  ],
  exports: [
    FooterComponent,
    SideNavOuterToolbarComponent,
    SideNavigationMenuComponent,
    SharedModule
  ],
  providers: [ScreenService]
})
export class LayoutModule { }
