using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Core.Entities
{
    public class Product : BaseEntity<int>
    { 
        public string Name { get; set; }
        public string Description { get; set; }     
        public string ImageURL { get; set; }
        public decimal Price { get; set; }   
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }    

    }
}
