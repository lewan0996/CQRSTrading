import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNavigationMenuComponent } from '../components/side-navigation-menu/side-navigation-menu.component';
import { DxDrawerModule } from 'devextreme-angular/ui/drawer';
import { HeaderComponent } from '../components/header/header.component';
import { DxScrollViewModule } from 'devextreme-angular/ui/scroll-view';
import { SideNavOuterToolbarComponent } from '../layouts/side-nav-outer-toolbar/side-nav-outer-toolbar.component';
import { FooterComponent } from '../components/footer/footer.component';
import { DxButtonModule } from 'devextreme-angular/ui/button';
import { DxToolbarModule } from 'devextreme-angular/ui/toolbar';
import { DxTreeViewModule } from 'devextreme-angular/ui/tree-view';
import { DxListModule } from 'devextreme-angular/ui/list';
import { DxContextMenuModule } from 'devextreme-angular/ui/context-menu';
import { UserPanelComponent } from '../components/user-panel/user-panel.component';
import { AppRoutingModule } from 'src/app/app-routing.module';



@NgModule({
  declarations: [
    SideNavOuterToolbarComponent,
    FooterComponent,
    HeaderComponent,
    SideNavigationMenuComponent,
    UserPanelComponent
  ],
  imports: [
    CommonModule,
    DxDrawerModule,
    DxScrollViewModule,
    CommonModule,
    CommonModule,
    DxButtonModule,
    DxToolbarModule,
    DxTreeViewModule,
    DxListModule,
    DxContextMenuModule,
    CommonModule,
    AppRoutingModule
  ],
  exports:[
    FooterComponent,
    SideNavOuterToolbarComponent,
    SideNavigationMenuComponent,
    AppRoutingModule
  ]
})
export class SharedModule { }
