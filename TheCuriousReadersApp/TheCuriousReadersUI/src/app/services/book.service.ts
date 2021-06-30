import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Errors } from '../enums/errors';
import { BookData } from '../models/book-data';
import { BookModel } from '../models/bookmodel';
import { PaginatedBookModel } from '../models/paginatedbookmodel';
import { PaginatedNewBookModel } from '../models/paginatednewbookmodel';
import { PaginatedNewCommentModel } from '../models/paginatednewcommentsmodel';
import { forkJoin, Observable, of } from 'rxjs';
import { GenreBooksCountModel } from 'src/app/models/genrebookscountmodel';
import { BookResponseModel } from '../models/book.responsemodel';
import { CommentResponseModel } from '../models/commentresponsemodel';

@Injectable({
  providedIn: 'root',
})

export class BookService {
  bookResponse = new BookResponseModel(0,"", "");
  errorMsg: string = '';

    constructor(private http: HttpClient) {}

  public addBook(data: any): Observable<BookResponseModel> {
    return this.http.post<BookResponseModel>(`${environment.baseUrl}/Books`, 
    data, 
    {headers:{'Content-Type':'application/json'}});
  }

  public addCover(cover: File, bookId: number): Observable<BookResponseModel> {
    let formData = new FormData();
    formData.append("cover", cover)

    return this.http.post<BookResponseModel>(`${environment.baseUrl}/Books/${bookId}/upload-cover`, formData, {headers:{'Accept':'application/json'}});
  }

  public getBookData(bookId : string ) : Observable<BookData>{
    return this.http.get<BookData>(`${environment.baseUrl}/Books/${bookId}`);
  }

  public subscribeToBook(data: {BookId : string, Copies: number}) : Observable<BookResponseModel>{
    return this.http.post<BookResponseModel>(`${environment.baseUrl}/Subscriptions`, data, {headers:{'Content-Type':'application/json'}});
  }

  getNewBooks(pageNumber: number, pageSize: number): Observable<PaginatedNewBookModel>{
    return this.http.get<PaginatedNewBookModel>(`${environment.baseUrl}/Books/new?PageNumber=${pageNumber}&PageSize=${pageSize}`);
  }

  getGenres(): Observable<GenreBooksCountModel[]>{
    return this.http.get<GenreBooksCountModel[]>(`${environment.baseUrl}/Books/genres`);
  }

  addReview(data: { rating: number; commentBody: string; }, bookId: string) : Observable<CommentResponseModel>{
    return this.http.post<CommentResponseModel>(`${environment.baseUrl}/Books/${bookId}/comments`, data,
    {headers:{'Content-Type':'application/json'}})
  }

  getComments(pageNumber: number, pageSize: number, bookId :string): Observable<PaginatedNewCommentModel>{
    return this.http.get<PaginatedNewCommentModel>(`${environment.baseUrl}/Books/${bookId}/comments?PageNumber=${pageNumber}&PageSize=${pageSize}`);
  }

  deleteBook(bookId :string): Observable<PaginatedNewCommentModel>{
    return this.http.delete<PaginatedNewCommentModel>(`${environment.baseUrl}/Books/${bookId}`);
  }

  changeBookAvailability(id: number, isAvailable: boolean): Observable<Response>{
    return this.http.patch<Response>(`${environment.baseUrl}/Books/${id}`, 
    [{op:"replace", path:"/isAvailable", value: isAvailable}], 
    {headers:{'Content-Type':'application/json-patch+json'}});
  }

}