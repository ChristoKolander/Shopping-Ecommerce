using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Features.RequestFeatures;
using Shopping.Web.Portal.Services.Interfaces;



namespace Shopping.Web.Portal.Pages
{
    public class ProductsBase : ComponentBase
    {
     
        public List<ProductDto> ProductDtoList { get; set; } = new List<ProductDto>();
        public MetaData MetaData { get; set; } = new MetaData();
        public QueryStringParameters queryStringParameters = new QueryStringParameters();
     

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public ICartService ShoppingCartService { get; set; }
  
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
  
      

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
                   group product by product.ProductCategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg =>
                        pg.ProductCategoryId == groupedProductDtos.Key).ProductCategoryName;
        }

        public async Task SelectedPage(int page)
        {
            queryStringParameters.PageNumber = page;
            await GetProducts();
        }
        
    }
}
