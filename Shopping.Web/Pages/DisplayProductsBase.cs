using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.CRUDs;
using System.Collections.Generic;

namespace Shopping.Web.Pages
{
    public class DisplayProductsBase: ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }

    }
}
