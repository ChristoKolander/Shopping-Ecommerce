using Shopping.Core.Entities;

namespace Shopping.Api.CQRS
{
    public class OrderDto
    {
        private const string DEFAULT_STATUS = "Pending";

        public int OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status => DEFAULT_STATUS;
        public Address ShippingAddress { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();

    }
}
