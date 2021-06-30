import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SearchModel } from '../models/searchmodel';
import { SearchResponseModel } from '../models/searchResponseModel';
import { PaginatedNewSearchResultsModel } from '../models/paginatednewsearchresultsmodel';

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  constructor(private http: HttpClient) {}

  search(searchModel: SearchModel, pageNumber: number, pageSize: number) : Observable<PaginatedNewSearchResultsModel>{    
    if (searchModel.parameter === "Book title") {
      return this.http.get<PaginatedNewSearchResultsModel>(
        `${environment.baseUrl}/Books/search?PageNumber=${pageNumber}&PageSize=${pageSize}&BookTitle=${searchModel.text}`,
        { headers: { 'Content-Type': 'application/json' } }
      );
    } else if (searchModel.parameter === "Book author") {
      return this.http.get<PaginatedNewSearchResultsModel>(
        `${environment.baseUrl}/Books/search?PageNumber=${pageNumber}&PageSize=${pageSize}&AuthorName=${searchModel.text}`,
        { headers: { 'Content-Type': 'application/json' } }
      );
    } else if (searchModel.parameter === "Description keywords") {
      return this.http.get<PaginatedNewSearchResultsModel>(
        `${environment.baseUrl}/Books/search?PageNumber=${pageNumber}&PageSize=${pageSize}&BookDescription=${searchModel.text}`,
        { headers: { 'Content-Type': 'application/json' } }
      );
    } else {
      return this.http.get<PaginatedNewSearchResultsModel>(
        `${environment.baseUrl}/Books/search?PageNumber=${pageNumber}&PageSize=${pageSize}&CommentBody=${searchModel.text}`,
        { headers: { 'Content-Type': 'application/json' } }
      );
    }
  }
}
