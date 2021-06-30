using System;
using System.Collections.Generic;
using System.Text;
using TheCuriousReaders.Models.ServiceModels;

namespace TheCuriousReaders.Models.ResponseModels
{
    public class PaginatedCommentResponse
    {
        public ICollection<PaginatedCommentModel> PaginatedCommentResponses { get; set; }
        public int TotalCount { get; set; }
    }
}
