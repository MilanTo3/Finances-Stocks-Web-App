using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class UserDTO
    {
        [Required]
        public string? UserName{get;set;}
        [Required]
        [EmailAddress]
        public string? Email{get;set;}
        [Required]
        public string? Password{get;set;}
        
        

    }
}