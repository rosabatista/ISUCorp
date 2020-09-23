import { PlaceService } from './../services/place.service';
import { Place } from './../models/place';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Pagination } from '../models/pagination';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css']
})
export class PlacesComponent implements OnInit {
  places$: Observable<Place[]>;
  pagination: Pagination;
  pageSize: number;
  orderForm: FormGroup;
  sortOrder: string;
  sortOrders = [
    {name: 'Sort by', value: ''},
    {name: 'By Alphabetic Ascending', value: 'name'},
    {name: 'By Alphabetic Descending', value: 'name desc'},
  ];

  constructor(private placeService: PlaceService,
              private formBuilder: FormBuilder, private router: Router) {
    this.pageSize = 10;
    this.sortOrder = '';

    this.orderForm = this.formBuilder.group(
    {
      sortOrder: new FormControl(this.sortOrders[0]),
    });
  }

  ngOnInit(): void {
    this.loadPlaces();
  }

  loadPlaces(pageNumber: number = 1): void {
    this.placeService.getList(pageNumber, this.pageSize, this.sortOrder)
      .subscribe(response =>
        {
          console.log(response);
          this.places$ = of(response.data);
          this.pagination = new Pagination(response.currentPage, response.totalPages,
            response.pageSize, response.totalCount, response.hasPrevious, response.hasNext);
        }
      );
  }

  changeOrder(): void {
    if (this.orderForm.value.sortOrder.value) {
      console.log(this.orderForm.value.sortOrder.value);
      this.sortOrder = this.orderForm.value.sortOrder.value;
      this.loadPlaces(1);
    }
  }

  viewDetails(placeId: number): void {
    console.log(placeId);
    this.router.navigate(['/place-details', placeId]);
  }

  createRange(): number[]{
    const items: number[] = [];

    for (let i = 1; i <= this.pagination.totalPages; i++){
       items.push(i);
    }

    return items;
  }
}
