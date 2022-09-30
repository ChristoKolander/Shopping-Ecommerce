using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Models.Dtos.CRUDs
{
   public class CartItemToAddDto
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }

        public string CartStringId { get; set; }

    }
}
