using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Web.Pages
{
    public class CheckoutBase : ComponentBase
    {
        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }

        protected decimal PaymentAmount { get; set; }

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }


        protected string DisplayButtons { get; set; } = "block";

        protected override async Task OnInitializedAsync()
        {

            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

            if (ShoppingCartItems != null && ShoppingCartItems.Count() > 0)
            {
                Guid orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";

            }
            else
            {
                DisplayButtons = "none";
            }

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("initPayPalButton");
            }
        }
    }          
}