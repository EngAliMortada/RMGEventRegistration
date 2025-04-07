import { RouterModule, Routes } from "@angular/router";
import { NgModule } from "@angular/core";
import { EventsGridComponentComponent } from "./components/events-grid-component/events-grid-component.component";
import { EventsFormComponentComponent } from "./components/events-form-component/events-form-component.component";

const routes: Routes = [
  {
    path: '', 
    component: EventsGridComponentComponent
  },
  {
    path: 'event-form', 
    component: EventsFormComponentComponent 
  },
  {
    path: 'event-form/:id', 
    component: EventsFormComponentComponent 
  }
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EventsRoutingModule {}