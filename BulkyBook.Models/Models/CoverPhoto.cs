using System;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class CoverPhoto
    {
        [Key]
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
    }
}
