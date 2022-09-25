using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Models.Dtos.CRUDs
{
   public class CartItemQtyUpdateDto
    {
        public int CartItemId { get; set; }
        public int Qty { get; set; }
    }
}
