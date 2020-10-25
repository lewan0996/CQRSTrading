import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxDrawerModule } from 'devextreme-angular/ui/drawer';
import { DxScrollViewModule } from 'devextreme-angular/ui/scroll-view';
import { DxButtonModule } from 'devextreme-angular/ui/button';
import { DxToolbarModule } from 'devextreme-angular/ui/toolbar';
import { DxTreeViewModule } from 'devextreme-angular/ui/tree-view';
import { DxListModule } from 'devextreme-angular/ui/list';
import { DxContextMenuModule } from 'devextreme-angular/ui/context-menu';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AppInfoService } from './services/app-info.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    DxDrawerModule,
    DxScrollViewModule,
    CommonModule,
    DxButtonModule,
    DxToolbarModule,
    DxTreeViewModule,
    DxListModule,
    DxContextMenuModule,
    AppRoutingModule
  ],
  exports: [
    AppRoutingModule,
    CommonModule,
    DxDrawerModule,
    DxScrollViewModule,
    CommonModule,
    DxButtonModule,
    DxToolbarModule,
    DxTreeViewModule,
    DxListModule,
    DxContextMenuModule,
  ],
  providers: [
    AppInfoService
  ]
})
export class SharedModule { }
