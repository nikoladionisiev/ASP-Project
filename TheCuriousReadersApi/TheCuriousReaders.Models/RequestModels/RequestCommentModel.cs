using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheCuriousReaders.Models.RequestModels
{
    public class RequestCommentModel
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(150, MinimumLength = 2,
        ErrorMessage = "Comment should be minimum 2 characters and maximum of 150 characters.")]
        public string CommentBody { get; set; }
    }
}
