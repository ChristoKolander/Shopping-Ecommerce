using Microsoft.AspNetCore.Components;
using Shopping.Web.Features;
using Shopping.Web.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Web.Components
{
    public class PaginationBase: ComponentBase
    {
        [Parameter]
        public MetaData MetaData { get; set; }
        [Parameter]
        public int Spread { get; set; }
        [Parameter]
        public EventCallback<int> SelectedPage { get; set; }

        public List<PagingLink> links;

        protected override void OnParametersSet()
        {
            CreatePaginationLinks();
        }

       public void CreatePaginationLinks()
        {
            links = new List<PagingLink>();
            links.Add(new PagingLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, "Previous"));

            for (int i = 1; i <= MetaData.TotalPages; i++)
            {
                if (i >= MetaData.CurrentPage - Spread && i <= MetaData.CurrentPage + Spread)
                {
                    links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.CurrentPage == i });
                }
            }
            
            links.Add(new PagingLink(MetaData.CurrentPage + 1, MetaData.HasNext, "Next"));
        }


        public async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == MetaData.CurrentPage || !link.Enabled)
                return;
            MetaData.CurrentPage = link.Page;
            await SelectedPage.InvokeAsync(link.Page);
        }

    }
}
