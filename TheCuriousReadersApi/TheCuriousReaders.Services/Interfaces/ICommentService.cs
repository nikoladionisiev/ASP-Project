using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ServiceModels;

namespace TheCuriousReaders.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentModel> CreateCommentAsync(CommentModel commentModel);
        Task<ICollection<PaginatedCommentModel>> GetCommentsWithPaginationAsync(PaginationParameters paginationParameters, int bookId);
        Task<int> GetTotalCommentsForABookAsync(int bookId);
    }
}
