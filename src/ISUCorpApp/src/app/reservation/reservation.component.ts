import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { ReservationService } from './../services/reservation.service';
import { Reservation } from './../models/reservation';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-reservation',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.css']
})

export class ReservationComponent implements OnInit {

  reservationId: number;
  reservation$: Observable<Reservation>;
  reservationForm: FormGroup;
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

  constructor(private reservationService: ReservationService, private avRoute: ActivatedRoute,
              private router: Router, private formBuilder: FormBuilder,
              private datepipe: DatePipe) {
    const idParam = 'id';

    if (this.avRoute.snapshot.params[idParam]) {
      this.reservationId = this.avRoute.snapshot.params[idParam];
    }

    this.reservationForm = this.formBuilder.group(
      {
        Reservation: this.reservationId,
        name: [''],
        type: [''],
        phone: [''],
        birthDate: [null],
        date: [null],
        place: [''],
        notes: ['']
      });
  }

  ngOnInit(): void {
    this.loadReservation();
  }

  loadReservation(): void {
    this.reservationService.get(this.reservationId)
      .subscribe(response =>
        {
          this.reservation$ = of(response.data);

          if (response.data != null) {
            this.reservationForm = this.formBuilder.group(
              {
                reservationId: this.reservationId,
                name: [response.data.contact.name],
                type: [response.data.contact.type],
                phone: [response.data.contact.phone],
                birthDate: [this.datepipe.transform(response.data.date, 'mediumDate')],
                date: [this.datepipe.transform(response.data.date, 'medium')],
                place: [response.data.place.name],
                notes: [response.data.notes]
              });
          }
        }
      );
  }

  delete(): void {
    const ans = confirm('Do you want to delete reservation with id: ' + this.reservationId);

    if (ans) {
      this.reservationService.delete(this.reservationId).subscribe(() => {
        this.router.navigate(['/', this.reservationId]);
      });
    }
  }
}
