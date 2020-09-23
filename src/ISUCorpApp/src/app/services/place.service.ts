import { Place } from './../models/place';
import { PagedResponse } from '../models/paged.response';
import { DataResponse } from '../models/data.response';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class PlaceService {

  myAppUrl: string;
  myApiUrl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) {
      this.myAppUrl = environment.appUrl;
      this.myApiUrl = 'api/places/';
  }

  getList(pageNumber: number, pageSize: number, sortOrder: string = '', searchBy: string = '')
    : Observable<PagedResponse<Place>> {
    // set request params
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('sortOrder', sortOrder);
    params = params.append('searchBy', searchBy);

    return this.http.get<PagedResponse<Place>>(this.myAppUrl + this.myApiUrl, { params })
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  get(placeId: number): Observable<DataResponse<Place>> {
      return this.http.get<DataResponse<Place>>(this.myAppUrl + this.myApiUrl + placeId)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  search(name: string): Observable<DataResponse<Place>> {
    return this.http.get<DataResponse<Place>>(
      this.myAppUrl + this.myApiUrl + 'find_by_name/' + name)
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  save(place: Place): Observable<DataResponse<Place>> {
      console.log(JSON.stringify(place));
      return this.http.post<DataResponse<Place>>(this.myAppUrl + this.myApiUrl,
        JSON.stringify(place), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  update(placeId: number, place: Place): Observable<Place> {
      console.log(placeId, place);
      return this.http.put<Place>(this.myAppUrl + this.myApiUrl + placeId,
        JSON.stringify(place), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  delete(placeId: number): Observable<Place> {
      return this.http.delete<Place>(this.myAppUrl + this.myApiUrl + placeId)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  errorHandler(error): Observable<never> {
    console.log(error);
    let errorMessage = '';

    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
