using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Entities
{
    public class Product
    {

        public int Id { get; set; }
        
       
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        [Required]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description can't be longer than 150 characters")]
        [Required]
        public string Description { get; set; }
        
        public string ImageURL { get; set; }

   
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        
        public int Qty { get; set; }

        public int CategoryId { get; set; }

        public string WebApiAdminInfoOnly { get; set; }


        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }


    }
}
