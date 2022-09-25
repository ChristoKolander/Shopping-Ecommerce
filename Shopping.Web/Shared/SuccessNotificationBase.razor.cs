using Microsoft.AspNetCore.Components;

namespace Shopping.Web.Shared
{
    public class SuccessNotificationBase: ComponentBase
    {
        public string modalDisplay;
        public string modalClass;
        public bool showBackdrop;

        [Inject]
        public NavigationManager Navigation { get; set; }

        public void Show()
        {
            modalDisplay = "block;";
            modalClass = "show";
            showBackdrop = true;
            StateHasChanged();
        }

        public void Hide()
        {
            modalDisplay = "none;";
            modalClass = "";
            showBackdrop = false;
            StateHasChanged();
            Navigation.NavigateTo("/products");
        }
    }
}
