import { Router, ActivatedRoute } from '@angular/router';
import { UtilService } from './../services/util.service';
import { Observable, of } from 'rxjs';
import { ContactService } from './../services/contact.service';
import { Contact } from './../models/contact';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})

export class ContactComponent implements OnInit {
  contactForm: FormGroup;
  contactId: number;
  contact$: Observable<Contact>;

  constructor(private contactService: ContactService, private utilService: UtilService,
              private formBuilder: FormBuilder, private datepipe: DatePipe,
              private router: Router, private avRoute: ActivatedRoute) {
    const idParam = 'id';

    if (this.avRoute.snapshot.params[idParam]) {
      this.contactId = this.avRoute.snapshot.params[idParam];
    }

    this.contactForm = this.formBuilder.group(
    {
      contactId: this.contactId,
      name: [''],
      type: [''],
      phone: [''],
      birthDate: [null]
    });
  }

  ngOnInit(): void {
    this.loadContact();
  }

  loadContact(): void {
    this.contactService.get(this.contactId)
      .subscribe(response =>
        {
          this.contact$ = of(response.data);

          if (response.data != null) {
            this.contactForm = this.formBuilder.group(
            {
              contactId: this.contactId,
              name: [response.data.name],
              type: [response.data.type],
              phone: [response.data.phone],
              birthDate: [this.datepipe.transform(response.data.birthDate, 'mediumDate')]
            });
          }
        }
      );
  }

  delete(): void {
    const ans = confirm('Do you want to delete a contact with id: ' + this.contactId);

    if (ans) {
      this.contactService.delete(this.contactId).subscribe(() => {
        this.router.navigate(['/contacts']);
      });
    }
  }

}
