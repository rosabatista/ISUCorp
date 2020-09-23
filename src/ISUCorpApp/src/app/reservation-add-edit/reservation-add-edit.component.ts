import { PlaceService } from './../services/place.service';
import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ContactService } from '../services/contact.service';
import { ReservationService } from '../services/reservation.service';
import { UtilService } from '../services/util.service';
import { Reservation } from './../models/reservation';
import { Contact } from '../models/contact';
import { Place } from '../models/place';

@Component({
  selector: 'app-reservation-add-edit',
  templateUrl: './reservation-add-edit.component.html',
  styleUrls: ['./reservation-add-edit.component.css']
})

export class ReservationAddEditComponent implements OnInit {
  reservationForm: FormGroup;
  formName: string;
  formType: string;
  formPhone: string;
  formBirthDate: string;
  formDate: string;
  formNotes: string;
  formPlace: string;
  errorMessage: any;
  reservationId: number;
  existingContact: Contact;
  existingPlace: Place;
  existingReservation: Reservation;
  contactTypeValues: string[];
  content = '<p>Some html</p>';
  config: any = {
    allowedContent: true,
    toolbar: [['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', 'Link', '-', 'CreatePlaceholder']],
    removePlugins: 'elementspath',
    resize_enabled: false,
    extraPlugins: 'font,divarea,placeholder',
    contentsCss: ['body {font-family: \'Helvetica Neue\', Helvetica, Arial, sans-serif;}'],
    autoParagraph: false,
    enterMode: 2
  };

  constructor(private reservationService: ReservationService, private contactService: ContactService,
              private placeService: PlaceService, private utilService: UtilService,
              private formBuilder: FormBuilder, private avRoute: ActivatedRoute,
              private router: Router, private datepipe: DatePipe) {
    const idParam = 'id';
    this.reservationId = 0;
    this.formName = 'name';
    this.formType = 'type';
    this.formPhone = 'phone';
    this.formBirthDate = 'birthDate';
    this.formDate = 'date';
    this.formNotes = 'notes';
    this.formPlace = 'place';

    this.getContactTypes();

    if (this.avRoute.snapshot.params[idParam]) {
      this.reservationId = this.avRoute.snapshot.params[idParam];
    }

    this.reservationForm = this.formBuilder.group(
    {
      Reservation: this.reservationId,
      name: ['', [Validators.required, Validators.maxLength(255)]],
      type: [null, [Validators.required]],
      phone: ['', [Validators.pattern('^((\\+91-?)|0)?[0-9]{10}$')]],
      birthDate: [null, [Validators.required]],
      date: [null, [Validators.required]],
      place: [null, [Validators.required, Validators.maxLength(255)]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    if (this.reservationId > 0) {
      this.reservationService.get(this.reservationId)
        .subscribe(response => {
          this.existingReservation = response.data;
          const index = this.existingReservation.contact.type as unknown as number;

          this.reservationForm.patchValue({
            name: this.existingReservation.contact.name,
            type: this.contactTypeValues[index - 1],
            phone: this.existingReservation.contact.phone,
            birthDate: this.datepipe.transform(
              this.existingReservation.contact.birthDate, 'yyyy-MM-dd'),
            date: this.datepipe.transform(
              this.existingReservation.date, 'yyyy-MM-dd'),
            place: this.existingReservation.place.name,
            notes: this.existingReservation.notes,
          });
        });
    }
  }

  save(): void {
    if (!this.reservationForm.valid) {
      return;
    }

    const contact: Contact = {
      id: this.existingContact
        ? this.existingContact.id
        : (this.existingReservation
          ? this.existingReservation.contactId
          : null),
      name: this.reservationForm.get(this.formName).value,
      type: this.reservationForm.get(this.formType).value,
      phone: this.reservationForm.get(this.formPhone).value,
      birthDate: this.reservationForm.get(this.formBirthDate).value,
      addedAt: new Date(),
      modifiedAt: null,
      reservations: null
    };

    const place: Place = {
      id: this.existingPlace
        ? this.existingPlace.id
        : (this.existingReservation
          ? this.existingReservation.placeId
          : null),
      name: this.reservationForm.get(this.formName).value,
      addedAt: new Date(),
      modifiedAt: null,
      reservations: null
    };

    const reservation: Reservation = {
      date: this.reservationForm.get(this.formDate).value,
      notes: this.reservationForm.get(this.formNotes).value,
      rating: null,
      favorite: false,
      contactId: contact.id,
      contact,
      placeId: place.id,
      place,
      addedAt: new Date(),
      modifiedAt: null,
    };

    if (this.reservationId === 0) {
      this.reservationService.save(reservation)
        .subscribe((response) => {
          this.router.navigate(['/reservation-edit', response.data.id]);
        });
    }

    if (this.reservationId > 0) {
      reservation.id = this.reservationId;

      this.reservationService.update(reservation.id, reservation)
        .subscribe((data) => {
          this.router.navigate([this.router.url]);
      });
    }
  }

  searchContact(): void {
    if (!this.reservationForm.controls[this.formName].valid) {
      this.existingContact = null;
      return;
    }

    this.contactService.search(this.reservationForm.get(this.formName).value)
      .subscribe((response) => {
        console.log(response);
        this.existingContact = response.data;

        if (response.data) {
          const index = this.existingContact.type as unknown as number;

          this.reservationForm.patchValue({
              name: this.existingContact.name,
              type: this.contactTypeValues[index - 1],
              phone: this.existingContact.phone,
              birthDate: this.datepipe.transform(this.existingContact.birthDate, 'yyyy-MM-dd')
            });
        }
    });
  }

  searchPlace(): void {
    if (!this.reservationForm.controls[this.formPlace].valid) {
      this.existingPlace = null;
      return;
    }

    this.placeService.search(this.reservationForm.get(this.formPlace).value)
      .subscribe((response) => {
        console.log(response);
        this.existingPlace = response.data;

        if (response.data) {
          console.log(response.data);
          this.reservationForm.patchValue({
              place: this.existingPlace.name
          });
        } else {
          this.reservationForm.patchValue({
            place: this.existingReservation && this.existingReservation.place
              ? this.existingReservation.place.name
              : ''
          });
        }
    });
  }

  getContactTypes(): void {
    this.utilService.getContactTypes()
      .subscribe((response) => {
        this.contactTypeValues = [];

        response.data.forEach(element => {
          this.contactTypeValues.push(element.name);
        });
    });
  }

  cancel(): void {
    this.router.navigate(['/']);
  }
}
