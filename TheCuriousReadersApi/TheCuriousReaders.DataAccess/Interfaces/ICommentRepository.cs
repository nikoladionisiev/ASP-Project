using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCuriousReaders.DataAccess.Entities;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ServiceModels;

namespace TheCuriousReaders.DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentModel> CreateCommentAsync(CommentModel commentModel);
        Task<ICollection<PaginatedCommentModel>> GetCommentsWithPaginationAsync(PaginationParameters paginationParameters, int bookId);
        Task<int> GetTotalCommentsForABookAsync(int bookId);
    }
}
