using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
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
        public List<CartItemDto> OldList { get; set; } = new List<CartItemDto>();
        public List<CartItemDto> OldCollection { get; set; } = new List<CartItemDto>();

        public Shopping.Core.Entities.ShoppingCart ShoppingCart { get; set; } = new Core.Entities.ShoppingCart();

        # endregion

        # region Injected Services

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
    
        [Inject]
        AuthStateProvider AuthStateProvider { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        # endregion

        protected override async Task OnInitializedAsync()
        {
            ShoppingCart = new Core.Entities.ShoppingCart();

            this.CartStringId = await ManageCartItemsLocalStorageService.GetCartStringId();
            this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();   //??
            
            await GetAuth();

            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            ProductUpdateDto = await ProductService.GetProduct(Id);

        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
                
            await GetCart();

            //For some reason, UserClaimStringId kept running out of scope(?), so need to use/add it here.
            cartItemToAddDto.UserClaimStringId = this.UserClaimStringId;


            // Returning null or default from Repo (suddenly) causes a null reference exception, that if
            // if item is already inside the basket. Instead of hitting DB and get that exception, 
            // and/or refactor chained code from basket to repo and back, intercept here. Avoiding hitting
            // db also. A pop up informning "Item already in basket. Please, update qty in basket instead!" will be shown before
            // redirecting to basket (eventually...), but this solves the problem for now.

            if (ShoppingCartItems.Any(p=>p.ProductId == cartItemToAddDto.ProductId))
            {
                NavigationManager.NavigateTo("/ShoppingCart");
                return;
            }

       
            var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);
            
            cartItemDto.UserClaimStringId = this.UserClaimStringId;

            if (cartItemDto != null)
            {

                ShoppingCartItems.Add(cartItemDto);
                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");
        }

        protected async Task<string> GetAuth()
        {
            var authstate = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authstate.User;
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (userId.Length > 0 && userId != UserClaimStringId)
                {

                    //When logging in for the first time, Removing notLoggedin LocalStorage values
                    //and the notLoggedin associated ShoppingCart.

                    this.OldCollection = await ManageCartItemsLocalStorageService.GetCollection();
                    await ManageCartItemsLocalStorageService.RemoveCollection();

                    this.OldList = await ShoppingCartService.GetItems2(UserClaimStringId);
                    await ShoppingCartService.DeleteShoppingCart(UserClaimStringId);
                   

                    await ManageCartItemsLocalStorageService.RemoveUserClaimStringId();
                    UserClaimStringId = userId;
                    await ManageCartItemsLocalStorageService.SaveUserClaimStringId(UserClaimStringId);

                    await ManageCartItemsLocalStorageService.RemoveCartStringId();
                    CartStringId = userId;
                    await ManageCartItemsLocalStorageService.SaveCartStringId(CartStringId);


                    foreach (var item in OldCollection)
                    {
                        item.CartStringId = CartStringId;
                    }

                    await ManageCartItemsLocalStorageService.SaveCollection(OldCollection);

                    //Localstorage Items all set by this point...


                    // Unable to delete ShoppingCartItems, did not work(need the old generated id).
                    // So approach is to update existing ids with LoggedInValues into a new List.

                    List<CartItemDto> list = new List<CartItemDto>();
                                    

                    foreach (var item in OldList)
                    {
                        list.Add(item);
                    }


                    foreach (var item in list)
                    {
                        item.CartStringId = CartStringId;
                        item.UserClaimStringId = UserClaimStringId;


                    }

                    foreach (var item in list)
                    {
                   
                        await ShoppingCartService.UpdateCartItem(item);
                    }



                    this.ShoppingCart.CartStringId = UserClaimStringId;
                    this.ShoppingCart.UserClaimStringId = UserClaimStringId;


                    if (CartCreatedValue != "true")
                    {
                        await ShoppingCartService.CreateShoppingCart(ShoppingCart);
                    }

                    // Setting a flag (new cart is Created here, having the new loggedIn values).
                    // And adding the the new CartStringId now associated with UserId made by system during registration, to LocalStorage.


                    CartCreatedValue = "true";
                    await ManageCartItemsLocalStorageService.AddCartCreatedValue(CartCreatedValue);
                    await ManageCartItemsLocalStorageService.SaveCartStringId(CartStringId);

                }

                // Returning the LoggedIn Users Id.

                return UserClaimStringId;

            }
         
            // IF not authenticated, run this code.
            
            else
            {
                this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();

                if (UserClaimStringId == null)

                {
                    Guid tempCartId = Guid.NewGuid();
                    UserClaimStringId = tempCartId.ToString();
                    CartStringId = tempCartId.ToString();
                    await ManageCartItemsLocalStorageService.SaveUserClaimStringId(UserClaimStringId);
                    await ManageCartItemsLocalStorageService.SaveCartStringId(CartStringId);


                    return UserClaimStringId;

                }

                else
                {
                    return UserClaimStringId;

                }

            }
        }

        // Only want ONE cart for each user - no matter loggedIn or not - in DB, this code helps doing so.
        // But maybe some lines are not needed, have to check it when time.

        protected async Task<string> GetCart()
        {
            this.CartStringId = await ManageCartItemsLocalStorageService.GetCartStringId();
            this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();
            this.CartCreatedValue = await ManageCartItemsLocalStorageService.GetCartCreatedValue();

            if (CartStringId.Length==0 && CartCreatedValue!="true")
            {
                var newCart = await ShoppingCartService.CreateShoppingCart(ShoppingCart);
                this.CartStringId = newCart.CartStringId;
                CartCreatedValue = "true";
                await ManageCartItemsLocalStorageService.AddCartCreatedValue(CartCreatedValue);
            }
            
            else
            {
   
                this.CartStringId = this.UserClaimStringId;
                this.ShoppingCart.CartStringId = this.CartStringId;
                this.ShoppingCart.UserClaimStringId = this.UserClaimStringId;           
                {
                    if (CartCreatedValue!="true")
                    {
                        var newCart = await ShoppingCartService.CreateShoppingCart(ShoppingCart);
                        CartCreatedValue = "true";
                        await ManageCartItemsLocalStorageService.AddCartCreatedValue(CartCreatedValue);
                    }
               
                }
          
            }
                return this.UserClaimStringId;

        }   

    }
}