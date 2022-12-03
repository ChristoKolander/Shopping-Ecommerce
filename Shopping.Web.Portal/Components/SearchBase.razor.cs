using Microsoft.AspNetCore.Components;

namespace Shopping.Web.Portal.Components
{
    public class SearchBase: ComponentBase
    {
        public string SearchTerm { get; set; }

        [Parameter]
        public EventCallback<string> OnSearchChanged { get; set; }

    }
}
