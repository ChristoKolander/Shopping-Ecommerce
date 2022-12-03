using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Core.Entities
{
    public class OrderItem : BaseEntity<int>
    {

        public int ProductOrderedId { get; set; }
        public ProductOrdered ProductOrdered { get; set; }

        public decimal UnitPrice { get; set; }
        public int Units { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

    
        public OrderItem () { }

        public OrderItem(ProductOrdered productOrdered, decimal unitPrice, int units)
        {
            ProductOrdered = productOrdered;
            UnitPrice = unitPrice;
            Units = units;
         
        }

    
       
  


    }
}
