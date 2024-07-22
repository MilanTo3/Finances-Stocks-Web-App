using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class CreateStockDTO
    {
        [Required]
        [MinLength(5, ErrorMessage="Symbol must be at least 5 characters long.")]
        [MaxLength(10, ErrorMessage="Symbol cannot be over 280 character.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage="Company Name must be at least 5 characters long.")]
        [MaxLength(10, ErrorMessage="Company Name cannot be over 280 character.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 100000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage="Industry cannot be over 10.")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}