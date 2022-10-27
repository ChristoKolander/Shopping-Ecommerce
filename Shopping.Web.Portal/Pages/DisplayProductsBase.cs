using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos;
using System.Collections.Generic;

namespace Shopping.Web.Portal.Pages
{
    public class DisplayProductsBase: ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }

    }
}
