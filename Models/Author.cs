using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Questo campo è obbligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "questo campo deve contenere minimo 3 caratteri massimo 20")]

        public string Username { get; set; }
        [Required(ErrorMessage = "Questo campo è obbligatorio")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "questo campo deve contenere minimo 8 caratteri massimo 15 caratteri")]

        public string Password { get; set; }
    }
}