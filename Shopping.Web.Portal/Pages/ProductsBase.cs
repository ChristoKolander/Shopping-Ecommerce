using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Features.RequestFeatures;
using Shopping.Web.Portal.Pages.Auth;
using Shopping.Web.Portal.Services.Interfaces;
using System.Security.Claims;


namespace Shopping.Web.Portal.Pages
{
    public class ProductsBase : ComponentBase
    {

        # region Fields and Properties

        public string CartCreatedValue { get; private set; }
        public string CartStringId { get; set; }
        public string UserClaimStringId { get; set; }


        public IEnumerable<ProductDto> Products { get; set; }
        public List<ProductDto> ProductDtoList { get; set; } = new List<ProductDto>();
        public List<CartItemDto> OldCollection { get; set; } = new List<CartItemDto>();
        public List<CartItemDto> OldList { get; set; } = new List<CartItemDto>();

        public MetaData MetaData { get; set; } = new MetaData();
        public QueryStringParameters queryStringParameters = new QueryStringParameters();

        public Shopping.Core.Entities.ShoppingCart ShoppingCart { get; set; } = new Core.Entities.ShoppingCart();

        #endregion

        #region Injected Services

        [Inject]
        AuthStateProvider AuthStateProvider { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
  

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
      
        #endregion

        protected async override Task OnInitializedAsync()
        {
            this.UserClaimStringId = await ManageCartItemsLocalStorageService.GetUserClaimStringId();   //??
            this.CartStringId = await ManageCartItemsLocalStorageService.GetCartStringId();


            ShoppingCart = new Core.Entities.ShoppingCart();

            await GetAuth();

            await GetProducts();

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

                    this.OldList = await ShoppingCartService.GetCartItems2(UserClaimStringId);
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
                        item.UserClaimStringId = UserClaimStringId;
                                            
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

                    await ShoppingCartService.CreateShoppingCart(ShoppingCart);

                 
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
              
        private async Task GetProducts()

        {
            var pagingResponse = await ProductService.GetProducts(queryStringParameters);
            ProductDtoList = pagingResponse.Items;
            MetaData = pagingResponse.MetaData;


            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            var totalQty = shoppingCartItems.Sum(i => i.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);

        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in ProductDtoList
                   group product by product.ProductCategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg =>
                        pg.ProductCategoryId == groupedProductDtos.Key).ProductCategoryName;
        }

        public async Task SelectedPage(int page)
        {
            queryStringParameters.PageNumber = page;
            await GetProducts();
        }
        
    }
}
