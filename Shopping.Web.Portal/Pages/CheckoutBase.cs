using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shopping.Web.Portal.Pages
{
    public class CheckoutBase : ComponentBase
    {

        [Parameter]
        public string userClaimStringId { get; set; }

        ClaimsPrincipal User { get; set; }

        public int TotalQty { get; set; }

        public string PaymentDescription { get; set; }

        public decimal PaymentAmount { get; set; }

        public string DisplayButtons { get; set; } = "block";

        public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;
        

        protected override async Task OnInitializedAsync()
        {
            await GetUserClaimId();

            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
       

            if (ShoppingCartItems != null && ShoppingCartItems.Count() > 0)
            {
                Guid orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                PaymentDescription = $"O_{userClaimStringId}_{orderGuid}";

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

        protected async Task GetUserClaimId()
        {
            var authState = await AuthenticationStateTask;
            var User = authState.User;
            if (User.Identity.IsAuthenticated)
            {
                var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
                userClaimStringId = userId;
            }
        }
    }
}