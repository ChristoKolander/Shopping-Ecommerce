using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;

namespace Shopping.Web.Portal.Pages
{
    public class DisplayProductsBase: ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }

    }
}
