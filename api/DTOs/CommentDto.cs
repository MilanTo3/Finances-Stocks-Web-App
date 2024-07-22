using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class CommentDTO
    {
        [Required]
        [MinLength(5, ErrorMessage="Tittle must be at least 5 characters long.")]
        [MaxLength(280, ErrorMessage="Tittle cannot be over 280 character.")]
        public string Title{get;set;} = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage="Tittle must be at least 5 characters long.")]
        [MaxLength(280, ErrorMessage="Tittle cannot be over 280 character.")]
        public string Content{get;set;} = string.Empty;

    }
}