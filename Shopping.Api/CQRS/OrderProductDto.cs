
namespace Shopping.Api.CQRS
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount => 0;
        public int Units { get; set; }
        public string ImageUrl { get; set; }
    }

}

