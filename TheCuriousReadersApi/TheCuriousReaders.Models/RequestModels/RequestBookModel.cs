using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace TheCuriousReaders.Models.RequestModels
{
    public class RequestBookModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Quantity must be more than 0.")]
        public int Quantity { get; set; }

        public RequestAuthorModel Author { get; set; }

        public RequestGenreModel Genre { get; set; }
    }
}
