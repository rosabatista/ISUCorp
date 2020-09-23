import { PagedResponse } from '../models/paged.response';
import { DataResponse } from '../models/data.response';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Contact } from '../models/contact';

@Injectable({
  providedIn: 'root'
})

export class ContactService {

  myAppUrl: string;
  myApiUrl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) {
      this.myAppUrl = environment.appUrl;
      this.myApiUrl = 'api/contacts/';
  }

  getList(pageNumber: number, pageSize: number, sortOrder: string = '', searchBy: string = '')
    : Observable<PagedResponse<Contact>> {
    // set request params
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('sortOrder', sortOrder);
    params = params.append('searchBy', searchBy);

    return this.http.get<PagedResponse<Contact>>(this.myAppUrl + this.myApiUrl, { params })
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  get(contactId: number): Observable<DataResponse<Contact>> {
      return this.http.get<DataResponse<Contact>>(this.myAppUrl + this.myApiUrl + contactId)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  search(name: string): Observable<DataResponse<Contact>> {
    return this.http.get<DataResponse<Contact>>(
      this.myAppUrl + this.myApiUrl + 'find_by_name/' + name)
    .pipe(
      retry(1),
      catchError(this.errorHandler)
    );
  }

  save(contact: Contact): Observable<DataResponse<Contact>> {
      console.log(JSON.stringify(contact));
      return this.http.post<DataResponse<Contact>>(this.myAppUrl + this.myApiUrl,
        JSON.stringify(contact), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  update(contactId: number, contact: Contact): Observable<Contact> {
      console.log(contactId, contact);
      return this.http.put<Contact>(this.myAppUrl + this.myApiUrl + contactId,
        JSON.stringify(contact), this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandler)
      );
  }

  delete(contactId: number): Observable<Contact> {
      return this.http.delete<Contact>(this.myAppUrl + this.myApiUrl + contactId)
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
