using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Features.RequestFeatures;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shopping.Web.Pages
{
    public class ProductsBase: ComponentBase
    {

        public IEnumerable<ProductDto> Products { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
     
        [Inject]
        public NavigationManager NavigationManager { get; set; }
  

        public List<ProductDto> ProductDtoList { get; set; } = new List<ProductDto>();
        public MetaData MetaData { get; set; } = new MetaData();
        public QueryStringParameters queryStringParameters = new QueryStringParameters();


        protected async override Task OnInitializedAsync()
        {
            await GetProducts();
            
        }

        private async Task GetProducts()

        {
            var pagingResponse = await ProductService.GetProducts(queryStringParameters);
            ProductDtoList = pagingResponse.Items;
            MetaData = pagingResponse.MetaData;

            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            var totalQty = shoppingCartItems.Sum(i => i.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);

        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in ProductDtoList
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg =>
                        pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }

        public async Task SelectedPage(int page)
        {
            queryStringParameters.PageNumber = page;
            await GetProducts();
        }

    }
}
