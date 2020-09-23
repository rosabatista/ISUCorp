import { PlacesComponent } from './places/places.component';
import { ContactsComponent } from './contacts/contacts.component';
import { ContactComponent } from './contact/contact.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { ReservationComponent } from './reservation/reservation.component';
import { ReservationAddEditComponent } from './reservation-add-edit/reservation-add-edit.component';
import { ContactAddEditComponent } from './contact-add-edit/contact-add-edit.component';
import { PlaceAddEditComponent } from './place-add-edit/place-add-edit.component';
import { PlaceComponent } from './place/place.component';

const routes: Routes = [
  { path: '', component: ReservationsComponent, pathMatch: 'full' },
  { path: 'home', component: HomeComponent },

  { path: 'reservations', component: ReservationsComponent },
  { path: 'reservations/:id', component: ReservationComponent },
  { path: 'reservation-add', component: ReservationAddEditComponent },
  { path: 'reservation-edit/:id', component: ReservationAddEditComponent },
  { path: 'reservation-details/:id', component: ReservationComponent },

  { path: 'contacts', component: ContactsComponent },
  { path: 'contact-add', component: ContactAddEditComponent },
  { path: 'contact-edit/:id', component: ContactAddEditComponent },
  { path: 'contact-details/:id', component: ContactComponent },

  { path: 'places', component: PlacesComponent },
  { path: 'place-add', component: PlaceAddEditComponent },
  { path: 'place-edit/:id', component: PlaceAddEditComponent },
  { path: 'place-details/:id', component: PlaceComponent },

  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
