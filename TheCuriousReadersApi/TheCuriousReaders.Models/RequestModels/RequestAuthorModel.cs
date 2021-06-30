using System.ComponentModel.DataAnnotations;

namespace TheCuriousReaders.Models.RequestModels
{
    public class RequestAuthorModel
    {
        [Required(ErrorMessage = "Author is required.")]
        public string Name { get; set; }
    }
}
