using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages
{
    public class ShoppingCartBase : ComponentBase
    {

          
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
  
        public List<CartItemDto> ShoppingCartItems { get; set; }


        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public ICartService ShoppingCartService { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

   
        protected override async Task OnInitializedAsync()
        {


            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            CartChanged();

        }

             
        protected async Task DeleteCartItem_Click(int id)
        {

            var cartItemDto = await ShoppingCartService.DeleteCartItem(id);
            await RemoveCartItem(id);
            CartChanged();

        }
     
        private async Task RemoveCartItem(int id)
        {

            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);

        }
     
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
      
        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {

            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };

                var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                await UpdateItemTotalPrice(returnedUpdateItemDto);
                CartChanged();
                await MakeUpdateQtyButtonVisible(id, false);

            }
            else
            {
                var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                if (item != null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }

            }

        }
       
        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }
     
        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }
      
        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {

            var item = GetCartItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }


            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);

        }
       
        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        
        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }
       
        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }
       
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

   


    }
}
