import { ConfigStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { EventInfoWrapper } from '@angular/core/primitives/event-dispatch';
import { Router } from '@angular/router';
import { EventsService } from '@proxy/events';
import { EventDisplayDto } from '@proxy/events/dtos';

@Component({
  selector: 'app-events-grid-component',
  templateUrl: './events-grid-component.component.html',
  styleUrl: './events-grid-component.component.scss',
  standalone: false
})
export class EventsGridComponentComponent implements OnInit {
  displayedEvents: Array<EventDisplayDto> = [];

  public get eventsEmpty (): boolean {
    return !this.displayedEvents || this.displayedEvents.length == 0;
  }

  constructor(private _eventsService: EventsService, private _router: Router, private _configState: ConfigStateService) {

  }

  async ngOnInit() {
    await this.showActiveEvents();
  }

  public get isAdmin(): boolean {
    let roles = this._configState.getDeep('currentUser.roles');
    return roles?.includes('admin');
  }

  public showActiveEvents(): Promise<void>  {
    return this.getActiveEvents();
  }

  public showMyEvents(): Promise<void>  {
    return this.getMyEvents();
  }

  public openEvent(event: EventDisplayDto) {
    this._router.navigate(['events', 'event-form', event.id]);
  }
 
  public AddNewEvent() {
    this._router.navigate(['events', 'event-form']);
  }

  public showAddNewButton() {
    return this.isAdmin;
  }

  //I return promise so I can await instead of subscribe directly in ngOnInit
  private getActiveEvents(): Promise<void> {
    return new Promise<void>(resolve => {
      this._eventsService.getAllActiveEvents().subscribe(response => {
        if(response.succeeded) {
          this.displayedEvents = response.values;
        }
        else {
          //handle errors
        }
        resolve();
      })
    })
  }

  private getMyEvents(): Promise<void> {
    return new Promise<void>(resolve => {
      this._eventsService.getUserEvents().subscribe(response => {
        if(response.succeeded) {
          this.displayedEvents = response.values;
        }
        else {
          //handle errors
        }
        resolve();
      })
    })
  }
  


}
