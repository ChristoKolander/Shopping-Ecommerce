namespace Shopping.Shared.Dtos.CRUDs
{
 
       public class CartItemDto
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string CartStringId { get; set; }
            public string UserClaimStringId { get; set; }   
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string ProductImageURL { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
            public int Qty { get; set; }



    }

 }

