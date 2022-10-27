using System.Collections.Generic;


namespace Shopping.Core.Entities
{

    public class ShoppingCart : BaseEntity<int>
    {

            public string UserClaimStringId { get; set; } 
            public string CartStringId { get; set; }

            public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
        }



}

