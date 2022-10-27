namespace Shopping.Core.Entities
{
    public class ProductCategory: BaseEntity<int>
    {   
        public string Name { get; set; }
        public string IconCSS { get; set; }
    }
}
