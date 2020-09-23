import { Place } from './../models/place';
import { PlaceService } from './../services/place.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-place',
  templateUrl: './place.component.html',
  styleUrls: ['./place.component.css']
})
export class PlaceComponent implements OnInit {
  placeId: number;
  place$: Observable<Place>;
  placeForm: FormGroup;
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
              private router: Router, private avRoute: ActivatedRoute) {
      const idParam = 'id';

      if (this.avRoute.snapshot.params[idParam]) {
        this.placeId = this.avRoute.snapshot.params[idParam];
      }

      this.placeForm = this.formBuilder.group(
      {
        placeId: this.placeId,
        name: [''],
        description: ['']
      });
  }

  ngOnInit(): void {
    this.loadPlace();
  }

  loadPlace(): void {
    this.placeService.get(this.placeId)
      .subscribe(response =>
        {
          this.place$ = of(response.data);

          if (response.data != null) {
            this.placeForm = this.formBuilder.group(
            {
              placeId: this.placeId,
              name: [response.data.name],
              description: [response.data.description]
            });
          }
        }
      );
  }

  delete(): void {
    const ans = confirm('Do you want to delete a place with id: ' + this.placeId);

    if (ans) {
      this.placeService.delete(this.placeId).subscribe(() => {
        this.router.navigate(['/places']);
      });
    }
  }
}
