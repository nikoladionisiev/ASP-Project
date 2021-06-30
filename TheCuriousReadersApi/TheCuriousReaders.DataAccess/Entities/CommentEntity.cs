using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheCuriousReaders.DataAccess.Entities
{
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]   
        public int Rating { get; set; }

        public string UserId { get; set; }

        public UserEntity User { get; set; }

        public int BookId { get; set; }

        public BookEntity Book { get; set; }

        [MaxLength(800)]
        public string CommentBody { get; set; }
    }
}
