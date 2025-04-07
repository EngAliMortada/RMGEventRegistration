import { ConfigStateService } from '@abp/ng.core';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EventsService } from '@proxy/events';
import { EventCreationDto, EventDisplayDto, EventRegistrationDisplayDto, EventUpdateDto } from '@proxy/events/dtos';
import { Result } from '@proxy/results';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-events-form-component',
  templateUrl: './events-form-component.component.html',
  styleUrl: './events-form-component.component.scss',
  standalone: false
})
export class EventsFormComponentComponent {
  @ViewChild('endDateInput') endDateInput: ElementRef;
  
  public form: FormGroup;
  public isSaving = false;
  public minStartDate = new Date().toISOString().slice(0, 16); // Today's date as minimum
  public minEndDate = new Date().toISOString().slice(0, 16); // Today's date as minimum
  public eventId: string;
  public loadedEventModel: EventDisplayDto;
  public registeredUsers: Array<EventRegistrationDisplayDto> = [];

  private _registeredUsersLoaded: boolean;
  private _registeredUsersHidden: boolean = true;

  constructor(
    private _fb: FormBuilder,
    private _eventService: EventsService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _configState: ConfigStateService
  ) {}

  async ngOnInit() {
    this.buildForm();
    this.setEventId();
    await this.loadEventModelIfExist();
    this.updateFormIfMdoelLoaded();
    this.disableFormIfModelLoaded();
  }

  public get isAdmin(): boolean {
    let roles = this._configState.getDeep('currentUser.roles');
    return roles?.includes('admin');
  }

  public get router() {
    return this._router;
  }

  public get registeredUsersLoaded() {
    return this._registeredUsersLoaded;
  }

  public get registeredUsersVisible() {
    return this.loadedEventModel && 
            this.loadedEventModel.userCanViewRegisteredUsers &&
            !this._registeredUsersHidden;
  }

  private buildForm() {
    this.form = this._fb.group({
      name: ['', Validators.required],
      capacity: [0, [Validators.required, Validators.min(1)]],
      isOnline: [false],
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
      link: [''],
      location: ['', Validators.required],
      isActive: [true]
    });

    // Handle online/location logic
    this.form.get('isOnline').valueChanges.subscribe(isOnline => {
      const linkControl = this.form.get('link');
      const locationControl = this.form.get('location');
      const capacityControl = this.form.get('capacity');
      
      if (isOnline) {
        locationControl.clearValidators();
        capacityControl.clearValidators();
        linkControl.setValidators([Validators.required]);
      } else {
        linkControl.clearValidators();
        locationControl.setValidators([Validators.required]);
        capacityControl.setValidators([Validators.required]);
      }
      
      linkControl.updateValueAndValidity();
      locationControl.updateValueAndValidity();

      console.log(this.form.valid);
    });
  }

  public async submit() : Promise<void> {
    if (this.form.invalid) return;

    this.isSaving = true;
    let paylood = null;
    let actionTask: Observable<Result<EventDisplayDto>>  = null;
    const createPayload : EventCreationDto = {
      ...this.form.value
    };

    const editPayload : EventUpdateDto = {
      ...this.form.value,
      id: this.eventId
    };

    paylood = this.editContext()? editPayload : createPayload;
    actionTask = this.editContext() ? this._eventService.updateEvent(paylood)
                                          : this._eventService.createEventASyncByDto(paylood)

    return new Promise((resolve, reject) => {
      actionTask.subscribe({
        next: () => {
          this._router.navigate(['/events']);
        },
        error: (err) => {
          this.isSaving = false;
          console.error(err);
          reject(err);
        },
        complete: () => (this.isSaving = false, resolve())
      });
    })
  }

  public disableSave() {
    setTimeout(() => {
      return this.isSaving || this.form.invalid;
    }, 0);
  }

  private setEventId(){
    this.eventId = this._activatedRoute.snapshot.paramMap.get('id');
    console.log(this.eventId);
  }

  private disableForm() {
    if(this.form) {
      this.form.disable();
    }
  }

