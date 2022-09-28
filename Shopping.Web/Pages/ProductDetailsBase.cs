using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Features.RequestFeatures;
using Shopping.Web.Pages.Auth;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shopping.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {

        [Parameter]
        public int Id { get; set; }

        public ProductDto Product { get; set; }

        private List<CartItemDto> ShoppingCartItems { get; set; }


        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }


        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }


        protected override async Task OnInitializedAsync()
        {

            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            Product = await ProductService.GetProduct(Id);
 
        }


        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {

            var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

            if (cartItemDto != null)
            {

                ShoppingCartItems.Add(cartItemDto);
                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");

        }       
    }
}
