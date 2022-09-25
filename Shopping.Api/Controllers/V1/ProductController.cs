using Microsoft.AspNetCore.Mvc;
using Shopping.Api.Repositories.Interfaces;
using Shopping.Models.Dtos.CRUDs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Api.Extensions;
using Shopping.Api.LoggerService;
using Shopping.Api.Entities;
using Newtonsoft.Json;
using Shopping.Api.Entities.RequestFeatures;
using AutoMapper;
using Shopping.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        #region Fields and CTOR
        private readonly IProductRepository productRepository;
        private readonly ILoggerManager logger;
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly ShoppingDbContext shoppingDbContext;

        public ProductController(IProductRepository productRepository,
                                 ILoggerManager logger,
                                 IRepositoryWrapper repository,
                                 IMapper mapper,
                                 ShoppingDbContext shoppingDbContext)
        {
            this.productRepository = productRepository;
            this.logger = logger;
            this.repository = repository;
            this.mapper = mapper;
            this.shoppingDbContext = shoppingDbContext;
        }

        #endregion


        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProductsWithParams([FromQuery] QueryStringParameters queryStringParameters)
        {

            var products = await repository.Product.GetProductsWithParams(queryStringParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("Products could not be found in database");
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

            var products = await repository.Product.GetProductsFiltered(productParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            if (products == null)
            {
                logger.LogError("Products could not be found in database");
                return NotFound();
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);


        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {

            var product = await repository.Product.GetProduct(id);

            if (product == null)
            {

                logger.LogError($"Product whit id: {id} could not be found in database");
                return BadRequest();
            }


            var productDto = product.ConvertToDto();

            return Ok(productDto);
        }


        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {

            var productCategories = await repository.Product.GetCategories();

            if (productCategories == null)
            {
                logger.LogError("ProductCategories could not be found in database");
                return NotFound();
            }

            var productCategoryDtos = productCategories.ConvertToDto();

            return Ok(productCategoryDtos);

        }


        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {

            var products = await productRepository.GetProductsByCategory(categoryId);

            if (products == null)
            {
                logger.LogError("Products could not be found in database");
                return NotFound();
            }

            var productDtos = products.ConvertToDto();

            return Ok(productDtos);

        }


        [HttpPost]
        [Authorize(Policy ="AdminRolePolicy")]
        public async Task<ActionResult<ProductCreateDto>> CreateProduct(ProductCreateDto productCreateDto)
        {

            if (productCreateDto == null)
            {
                logger.LogError("Product object sent from client is null.");
                return BadRequest("Product object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Product object sent from client.");
                return UnprocessableEntity(ModelState);
            }

            var productEntity = mapper.Map<Product>(productCreateDto);

            var createdProduct = await productRepository.CreateProduct(productEntity);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);

        }


        [HttpPatch("{id:int}")]
        [Authorize(Roles ="Administrator, Manager")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(ProductDto productUpdateDto)
        {

            if (productUpdateDto is null)
            {
                logger.LogError("Product object is null.");
                return BadRequest("Product object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Product object sent from client.");
                return BadRequest("Invalid model object");
            }


            var productEntity = await productRepository.GetProduct(productUpdateDto.Id);

            if (productEntity == null)
            {
                logger.LogError($"Product with id: {productEntity.Id} has not been found in DB.");
                return NotFound($"Product with Id = {productEntity.Id} NOT found.");
            }

                mapper.Map(productUpdateDto,productEntity);

                await productRepository.UpdateProduct(productEntity);

                return NoContent();

        }


        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminRolePolicy")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {

            var productToDelete = await productRepository.GetProduct(id);

            if (productToDelete == null)
            {
                return NotFound($"Product with Id = {id} not found");
            }

               await productRepository.DeleteProduct(id);

            return Ok(productToDelete);

        }


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


