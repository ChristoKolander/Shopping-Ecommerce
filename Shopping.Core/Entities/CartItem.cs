namespace Shopping.Core.Entities
{
    public class CartItem : BaseEntity<int> 
    {
     
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public string CartStringId { get; set; }
        public string UserClaimStringId { get; set;}

    }


}

