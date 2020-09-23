import { ContactService } from './../services/contact.service';
import { Contact } from './../models/contact';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Pagination } from '../models/pagination';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})

export class ContactsComponent implements OnInit {
  contacts$: Observable<Contact[]>;
  pagination: Pagination;
  pageSize: number;
  orderForm: FormGroup;
  sortOrder: string;
  sortOrders = [
    {name: 'Sort by', value: ''},
    {name: 'By BirthDate Ascending', value: 'birthdate'},
    {name: 'By BirthDate Descending', value: 'birthdate desc'},
    {name: 'By Alphabetic Ascending', value: 'name'},
    {name: 'By Alphabetic Descending', value: 'name desc'},
  ];

  constructor(private contactService: ContactService,
              private formBuilder: FormBuilder, private router: Router) {
    this.pageSize = 10;
    this.sortOrder = '';

    this.orderForm = this.formBuilder.group(
    {
      sortOrder: new FormControl(this.sortOrders[0]),
    });
  }

  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts(pageNumber: number = 1): void {
    this.contactService.getList(pageNumber, this.pageSize, this.sortOrder)
      .subscribe(response =>
        {
          this.contacts$ = of(response.data);
          this.pagination = new Pagination(response.currentPage, response.totalPages,
            response.pageSize, response.totalCount, response.hasPrevious, response.hasNext);
        }
      );
  }

  changeOrder(): void {
    if (this.orderForm.value.sortOrder.value) {
      console.log(this.orderForm.value.sortOrder.value);
      this.sortOrder = this.orderForm.value.sortOrder.value;
      this.loadContacts(1);
    }
  }

  viewDetails(contactId: number): void {
    console.log(contactId);
    this.router.navigate(['/contact-details', contactId]);
  }

  createRange(): number[]{
    const items: number[] = [];

    for (let i = 1; i <= this.pagination.totalPages; i++){
       items.push(i);
    }

    return items;
  }
}
