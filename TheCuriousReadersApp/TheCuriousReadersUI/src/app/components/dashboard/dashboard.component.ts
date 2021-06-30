import { Component, OnInit } from '@angular/core';
import { BookModel } from 'src/app/models/bookmodel';
import { AuthorModel } from 'src/app/models/authormodel';
import { NewBookModel } from 'src/app/models/newbookmodel';
import { paginationParameters } from 'src/app/constants/paginationparameters';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { AuthService } from 'src/app/services/auth.service';
import { ApprovedSubscriptionsResponse } from 'src/app/models/approvedsubscriptionsresponse';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  books: ApprovedSubscriptionsResponse[] = [];
  pageNumber: number = paginationParameters.pageNumber;
  pageSize: number = paginationParameters.pageSize;
  returnDate = new Date();
  totalItems: number = 0;
  userId: string = '';
  subscribedBooks?: ApprovedSubscriptionsResponse;
  id : number = 0
  constructor(
    private subscriptionService: SubscriptionService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getCurrentPageData(this.pageNumber);
  }

  getCurrentPageData(pageNumber: number) {
    this.userId = this.authService.getUserId();

    this.subscriptionService
      .getInformationForDashboard(this.userId, this.pageNumber, this.pageSize)
      .subscribe(myBooks => {
        this.books = myBooks;
        this.totalItems = myBooks.length;
      });

      //requestMoreDays()
     
  }

  dateToReturn(bookDate: string) {
    return new Date(bookDate).toLocaleString();
  }
  
  request(){
    
    
  }
}
