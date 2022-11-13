using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;

namespace Shopping.Web.Portal.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {

        #region Fields and CTOR

        private readonly ILocalStorageService localStorageService;
        private readonly ICartService shoppingCartService;
        private readonly CancellationToken cancellationToken = new CancellationToken();

        public string KeyCartCreated = "CartCreatedValue";
        public string Key = "CKCartItemCollection";
        public string KeyCart = "CartStringId";
        public string KeyUser = "UserClaimStringId";



        [Parameter]
        public string UserClaimStringId { get; set; }

        [Parameter]
        public string CartStringId { get; set; }

        [Parameter]
        public string CartCreatedValue { get; set; }


        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService,
                                                  ICartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }


        #endregion

        # region Cart

        public async Task<string> GetCartCreatedValue()
        {

            return await localStorageService.GetItemAsync<string>(KeyCartCreated, cancellationToken);

        }

        public async Task AddCartCreatedValue(string CartCreatedValue)
        {

            await this.localStorageService.SetItemAsync(KeyCartCreated, CartCreatedValue);

        }

        # endregion

        # region Collection

        public async Task<List<CartItemDto>> GetCollection()
        {
                
            return await localStorageService.GetItemAsync<List<CartItemDto>>(Key, cancellationToken)
                   ?? await AddCollection();
        }     
       
        private async Task<List<CartItemDto>> AddCollection()
        {

            if (UserClaimStringId == null)
            {
                this.UserClaimStringId = await GetUserClaimStringId();
            }

            var shoppingCartCollection = await this.shoppingCartService.GetCartItems(this.UserClaimStringId);
            if (shoppingCartCollection != null)
            {
                await this.localStorageService.SetItemAsync(Key, shoppingCartCollection);
            }

            return shoppingCartCollection;

        }        
      
        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            
            await localStorageService.SetItemAsync(Key, cartItemDtos);

        }

        public async Task RemoveCollection()
        {
            await localStorageService.RemoveItemAsync(Key);
        }

        # endregion

        # region UserClaimStringId

        public async Task SaveUserClaimStringId(string userClaimStringId)
        {

            await localStorageService.SetItemAsync(KeyUser, userClaimStringId);
     
        } 
       
        public async Task<string> GetUserClaimStringId()
        {

            return await localStorageService.GetItemAsync<string>(KeyUser, cancellationToken);
                  
        }

        public async Task RemoveUserClaimStringId()
        {

            await localStorageService.RemoveItemAsync(KeyUser);

        }

        # endregion

        # region CartStringId

        public async Task SaveCartStringId(string cartStringId)
        {

            await localStorageService.SetItemAsync(KeyCart, cartStringId);

        }

        public async Task RemoveCartStringId()
        {
            await localStorageService.RemoveItemAsync(KeyCart);

        }

        public async Task<string> GetCartStringId()
        {

            return await localStorageService.GetItemAsync<string>(KeyCart, cancellationToken);

        }

        # endregion

        #region NotUsedAtTheMoment..

        public async Task<string> AddUserClaimStringId()
        {

            await localStorageService.SetItemAsync(KeyUser, UserClaimStringId);

            return UserClaimStringId;

        }

      


        # endregion
    }

}

