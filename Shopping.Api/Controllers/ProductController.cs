using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Core.Entities;
using Shopping.Shared.Entities.RequestFeatures;
using Shopping.Core.Interfaces;
using Shopping.Infrastructure.GenericRepositoriesRemake;
using Shopping.Api.Conversions;

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
                return NotFound($"Product with Id = {id} not found");
            }


            var productDto = product.ConvertToDto();

            return Ok(productDto);
        }

        [HttpGet("Products")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {

            var products = await productRepository.GetProducts();

            if (products == null)
            {
                logger.LogError("GetProducts; could not get products");
                return NotFound("Products could not be found");
            }

            else 
            {
             var productDtos = products.ConvertToDto();

            return Ok(productDtos);
            }
           

        }


        [HttpGet("AllProducts")]
        public async Task<ActionResult<ProductDto>> GetProductsWithParams([FromQuery] QueryStringParameters queryStringParameters)
        {

            var products = await productRepository.GetProductsWithParams(queryStringParameters);

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("GetProductsWithParams; could not get products");
                return NotFound("Products could not be found");
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);

        }

        [HttpGet("FilterByPrice")]
        public async Task<ActionResult<ProductDto>> GetProductsFilteredByPrice([FromQuery] ProductParameters productParameters)
        {
            if (!productParameters.ValidPriceRange)
            {
                return BadRequest("MaxPrice must be larger than MinPrice");
            }

            var products = await productRepository.GetProductsFilteredByPrice(productParameters);

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("GetProductsFiltered; could not get products");
                return NotFound("Products filtered by Price could not be found");
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

            //var createdProduct =  await productRepository.CreateProduct(productEntity);

            Product createdProduct2 = await productRepository.AddAsync(productEntity);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct2.Id }, createdProduct2);

            // Too much notes.
        }


        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult<ProductUpdateDto>> UpdateProduct(ProductUpdateDto productUpdateDto)
        {

            if (productUpdateDto is null)
            {
                logger.LogError("UpdateProduct; could not update product because it is null");
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

            await productRepository.UpdateAsync(productEntity, productEntity.Id);

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminRolePolicy")]
        public async Task<ActionResult<ProductUpdateDto>> DeleteProduct(int id)
        {

            var productToDelete = await productRepository.GetProduct(id);

            if (productToDelete == null)
            {
                logger.LogError("Delete Product; could not find product to delete");
                return NotFound($"Product with Id = {id} not found");
            }

            await productRepository.DeleteAsync(id);

            //await productRepository.DeleteProduct(id);

            return Ok(productToDelete);

        }

       
        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<ProductDto>> GetItemsByCategory(int categoryId)
        {

            var products = await productRepository.GetProductsByCategory(categoryId);

            if (products == null)
            {
                logger.LogError("GetItemsByCategory; could not find products for this Category");
                return NotFound("Products in this Category could not be found");
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);

        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<ProductCategoryDto>> GetProductCategories()
        {

            var productCategories = await productRepository.GetCategories();

            if (productCategories == null)
            {
                logger.LogError("GetProductCategories; could not find product categories");
                return NotFound("Product Category could not be found, or does not exist");
            }

            var productCategoryDtos = productCategories.ConvertToDto();

            return Ok(productCategoryDtos);

        }

        [HttpGet("{Name}/searchbyname")]
        public async Task<ActionResult<APIListOfEntityResponse<Product>>> SearchByName(string Name)
        {
            var result = await productRepository.Search(x => x.Name.ToLower().Contains(Name.ToLower()), null, "ProductCategory");

            if (result != null && result.Count() > 0)
            {
                return Ok(new APIListOfEntityResponse<Product>()
                {
                    Success = true,
                    Data = result
                });
            }
            else
            {
                return Ok(new APIEntityResponse<Product>()
                {
                    Success = false,
                    ErrorMessages = new List<string>() { "SearchByName;  No searchresults found" },
                    Data = null
                });
            }

        }

    }


}


