using Blazored.LocalStorage;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {

        #region Fields and CTOR

        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;
        CancellationToken cancellationToken = new CancellationToken();

        const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService,
                                                  IShoppingCartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }


        #endregion

        public async Task<List<CartItemDto>> GetCollection()
        {
            return await localStorageService.GetItemAsync<List<CartItemDto>>(key, cancellationToken)
                   ?? await AddCollection();
        }
        private async Task<List<CartItemDto>> AddCollection()
        {          
            var shoppingCartCollection = await shoppingCartService.GetItems(HardCoded.UserId);

            if (shoppingCartCollection != null)
            {
                await localStorageService.SetItemAsync(key, shoppingCartCollection);
            }

            return shoppingCartCollection;
        }
        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await localStorageService.SetItemAsync(key, cartItemDtos);

        }
        public async Task RemoveCollection()
        {
            await localStorageService.RemoveItemAsync(key);
        }
      
    }
}
