using System.ComponentModel.DataAnnotations;


namespace Shopping.Core.Entities
{
  
    //Using this to store values made when order was placed. Price might change, but not here!

    public class ProductOrdered : BaseEntity<int>
    {
      
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }

        private ProductOrdered(){ }

        public ProductOrdered(int productId, string productName, string imageUrl)
        {           
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
        }
    }
}
