using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Core.Entities;
using Shopping.Core.Entities.RequestFeatures;
using Shopping.Core.Interfaces;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        #region Fields and CTOR

        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public ProductController(IProductRepository productRepository, IMapper mapper, ILoggerManager logger)
        {
         
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        #endregion


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {

            var product = await productRepository.GetProduct(id);

            if (product == null)
            {
                logger.LogError("GetProduct; could not get product");
                return BadRequest();
            }


            var productDto = product.ConvertToDto();

            return Ok(productDto);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProductsWithParams([FromQuery] QueryStringParameters queryStringParameters)
        {

            var products = await productRepository.GetProductsWithParams(queryStringParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("GetProductsWithParams; could not get products");
                return NotFound();
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);

        }

        [HttpGet("Filter")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsFiltered([FromQuery] ProductParameters productParameters)
        {
            if (!productParameters.ValidPriceRange)
            {
                return BadRequest("MaxPrice must be larger than MinPrice");
            }

            var products = await productRepository.GetProductsFiltered(productParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("GetProductsFiltered; could not get products");
                return NotFound();
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);


        }


        [HttpPost]
        [Authorize(Policy = "AdminRolePolicy")]
        public async Task<ActionResult<ProductCreateDto>> CreateProduct(ProductCreateDto productCreateDto)
        {

            if (productCreateDto == null)
            {
                logger.LogError("CreateProduct; could not create product because it is null");
                return BadRequest("Product object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("CreateProduct; could not create product because modelstate invalid");
                return UnprocessableEntity(ModelState);
            }

            var productEntity = mapper.Map<Product>(productCreateDto);

            var createdProduct = await productRepository.CreateProduct(productEntity);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);

        }


        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Administrators, Managers")]
        public async Task<ActionResult<ProductUpdateDto>> UpdateProduct(ProductUpdateDto productUpdateDto)
        {

            if (productUpdateDto is null)
            {
                logger.LogError("UpdateProduct; could not create product because it is null");
                return BadRequest("Product object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("UpdateProduct; could not update product because modelstate invalid");
                return BadRequest("Invalid model object");
            }


            var productEntity = await productRepository.GetProduct(productUpdateDto.Id);

            if (productEntity == null)
            {
                logger.LogError("UpdateProduct; could not update product because it was not found");
                return NotFound($"Product with Id = {productEntity.Id} NOT found.");
            }

            mapper.Map(productUpdateDto, productEntity);

            await productRepository.UpdateProduct(productEntity);

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminRolePolicy")]
        public async Task<ActionResult<ProductUpdateDto>> DeleteProduct(int id)
        {

            var productToDelete = await productRepository.GetProduct(id);

            if (productToDelete == null)
            {
                logger.LogError("Delete Product; could not find product");
                return NotFound($"Product with Id = {id} not found");
            }

            await productRepository.DeleteProduct(id);

            return Ok(productToDelete);

        }

       
        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {

            var products = await productRepository.GetProductsByCategory(categoryId);

            if (products == null)
            {
                logger.LogError("GetItemsByCategory; could not find products");
                return NotFound();
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);

        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {

            var productCategories = await productRepository.GetCategories();

            if (productCategories == null)
            {
                logger.LogError("GetProductCategories; could not find productCategories");
                return NotFound();
            }

            var productCategoryDtos = productCategories.ConvertToDto();

            return Ok(productCategoryDtos);

        }


        //[HttpGet]
        //public async Task<ActionResult<List<ProductDto>>> GetProducts()
        //{

        //    var products = await productRepository.GetProducts();

        //    if (products == null)
        //    {
        //        return NotFound();
        //    }          

        //    var productDtos = products.ConvertToDto();

        //    return Ok(productDtos);

        //}



        //[HttpGet]
        //[Route(nameof(Search))]
        //public async Task<IActionResult> Search([FromQuery] SearchParameters searchParameters)
        //{
        //    var products = await productRepository.Search(searchParameters);

        //    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

        //    var productDtos = products.ConvertToDto();

        //    return Ok(productDtos);

        //}
    }


}


