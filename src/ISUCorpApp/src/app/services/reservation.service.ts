import { BaseResponse } from './../models/base.response';
import { PagedResponse } from './../models/paged.response';
import { DataResponse } from '../models/data.response';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { retry, catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Reservation } from '../models/reservation';

@Injectable({
  providedIn: 'root'
})

export class ReservationService {

  myAppUrl: string;
  myApiUrl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) {
      this.myAppUrl = environment.appUrl;
      this.myApiUrl = 'api/reservations/';
  }

  getList(pageNumber: number, pageSize: number, sortOrder: string = '', searchBy: string = '')
    : Observable<PagedResponse<Reservation>> {
    // set request params
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('sortOrder', sortOrder);
    params = params.append('searchBy', searchBy);

    return this.http.get<PagedResponse<Reservation>>(this.myAppUrl + this.myApiUrl, { params })
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  get(reservationId: number): Observable<DataResponse<Reservation>> {
      return this.http.get<DataResponse<Reservation>>(this.myAppUrl + this.myApiUrl + reservationId)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  save(reservation: Reservation): Observable<DataResponse<Reservation>> {
      console.log(reservation);
      return this.http.post<DataResponse<Reservation>>(this.myAppUrl + this.myApiUrl,
        JSON.stringify(reservation), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  rate(id: number, rating: number): Observable<BaseResponse> {
    const item = {id, rating};
    console.log(item);

    return this.http.post<DataResponse<Reservation>>(this.myAppUrl + this.myApiUrl,
      JSON.stringify(item), this.httpOptions)
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  setFavorite(id: number): Observable<BaseResponse> {
    return this.http.post<DataResponse<Reservation>>(this.myAppUrl + this.myApiUrl + 
      'set_favorite/' + id, this.httpOptions)
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  update(reservationId: number, reservation: Reservation): Observable<DataResponse<Reservation>> {
      return this.http.put<DataResponse<Reservation>>(this.myAppUrl + this.myApiUrl + reservationId,
        JSON.stringify(reservation), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  delete(reservationId: number): Observable<Reservation> {
      return this.http.delete<Reservation>(this.myAppUrl + this.myApiUrl + reservationId)
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
