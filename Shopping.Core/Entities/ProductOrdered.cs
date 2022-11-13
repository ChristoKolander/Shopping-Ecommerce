using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shopping.Core.Entities
{
  
    //Using this to store values made when order was placed. Price might change, but not here!

    public class ProductOrdered: BaseEntity<int>

    { 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ImageUrl { get; set; }

        public ProductOrdered(){ }

        public ProductOrdered(int productId, string productName, string imageUrl, decimal productPrice)
        {           
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
            ProductPrice = productPrice;
        }

     
    }
}
