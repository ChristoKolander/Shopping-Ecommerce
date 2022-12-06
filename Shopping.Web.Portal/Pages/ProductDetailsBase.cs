using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Pages.Admin.Users;
using Shopping.Web.Portal.Pages.Auth;
using Shopping.Web.Portal.Services.Interfaces;
using System.Security.Claims;



namespace Shopping.Web.Portal.Pages
{
    public class ProductDetailsBase : ComponentBase
    {

        #region Fields and Properties

        [Parameter]
        public int Id { get; set; }

        public string CartCreatedValue { get; set; }
        public string CartStringId { get; set; }
        public string UserClaimStringId { get; set; }

        public ProductUpdateDto ProductUpdateDto { get; set; }
        public CartItemToAddDto CartItemToAddDto { get; set; }

        private List<CartItemDto> ShoppingCartItems { get; set; }
      

        public Shopping.Core.Entities.Cart Cart { get; set; } = new Core.Entities.Cart();

        #endregion


        #region Injected Services
    
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ICartService ShoppingCartService { get; set; }
    

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        # endregion


        protected override async Task OnInitializedAsync()
        {
            Cart = new Core.Entities.Cart();

            this.CartStringId = await ManageCartItemsLocalStorageService.GetCartStringId();
            this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();   //??
            
            //await GetAuth();

            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            ProductUpdateDto = await ProductService.GetProduct(Id);

        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
                
            await GetCart();

            cartItemToAddDto.UserClaimStringId = this.UserClaimStringId;


            if (ShoppingCartItems.Any(p=>p.ProductId == cartItemToAddDto.ProductId))
            {
                NavigationManager.NavigateTo("/ShoppingCart");
                return;
            }

       
            var cartItemDto = await ShoppingCartService.AddCartItem(cartItemToAddDto);
            
            cartItemDto.UserClaimStringId = this.UserClaimStringId;

            if (cartItemDto != null)
            {

                ShoppingCartItems.Add(cartItemDto);
                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");
        }
    
        protected async Task<string> GetCart()
        {
            this.CartStringId = await ManageCartItemsLocalStorageService.GetCartStringId();
            this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();
            this.CartCreatedValue = await ManageCartItemsLocalStorageService.GetCartCreatedValue();

            if (CartStringId.Length==0 && CartCreatedValue!="true")
            {
                var newCart = await ShoppingCartService.CreateCart(Cart);
                this.CartStringId = newCart.CartStringId;
                CartCreatedValue = "true";
                await ManageCartItemsLocalStorageService.AddCartCreatedValue(CartCreatedValue);
            }
            
            else
            {
   
                this.CartStringId = this.UserClaimStringId;
                this.Cart.CartStringId = this.CartStringId;
                this.Cart.UserClaimStringId = this.UserClaimStringId;           
                {
                    if (CartCreatedValue!="true")
                    {
                        var newCart = await ShoppingCartService.CreateCart(Cart);
                        CartCreatedValue = "true";
                        await ManageCartItemsLocalStorageService.AddCartCreatedValue(CartCreatedValue);
                    }
               
                }
          
            }
                return this.UserClaimStringId;

        }

     }
}