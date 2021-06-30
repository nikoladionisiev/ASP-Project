import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbRatingConfig } from '@ng-bootstrap/ng-bootstrap';
import { paginationComments } from 'src/app/constants/paginationcomments';
import { routePaths } from 'src/app/constants/routes';
import { BookData } from 'src/app/models/book-data';
import { CommentModel } from 'src/app/models/commentmodel';
import { AuthService } from 'src/app/services/auth.service';
import { BookService } from 'src/app/services/book.service';
import { SubscriptionService } from 'src/app/services/subscription.service';
import { BookSubscribersComponent } from '../book-subscribers/book-subscribers.component';

@Component({
  selector: 'book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.scss'],
  providers: [NgbRatingConfig]
})
export class BookDetailsComponent implements OnInit {
  bookId: string;
  bookData?: BookData;
  commentModel = new CommentModel(0,"","", "");
  ratings = [1, 2, 3, 4, 5];
  totalItems: number = 0; 
  comments: CommentModel[] = [];
  pageNumber: number = paginationComments.pageNumber;
  pageSize: number = paginationComments.pageSize;
  copiesAmount: number = 1;

  errorMsg: string = '';
  successMsg: string = '';

  totalSubscribers : number = 0;

  currentRate : number = 0;

  constructor(private activatedRoute: ActivatedRoute, private bookService: BookService,
     private subscriptionService : SubscriptionService,  private authService: AuthService, 
     private router: Router, config: NgbRatingConfig) {
    this.bookId = this.activatedRoute.snapshot.params.id;
    config.max = 5;
  }
  
  ngOnInit(): void {
    this.bookService.getBookData(this.bookId).subscribe((data: BookData) => {
      this.bookData = data;
    })

    this.getCurrentPageData(this.pageNumber);

    this.subscriptionService.getTotalSubscribersForABook(parseInt(this.bookId))
      .subscribe(subscriptions => {
        this.totalSubscribers =  subscriptions.totalSubscribers;
      })


      this.bookService.getComments(this.pageNumber, this.pageSize, this.bookId)
      .subscribe(newComments => {
     this.comments =  newComments.paginatedCommentResponses;
     this.totalItems = newComments.totalCount;
   })

  };

  OnSubscribe(){
    const data = {
      BookId: this.bookId,
      Copies: this.copiesAmount
      }
      
    this.bookService.subscribeToBook(data)
    .subscribe(newBooks => {
      this.successMsg = 'You have been subscribed for that book but your subscription needs to be reviewed by an admin first.';
      this.bookService.getBookData(this.bookId).subscribe((data: BookData) => {
        this.bookData = data;
      })

      this.subscriptionService.getTotalSubscribersForABook(parseInt(this.bookId))
      .subscribe(subscriptions => {
        this.totalSubscribers =  subscriptions.totalSubscribers;
      })
    }, (error => {
      if(error.error.error === 'User already subscribed for that book.'){
        this.errorMsg = 'You are already subscribed for that book.';
      }
      else{
        this.errorMsg = error.error.error;
      }
    }));
  }

  clearErrorMessage(): void {
    this.errorMsg = '';
  }

  clearSuccessMessage(): void {
    this.successMsg = '';
  }

  OnComment(){
    const data = {
      rating: this.currentRate,
      commentBody: this.commentModel.commentBody
      }

      if (!data.rating) {
        this.errorMsg = "Rating is required!";
      }
      else if(!data.commentBody){
        this.errorMsg = "Comment is required!";
      }
      else if(!data.commentBody.trim()){
        this.errorMsg = "You cannot comment with whitespaces!";
      }
      else{
        this.bookService.addReview(data, this.bookId)
        .subscribe(newReview => {
          this.successMsg = "Comment submitted!";
          this.bookService.getComments(this.pageNumber, this.pageSize, this.bookId)
          .subscribe(newComments => {
         this.comments =  newComments.paginatedCommentResponses;
         this.totalItems = newComments.totalCount;
       })
        }, (error => {
         this.errorMsg = error.error.error;
        }));
      }
  }

  getCurrentPageData(pageNumber: number){
    this.bookService.getComments(this.pageNumber, this.pageSize, this.bookId)
    .subscribe(newComments => {
      this.comments =  newComments.paginatedCommentResponses;
      this.totalItems = newComments.totalCount;
    })
  }


  

  OnDelete(){
    if(confirm("Are you sure you want to delete this book?")) {
      this.bookService.deleteBook(this.bookId)
    .subscribe(deleteBook => {
        this.router.navigate([routePaths.adminPanel]);
    });
  }
  }
  
  isUserAdmin() : boolean{
    return this.authService.isUserAdmin();
  }
  
  checkIfUserIsAuthenticated() : boolean{
    return this.authService.isUserAuthenticated();
  }

  changeBookAvailability(available: boolean){
    this.bookService.changeBookAvailability(this.activatedRoute.snapshot.params.id, available)
    .subscribe(() => {
      if(available){
        this.successMsg = 'The book is now available for subscriptions.';
      }
      else{
        this.successMsg = 'The book is now unavailable for subscriptions.';
      }
      this.bookService.getBookData(this.bookId)
      .subscribe((data: BookData) => {
        this.bookData = data;
      })
    },() => {
      this.errorMsg = 'Something occured. Please try again.';
    })
  }
}
