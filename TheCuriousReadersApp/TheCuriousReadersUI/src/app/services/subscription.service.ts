import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from 'src/environments/environment';
import { ApproveSubscriptionModel } from "../models/approvesubscription.model";
import { BookSubscriptionModel } from "../models/book.subscription.model";
import { NotReviewedSubscriptionsModel } from "../models/notreviewed.subscriptions.model";
import { PaginatedDashboardInformation } from '../models/paginateddashboardinformation';
import { NewBookModel } from '../models/newbookmodel';
import { ApprovedSubscriptionsResponse } from '../models/approvedsubscriptionsresponse';

@Injectable({
    providedIn: 'root',
  })
  
export class SubscriptionService {
    
    constructor(private http: HttpClient) {}

    getTotalSubscribersForABook(bookId: number): Observable<BookSubscriptionModel>{
      return this.http.get<BookSubscriptionModel>(`${environment.baseUrl}/Subscriptions/books/${bookId}/total`);
    }

    getNonReviewedSubscriptions(pageNumber: number, pageSize: number): Observable<NotReviewedSubscriptionsModel[]>{
      return this.http.get<NotReviewedSubscriptionsModel[]>(`${environment.baseUrl}/Subscriptions/non-reviewed?PageNumber=${pageNumber}&PageSize=${pageSize}`);
    }

    approveSubscription(id: number, approveModel: ApproveSubscriptionModel): Observable<Response>{
      return this.http.put<Response>(`${environment.baseUrl}/Subscriptions/${id}`, approveModel, {headers:{'Content-Type':'application/json'}});
    }

    getInformationForDashboard(userId: string, pageNumber: number, pageSize: number): Observable<ApprovedSubscriptionsResponse[]>{
      return this.http.get<ApprovedSubscriptionsResponse[]>(`${environment.baseUrl}/Subscriptions/approved/${userId}?PageNumber=${pageNumber}&PageSize=${pageSize}`)
    }

    requestMoreDays(id: number, approveModel: ApproveSubscriptionModel): Observable<Response>{
      return this.http.put<Response>(`${environment.baseUrl}/Subscriptions/${id}`, approveModel, {headers:{'Content-Type':'application/json'}});
      //return this.http.put<Response>(`${environment.baseUrl}/${id}`, approveModel)
    }
}