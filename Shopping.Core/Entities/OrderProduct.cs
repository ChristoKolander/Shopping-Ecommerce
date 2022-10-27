namespace Shopping.Core.Entities
{
    public class OrderProduct : BaseEntity<int>
    {
        public ProductOrdered ProductOrdered { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }

        private OrderProduct () { }

        public OrderProduct(ProductOrdered productOrdered, decimal unitPrice, int units)
        {
            ProductOrdered = productOrdered;
            UnitPrice = unitPrice;
            Units = units;
        }


    }
}
