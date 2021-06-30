import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { routePaths } from '../../constants/routes';
import { SearchModel } from 'src/app/models/searchmodel';
import { SearchService } from "../../services/search.service";
import { SearchResponseModel } from "../../models/searchResponseModel";
import { paginationParameters } from 'src/app/constants/paginationparameters';
import { PaginatedNewSearchResultsModel } from '../../models/paginatednewsearchresultsmodel';
import { paginationSearch } from 'src/app/constants/paginationsearch';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {
  parameterToSearchFor: string = '';
  textToSearchFor: string = '';
  parameters = ["Book title", "Book author", "Description keywords", "Comments"];
  errorMsg: string = '';
  searchResults: SearchResponseModel[] = [];
  successMsg: string = '';
  searchPageNumber: number = paginationSearch.pageNumber;
  searchPageSize: number = paginationSearch.pageSize;
  searchData = new SearchModel("", "");
  bookCountInResponse = new PaginatedNewSearchResultsModel(this.searchResults, 0);

  constructor(private authService: AuthService, private router: Router, private searchService: SearchService) {}

  ngOnInit(): void {}

  logOut() {
    this.authService.logOut();
  }

  openBookDetails(id: number): void {    
    this.router.navigate([routePaths.bookDetails.concat(id.toString())]);
  }

  openAdminPage(): void {
    this.router.navigate([routePaths.adminPanel]);
  }

  openDashboard(): void {
    this.router.navigate([routePaths.dashboardPage]);
  }

  onSubmit(): void {
    const data = {
      parameter: this.parameterToSearchFor,
      text: this.textToSearchFor
    }

    const jsonRequest = JSON.stringify(data);

    if (!data.text) {
      this.errorMsg = "Please enter text to search for.";
    } else if (!data.text.trim()) {
      this.errorMsg = "You can't search only for whitespaces!";
    } else if (!data.parameter) {
      this.errorMsg = "Please enter a parameter to search for."
    } else {
      this.searchData = data;
      this.getSearchResults(data);
    }
  }

  clearErrorMessage(): void {
    this.errorMsg = '';
  }

  clearSuccessMessage(): void {
    this.successMsg = '';
  }

  getSearchResults(data: SearchModel) {
    const {
      searchPageNumber,
      searchPageSize
    } = this;

    this.searchService.search(data, searchPageNumber, searchPageSize)
      .subscribe(res => {
        this.bookCountInResponse.totalCount = res.totalCount
        this.bookCountInResponse.books = res.books

      }, error => {
        this.errorMsg = "No results.";
      });
  }

  getCurrentPageData(newPage: number) {
    this.searchPageNumber = newPage;

    const {
      searchPageNumber,
      searchPageSize
    } = this;

    this.searchService.search(this.searchData, searchPageNumber, searchPageSize)
      .subscribe(newBooks => {
        this.bookCountInResponse.totalCount = newBooks.totalCount
        this.bookCountInResponse.books = newBooks.books
      })
  }

  isUserAdmin() : boolean {
    return this.authService.isUserAdmin();
  }
}
