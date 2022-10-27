
namespace Shopping.Shared.Dtos.CRUDs
{
   public class CartItemToAddDto
    {
        public string CartStringId { get; set; }
        public string UserClaimStringId{ get; set; }     
        public int ProductId { get; set; }
        public int Qty { get; set; }


    }
}
