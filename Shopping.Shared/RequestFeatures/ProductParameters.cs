namespace Shopping.Shared.Entities.RequestFeatures
{
    public class ProductParameters: QueryStringParameters
    {
        public uint MinPrice { get; set; } = 1;
        public uint MaxPrice { get; set; } = 50000;
        public bool ValidPriceRange => MaxPrice > MinPrice;

    }
}
