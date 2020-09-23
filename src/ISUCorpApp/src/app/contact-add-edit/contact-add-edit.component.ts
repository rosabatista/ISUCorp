import { UtilService } from './../services/util.service';
import { ContactType } from './../models/contact.type';
import { Observable, of } from 'rxjs';
import { ContactService } from './../services/contact.service';
import { Contact } from './../models/contact';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, AbstractControl, FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-contact-add-edit',
  templateUrl: './contact-add-edit.component.html',
  styleUrls: ['./contact-add-edit.component.css']
})

export class ContactAddEditComponent implements OnInit {
  contactForm: FormGroup;
  formName: string;
  formType: string;
  formPhone: string;
  formBirthDate: string;
  errorMessage: any;
  contactId: number;
  existingContact: Contact;
  contactTypeValues: string[];

  constructor(private contactService: ContactService, private utilService: UtilService,
              private formBuilder: FormBuilder, private avRoute: ActivatedRoute,
              private router: Router, private datepipe: DatePipe) {
    const idParam = 'id';
    this.contactId = 0;
    this.formName = 'name';
    this.formType = 'type';
    this.formPhone = 'phone';
    this.formBirthDate = 'birthDate';

    this.getContactTypes();

    if (this.avRoute.snapshot.params[idParam]) {
      this.contactId = this.avRoute.snapshot.params[idParam];
    }

    this.contactForm = this.formBuilder.group(
    {
      contactId: this.contactId,
      name: ['', [Validators.required, Validators.maxLength(255)]],
      type: [null, [Validators.required]],
      phone: ['', [Validators.pattern('^((\\+91-?)|0)?[0-9]{10}$')]],
      birthDate: [null, [Validators.required]]
    });
  }

  ngOnInit(): void {
    if (this.contactId > 0) {
      this.contactService.get(this.contactId)
        .subscribe(response => {
          this.existingContact = response.data;
          const index = this.existingContact.type as unknown as number;

          this.contactForm.patchValue({
            name: this.existingContact.name,
            type: this.contactTypeValues[index - 1],
            phone: this.existingContact.phone,
            birthDate: this.datepipe.transform(this.existingContact.birthDate, 'yyyy-MM-dd')
          });
        });
    }
  }

  save(): void {
    if (!this.contactForm.valid) {
      return;
    }

    const contact: Contact = {
      id: this.contactId > 0 ? this.contactId : null,
      name: this.contactForm.get(this.formName).value,
      type: this.contactForm.get(this.formType).value,
      phone: this.contactForm.get(this.formPhone).value,
      birthDate: this.contactForm.get(this.formBirthDate).value,
      addedAt: new Date(),
      modifiedAt: null,
      reservations: null
    };

    if (this.contactId === 0) {
      this.contactService.save(contact)
        .subscribe((response) => {
          this.router.navigate(['/contact-edit', response.data.id]);
        });
    }

    if (this.contactId > 0) {
      this.contactService.update(contact.id, contact)
        .subscribe((data) => {
          this.router.navigate([this.router.url]);
      });
    }
  }

  search(): void {
    if (!this.contactForm.controls[this.formName].valid) {
      this.existingContact = null;
      return;
    }

    this.contactService.search(this.contactForm.get(this.formName).value)
      .subscribe((response) => {
        console.log(response);
        this.existingContact = response.data;

        if (response.data) {
          const index = this.existingContact.type as unknown as number;

          this.contactForm.patchValue({
              name: this.existingContact.name,
              type: this.contactTypeValues[index - 1],
              phone: this.existingContact.phone,
              birthDate: this.datepipe.transform(this.existingContact.birthDate, 'yyyy-MM-dd')
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
    this.router.navigate(['/contacts']);
  }
}
