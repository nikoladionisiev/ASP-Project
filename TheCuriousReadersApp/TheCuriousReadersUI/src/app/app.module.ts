import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import {NgxPaginationModule} from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { RegisterComponent } from './components/register/register.component';
import { ErrorMessageComponent } from './components/error-message/error-message.component';
import { SuccessMessageComponent } from './components/success-message/success-message.component';
import { BookComponent } from './components/book/book.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { NewBooksComponent } from './components/new-books/new-books.component';
import { GenresBooksComponent } from './components/genres-books/genres-books.component';
import { LoginComponent } from './components/login/login.component';
import { HeaderComponent } from './components/header/header.component';
import { BookSubscribersComponent } from './components/book-subscribers/book-subscribers.component';
import { ValidateEqualModule } from 'ng-validate-equal';
import { TotalReadersComponent } from './components/total-readers/total-readers.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AdminViewSubscriptionsComponent } from './components/admin-view-subscriptions/admin-view-subscriptions.component';
import { ContenteditableValueAccessorModule } from '@tinkoff/angular-contenteditable-accessor';
import { environment } from 'src/environments/environment';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TooltipModule } from 'ng2-tooltip-directive';


export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    AdminPanelComponent,
    ErrorMessageComponent,
    SuccessMessageComponent,
    BookComponent,
    BookDetailsComponent,
    FooterComponent,
    HomeComponent,
    LandingPageComponent,
    NewBooksComponent,
    GenresBooksComponent,
    LoginComponent,
    HeaderComponent,
    BookSubscribersComponent,
    TotalReadersComponent,
    AdminViewSubscriptionsComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    ValidateEqualModule,
    ContenteditableValueAccessorModule,
    TooltipModule,
    NgbModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:44385', 'localhost:5001']
      }
      
    }),
    
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [AppRoutingModule] 
})
export class AppModule { }
 