import { PlaceService } from './services/place.service';
import { UtilService } from './services/util.service';
import { ContactService } from './services/contact.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { CKEditorModule } from 'ng2-ckeditor';
import {NgbRatingModule} from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { ReservationComponent } from './reservation/reservation.component';
import { ReservationAddEditComponent } from './reservation-add-edit/reservation-add-edit.component';
import { ReservationService } from './services/reservation.service';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { NavigationComponent } from './navigation/navigation.component';
import { ContactAddEditComponent } from './contact-add-edit/contact-add-edit.component';
import { ContactComponent } from './contact/contact.component';
import { ContactsComponent } from './contacts/contacts.component';
import { PlacesComponent } from './places/places.component';
import { PlaceComponent } from './place/place.component';
import { PlaceAddEditComponent } from './place-add-edit/place-add-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    ReservationsComponent,
    ReservationComponent,
    ReservationAddEditComponent,
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    NavigationComponent,
    ContactAddEditComponent,
    ContactComponent,
    ContactsComponent,
    PlacesComponent,
    PlaceComponent,
    PlaceAddEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    CKEditorModule,
    NgbRatingModule
  ],
  providers: [
    DatePipe,
    ContactService,
    PlaceService,
    ReservationService,
    UtilService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
