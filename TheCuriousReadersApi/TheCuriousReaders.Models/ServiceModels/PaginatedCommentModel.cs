using System;
using System.Collections.Generic;
using System.Text;

namespace TheCuriousReaders.Models.ServiceModels
{
    public class PaginatedCommentModel
    {
        public double Rating { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string CommentBody { get; set; }
    }
}
