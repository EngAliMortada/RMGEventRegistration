import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { EventsRoutingModule } from './events-routing.module';
import { EventsGridComponentComponent } from './components/events-grid-component/events-grid-component.component';
import { EventsFormComponentComponent } from './components/events-form-component/events-form-component.component';
import { DateAdapter, ThemeSharedModule } from '@abp/ng.theme.shared';


@NgModule({
  declarations: [EventsGridComponentComponent, EventsFormComponentComponent],
  imports: [
    CommonModule,
    SharedModule,
    EventsRoutingModule,
    ThemeSharedModule.forRoot()
  ],
  providers: [DateAdapter]
})

export class EventsModule { }
