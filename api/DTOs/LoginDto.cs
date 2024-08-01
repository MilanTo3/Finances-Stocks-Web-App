using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserName {get;set;}
        [Required]
        public string Password {get;set;}
    }
}