  private async loadEventModelIfExist(): Promise<void> {
    if(!this.eventId) return;

    return new Promise<void>((resolve, reject) => {
      this._eventService.getEventByIdByEventId(this.eventId).subscribe(respones => {
        if(respones.succeeded) {
          this.loadedEventModel = respones.value;
          resolve();
        }
        else {
          console.error(respones.errorCode);
          console.error(respones.errorMessage);
          reject(respones);
        }
      })
    });
  }

  private disableFormIfModelLoaded() {
    if(this.loadedEventModel && !this.loadedEventModel.userCanUpdateEvent) {
      this.disableForm();
    }
  }

  private updateFormIfMdoelLoaded() {
    if(!this.loadedEventModel) return;

    this.form.patchValue({
      name: this.loadedEventModel.name,
      capacity: this.loadedEventModel.capacity,
      isOnline: this.loadedEventModel.isOnline,
      startDate: this.loadedEventModel.startDate?.slice(0, 16),
      endDate: this.loadedEventModel.endDate?.slice(0, 16),
      link: this.loadedEventModel.link,
      location: this.loadedEventModel.location,
      isActive: this.loadedEventModel.isActive
    })
  }

  public handle(dateTime) {
    // this.form.get('endDate').reset();
    // this.minEndDate = dateTime.toISOString().slice(0, 16);
    // this.endDateInput.nativeElement.min = this.minEndDate;
  }

  public editContext() {
    return this.eventId;
  }

  public showSaveButton() {
    return this.isAdmin && !this.editContext();
  }

  public showUpdateButton() {
    return this.loadedEventModel &&
    this.loadedEventModel.userCanUpdateEvent;
  }

  public showCancelRegistrationButton() {
    return this.loadedEventModel &&
           this.loadedEventModel.userCanCancelEvent;
  }

  public showDeleteEventButton() {
    return this.loadedEventModel &&
            this.loadedEventModel.userCanDeleteEvent;
  }

  public showRegisterButton() {
    return this.loadedEventModel &&
    this.loadedEventModel.userCanRegisterInEvent;
  }
  
  public async performDelete(): Promise<void> {
    return new Promise<void>((resolve, rejects) => {
      this._eventService.deleteEvent({id: this.eventId}).subscribe(respone => {
        if(respone.succeeded) {
            this._router.navigate(['/events']);
            resolve();
        }
        else {
          console.error(respone.errorCode);
          console.error(respone.errorMessage);
          rejects();
        }
      });
    })
  }

  public async performRegister(): Promise<void> {
    return new Promise<void>((resolve, rejects) => {
      this._eventService.registerInEventByDto({id: this.eventId}).subscribe(respone => {
        if(respone.succeeded) {
            this._router.navigate(['/events']);
            resolve();
        }
        else {
          console.error(respone.errorCode);
          console.error(respone.errorMessage);
          rejects();
        }
      });
    })
  }

  public async cancelRegistration(): Promise<void> {
    return new Promise<void>((resolve, rejects) => {
      this._eventService.cancelRegistratonDtoByDto({eventId: this.eventId}).subscribe(respone => {
        if(respone.succeeded) {
            this._router.navigate(['/events']);
            resolve();
        }
        else {
          console.error(respone.errorCode);
          console.error(respone.errorMessage);
          rejects();
        }
      });
    })
  }

  public async getRegisteredUsers() {
    this.toggleRegisteredUsersVisibility();
    if(!this.registeredUsersVisible) return;
    if(this.registeredUsersLoaded) return;
    await this.getEventRegisteredUsers();
  }

  private async getEventRegisteredUsers(): Promise<void> {
    return new Promise<void>((resolve, rejects) => {
      this._eventService.getEventRegistrationsByEventId(this.eventId).subscribe(respone => {
        if(respone.succeeded) {
            this.registeredUsers = respone.values;
            this.setRegisteredUsersLoaded();
            resolve();
        }
        else {
          console.error(respone.errorCode);
          console.error(respone.errorMessage);
          rejects();
        }
      });
    })
  }

  private toggleRegisteredUsersVisibility() {
    this._registeredUsersHidden = !this._registeredUsersHidden;
  }



  private setRegisteredUsersLoaded() {
    this._registeredUsersLoaded = true;
  }

  public RegisterdUsersButtonVisible() {
    return this.loadedEventModel && 
    this.loadedEventModel.userCanViewRegisteredUsers;
  }
}
