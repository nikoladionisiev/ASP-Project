﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ResponseModels;
using TheCuriousReaders.Models.ServiceModels;
using TheCuriousReaders.Services.Interfaces;

namespace TheCuriousReaders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBooksService _bookService;
        private readonly IGenresService _genreService;
        private readonly ICommentService _commentService;

        public BooksController(IMapper mapper, 
        IBooksService booksService, 
        IGenresService genreService,
        ICommentService commentService,
        IBlobService blobService)
        {
            _bookService = booksService;
            _mapper = mapper;
            _genreService = genreService;
            _commentService = commentService;
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewlyCreated([FromQuery] PaginationParameters paginationParameters)
        {
            var paginatedBooks = await _bookService.GetBooksWithPaginationAsync(paginationParameters, true);
            var paginatedNewBooksResponse = await _bookService.GetPaginatedBooksAndTheirTotalCountAsync(paginatedBooks);

            return Ok(_mapper.Map<PaginatedNewBookResponse>(paginatedNewBooksResponse));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters)
        {
            var paginatedBooks = await _bookService.GetBooksWithPaginationAsync(paginationParameters);

            return Ok(_mapper.Map<ICollection<BookResponse>>(paginatedBooks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetABookByIdAsync(id);

            if (book is null)
            {
                return NotFound($"Book with id {id} could not be found in the database.");
            }

            return Ok(_mapper.Map<BookResponse>(book));
        }

        [HttpGet("genres")]
        public async Task<IActionResult> Get()
        {
            var response = await _genreService.GetGenresAsync();

            return Ok(response);
        }

        [HttpGet("{bookId}/comments")]
        public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters, int bookId)
        {
            var paginatedComments = await _commentService.GetCommentsWithPaginationAsync(paginationParameters, bookId);
            var paginatedCommentsResponse = new PaginatedCommentResponse()
            {
                PaginatedCommentResponses = paginatedComments,
                TotalCount = await _commentService.GetTotalCommentsForABookAsync(bookId)
            };

            return Ok(paginatedCommentsResponse);
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters, [FromQuery] SearchParameters model)
        {
            var response = await _bookService.SearchAsync(paginationParameters, model);

            if (response.Books.Count == 0)
            {
                return NotFound("No matching records!");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestBookModel requestBookModel)
        {
            var bookToCreate = await _bookService.CreateBookAsync(_mapper.Map<BookModel>(requestBookModel));

            if (!(bookToCreate is null))
            {
                return StatusCode(201, _mapper.Map<BookResponse>(bookToCreate));
            }

            return BadRequest("Something occured. Please try again.");
        }

        [HttpPost]
        [Route("{bookId}/upload-cover")]
        public async Task<IActionResult> Post([FromForm] IFormFile cover, [FromRoute] int bookId)
        {
            var book = await _bookService.AddCoverAsync(bookId, cover);

            if (!(book is null))
            {
                return StatusCode(201, _mapper.Map<BookResponse>(book));
            }

            return BadRequest("Something occured. Please try again.");
        }

        [Authorize]
        [HttpPost("{bookId}/comments")]
        public async Task<IActionResult> Post([FromBody] RequestCommentModel requestCommentModel, int bookId)
        {
            var userId = User.Claims.SingleOrDefault(c => c.Type == "id").Value;
            var commentModel = _mapper.Map<CommentModel>(requestCommentModel);

            commentModel.UserId = userId;
            commentModel.BookId = bookId;

            var commentToCreate = await _commentService.CreateCommentAsync(commentModel);

            if (!(commentToCreate is null))
            {
                return StatusCode(201, _mapper.Map<CommentResponse>(commentToCreate));
            }

            return BadRequest("An error occured. Please try again.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _bookService.DeleteABookByIdAsync(id);

            if (record == default)
                return NotFound($"Record with id {id} was not found.");

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<RequestBookPatch> patchDoc)
        {
            var book = await _bookService.GetABookByIdAsync(id);
            if (book is null)
            {
                return NotFound($"Book with id {id} does not exist in our database.");
            }

            var requestBookPatchMapping = _mapper.Map<RequestBookPatch>(book);
            //If any exception occurs here it is due to bad request of the JsonPatch request body
            try
            {
                patchDoc.ApplyTo(requestBookPatchMapping);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }

            await _bookService.PatchABookAsync(id, _mapper.Map<BookModel>(requestBookPatchMapping));

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestBookModel requestBookModel)
        {
            if (_bookService.GetABookByIdAsync(id) == null)
            {
                return NotFound($"Record with id {id} not found.");
            }

            var bookToCreate = await _bookService.UpdateABookByIdAsync(id, _mapper.Map<BookModel>(requestBookModel));

            if (!(bookToCreate is null))
            {
                return StatusCode(201, _mapper.Map<BookResponse>(bookToCreate));
            }

            return BadRequest("Something occured. Please try again.");
        }

    }
}

