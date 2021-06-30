﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCuriousReaders.DataAccess.Interfaces;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ServiceModels;
using TheCuriousReaders.Services.Interfaces;

namespace TheCuriousReaders.Services.Services
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentModel> CreateCommentAsync(CommentModel commentModel)
        {
            return await _commentRepository.CreateCommentAsync(commentModel);
        }

        public async Task<ICollection<PaginatedCommentModel>> GetCommentsWithPaginationAsync(PaginationParameters paginationParameters, int bookId)
        {
            return await _commentRepository.GetCommentsWithPaginationAsync(paginationParameters, bookId); ;
        }

        public Task<int> GetTotalCommentsForABookAsync(int bookId)
        {
            return _commentRepository.GetTotalCommentsForABookAsync(bookId);
        }
    }
}
