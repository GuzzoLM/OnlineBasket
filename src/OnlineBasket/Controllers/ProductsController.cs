namespace OnlineBasket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.DTO;
    using OnlineBasket.DTO.TypeAdapters;

    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Search for products filtered by given parameters
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="price">The price of the item</param>
        /// <param name="stock">The number of available items in stock</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            [FromQuery] string name = null,
            [FromQuery] decimal? price = null,
            [FromQuery] int? stock = null)
        {
            var products = await _productRepository.GetItems(name, price, stock);

            return Ok(products);
        }

        /// <summary>
        /// Get specific product by Id
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productRepository.Get(id);

            return Ok(product);
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="newProduct">The new product to be added</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] Product newProduct)
        {
            var successGuid = await _productRepository.Create(newProduct.ToModel());

            return CreatedAtAction(nameof(Get), new { id = successGuid }, successGuid);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Unique identifier of the product to be updated</param>
        /// <param name="product">The updated product</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Product product)
        {
            await _productRepository.Update(id, product.ToModel());

            return NoContent();
        }

        /// <summary>
        /// Delete an existing product
        /// </summary>
        /// <param name="id">Unique identifier of the product to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _productRepository.Delete(id);

            return NoContent();
        }
    }
}