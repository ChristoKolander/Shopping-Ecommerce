using System.Collections.Generic;


namespace Shopping.Core.Entities
{

    public class Cart : BaseEntity<int>
    {

            public string UserClaimStringId { get; set; } 
            public string CartStringId { get; set; }

            public List<CartItem> CartItems = new List<CartItem>(); 
        }



}

