import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { AjoutdemandeComponent } from './ajoutdemande/ajoutdemande.component';
import { AdminRoutingModule } from './admin-routing.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    LayoutComponent,
    DashboardComponent,
    AjoutdemandeComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
