import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ReservationService } from '../services/reservation.service';
import { Reservation } from '../models/reservation';
import { Pagination } from '../models/pagination';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})

export class ReservationsComponent implements OnInit {

  reservations$: Observable<Reservation[]>;
  pagination: Pagination;
  pageSize: number;
  orderForm: FormGroup;
  sortOrder: string;
  currentPage: number;
  sortOrders = [
    {name: 'Sort by', value: ''},
    {name: 'By Date Ascending', value: 'date'},
    {name: 'By Date Descending', value: 'date desc'},
    {name: 'By Alphabetic Ascending', value: 'name'},
    {name: 'By Alphabetic Descending', value: 'name desc'},
  ];

  constructor(private reservationService: ReservationService,
              private formBuilder: FormBuilder, private router: Router) {
    this.pageSize = 3;
    this.sortOrder = '';

    this.orderForm = this.formBuilder.group(
      {
        sortOrder: new FormControl(this.sortOrders[0]),
      });
  }

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations(pageNumber: number = 1): void {
    this.currentPage = pageNumber;
    this.reservationService.getList(pageNumber, this.pageSize, this.sortOrder)
      .subscribe(response =>
        {
          this.reservations$ = of(response.data);
          this.pagination = new Pagination(response.currentPage, response.totalPages,
            response.pageSize, response.totalCount, response.hasPrevious, response.hasNext);
        }
      );
  }

  changeOrder(): void {
    if (this.orderForm.value.sortOrder.value) {
      this.sortOrder = this.orderForm.value.sortOrder.value;
      this.loadReservations(1);
    }
  }

  setFavorite(reservationId: number): void {
    if (reservationId) {
      this.reservationService.setFavorite(reservationId)
      .subscribe(response => {
          if (response.success) {
            this.loadReservations(this.currentPage);
          }
          console.log(of(response));
        }
      );
    }
  }

  viewDetails(reservationId: number): void {
    console.log(reservationId);
    this.router.navigate(['/reservation-details', reservationId]);
  }

  createRange(): number[]{
    const items: number[] = [];

    for (let i = 1; i <= this.pagination.totalPages; i++){
       items.push(i);
    }

    return items;
  }
}
