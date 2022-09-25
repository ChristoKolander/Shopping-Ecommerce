using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shopping.Models.Dtos.CRUDs
{
    public class ProductUpdateDto
    {

        public int Id { get; set; }
        [StringLength(75, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description can't be longer than 150 characters")]
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }

    }
}
