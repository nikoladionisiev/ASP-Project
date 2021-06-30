using System;
using System.Collections.Generic;
using System.Text;

namespace TheCuriousReaders.Models.ServiceModels
{
    public class CommentModel
    {
        public int Rating { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }

        public string CommentBody { get; set; }
    }
}
