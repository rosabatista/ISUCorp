import { Place } from './../models/place';
import { PlaceService } from './../services/place.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Contact } from '../models/contact';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilService } from '../services/util.service';

@Component({
  selector: 'app-place-add-edit',
  templateUrl: './place-add-edit.component.html',
  styleUrls: ['./place-add-edit.component.css']
})
export class PlaceAddEditComponent implements OnInit {
  placeForm: FormGroup;
  formName: string;
  formDescription: string;
  errorMessage: any;
  placeId: number;
  existingPlace: Place;
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

  constructor(private placeService: PlaceService, private formBuilder: FormBuilder,
              private avRoute: ActivatedRoute, private router: Router) {
    const idParam = 'id';
    this.placeId = 0;
    this.formName = 'name';
    this.formDescription = 'description';

    if (this.avRoute.snapshot.params[idParam]) {
      this.placeId = this.avRoute.snapshot.params[idParam];
    }

    this.placeForm = this.formBuilder.group(
    {
      placeId: this.placeId,
      name: ['', [Validators.required, Validators.maxLength(255)]],
      description: ['']
    });
  }

  ngOnInit(): void {
    if (this.placeId > 0) {
      this.placeService.get(this.placeId)
        .subscribe(response => {
          this.existingPlace = response.data;
          console.log(this.existingPlace);

          this.placeForm.patchValue({
            name: this.existingPlace.name,
            description: this.existingPlace.description
          });
        });
    }
  }

  save(): void {
    if (!this.placeForm.valid) {
      return;
    }

    const place: Place = {
      id: this.placeId > 0 ? this.placeId : null,
      name: this.placeForm.get(this.formName).value,
      description: this.placeForm.get(this.formDescription).value,
      addedAt: new Date(),
      modifiedAt: null,
      reservations: null
    };

    if (this.placeId === 0) {
      this.placeService.save(place)
        .subscribe((response) => {
          this.router.navigate(['/place-edit', response.data.id]);
        });
    }

    if (this.placeId > 0) {
      this.placeService.update(place.id, place)
        .subscribe((data) => {
          this.router.navigate([this.router.url]);
      });
    }
  }

  search(): void {
    if (!this.placeForm.controls[this.formName].valid) {
      this.existingPlace = null;
      return;
    }

    this.placeService.search(this.placeForm.get(this.formName).value)
      .subscribe((response) => {
        console.log(response);
        this.existingPlace = response.data;

        if (response.data) {

          this.placeForm.patchValue({
              name: this.existingPlace.name,
              description: this.existingPlace.description,
            });
        }
    });
  }

  cancel(): void {
    this.router.navigate(['/places']);
  }

}
