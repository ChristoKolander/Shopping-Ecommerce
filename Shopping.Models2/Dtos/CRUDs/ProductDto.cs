using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shopping.Models.Dtos.CRUDs
{
   public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(75, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description can't be longer than 150 characters")]
        public string Description { get; set; } 
       
        public string ImageURL { get; set; }

       
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        
        public int Qty { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}

