using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Shopping.Web.Components
{
    public class ServerValidationsBase: ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool Result { get; set; }

        [Parameter]
        public bool Delete{ get; set; }

        [Parameter]
        public bool Edit { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string ReturnUrl { get; set; } = "/";

        protected override async Task OnParametersSetAsync()
        {
       
            await base.OnParametersSetAsync();
        }

        public void CloseValidation()
        {
            IsVisible = false;
        }
    }
}
