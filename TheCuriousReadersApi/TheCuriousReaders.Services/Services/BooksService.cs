using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCuriousReaders.DataAccess.Interfaces;
using TheCuriousReaders.Models.Helpers;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ResponseModels;
using TheCuriousReaders.Models.ServiceModels;
using TheCuriousReaders.Services.CustomExceptions;
using TheCuriousReaders.Services.Interfaces;

namespace TheCuriousReaders.Services.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBlobService _blobService;

        public BooksService(IBookRepository bookRepository,
            IMapper mapper,
            IAuthorRepository authorRepository,
            IBlobService blobService)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _blobService = blobService;
        }

        public async Task<int> CountOfNewBooksAsync()
        {
            return await _bookRepository.CountOfNewBooksAsync();
        }

        public async Task<BookModel> CreateBookAsync(BookModel bookModel)
        {
            if (await _authorRepository.TitleWithAuthorExists(bookModel.Title, bookModel.Author.Name))
            {
                throw new DuplicateResourceException($"The book {bookModel.Title} from {bookModel.Author.Name} already exists in our database.");
            }

            if (!GenresValidator.IsGenreValid(bookModel.Genre.Name))
            {
                throw new ValidationException($"This genre '{bookModel.Genre.Name}' does not exist in our database.");
            }

            return await _bookRepository.CreateBookAsync(bookModel);
        }

        public async Task<BookModel> GetABookByIdAsync(int id)
        {
            return await _bookRepository.GetABookAsync(id);
        }

        public async Task<ICollection<BookModel>> GetBooksWithPaginationAsync(PaginationParameters paginationParameters, bool shouldBeNewlyCreated = false)
        {
            return await _bookRepository.GetBooksWithPaginationAsync(paginationParameters, shouldBeNewlyCreated);
        }

        public async Task<PaginatedNewBookModel> GetPaginatedBooksAndTheirTotalCountAsync(ICollection<BookModel> newBooks)
        {
            var totalCount = await _bookRepository.CountOfNewBooksAsync();

            var paginatedNewBooks = new PaginatedNewBookModel
            {
                NewBooks = newBooks,
                TotalCount = totalCount
            };

            return paginatedNewBooks;
        }

        public async Task<SearchResponse> SearchAsync(PaginationParameters paginationParameters, SearchParameters model)
        {
            var totalCount = await _bookRepository.CountOfSearchAsync(model);

            var books = new SearchResponse
            {
                Books = await _bookRepository.SearchAsync(paginationParameters, model),
                TotalCount = totalCount
            };

            return books;
        }

        public async Task<BookModel> DeleteABookByIdAsync(int id)
        {
            return await _bookRepository.DeleteABookAsync(id);
        }

        public async Task<BookModel> AddCoverAsync(int id, IFormFile cover)
        {
            var coverUri = await _blobService.UploadFileAsync(cover);

            return await _bookRepository.AddCoverAsync(id, coverUri);
        }

        public async Task PatchABookAsync(int id, BookModel bookModel)
        {
            await _bookRepository.PatchABookAsync(id, bookModel);
        }

        public async Task<BookModel> UpdateABookByIdAsync(int id, BookModel bookModel)
        {
            if (await _authorRepository.TitleWithAuthorExists(bookModel.Title, bookModel.Author.Name))
            {
                throw new DuplicateResourceException($"The book {bookModel.Title} from {bookModel.Author.Name} already exists in our database.");
            }

            if (!GenresValidator.IsGenreValid(bookModel.Genre.Name))
            {
                throw new ValidationException($"This genre '{bookModel.Genre.Name}' does not exist in our database.");
            }

            return await _bookRepository.UpdateABookAsync(id, bookModel);
        }
    }
}
