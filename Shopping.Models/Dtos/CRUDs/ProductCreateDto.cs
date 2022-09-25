using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shopping.Models.Dtos.CRUDs
{
    public class ProductCreateDto
    {


        [StringLength(75, MinimumLength = 4)]
        [Required]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description can't be longer than 150 characters")]    
        [Required]
        public string Description { get; set; }
        
        public string ImageURL { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
       
        public int? Qty { get; set; }
       
        public int? CategoryId { get; set; }


    }
}
