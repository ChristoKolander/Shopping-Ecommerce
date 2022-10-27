using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shopping.Shared.Dtos.CRUDs
{
    public class ProductUpdateDto
    {

        public int Id { get; set; }

        [StringLength(75, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Description can't be longer than 150 characters")]
        [Required]
        public string Description { get; set; }

        public string ImageURL { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Qty { get; set; }

        public int ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }


    }
}
