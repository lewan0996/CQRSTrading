import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DxDataGridModule, DxFormModule } from 'devextreme-angular';

const routes: Routes = [

];

@NgModule({
  imports: [RouterModule.forRoot(routes), DxDataGridModule, DxFormModule],
  providers: [],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule { }
