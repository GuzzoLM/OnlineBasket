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
        /// <response code="200">Returns found items</response>
        [HttpGet]
        [ProducesResponseType(200)]
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
        /// <response code="200">Returns found item</response>
        /// <response code="404">Item was not found</response>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            try
            {
                var product = await _productRepository.Get(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="newProduct">The new product to be added</param>
        /// <returns></returns>
        /// <response code="201">Item successfully created</response>
        /// <response code="400">Item already exists</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Guid>> Post([FromBody] Product newProduct)
        {
            try
            {
                var successGuid = await _productRepository.Create(newProduct.ToModel());
                return CreatedAtAction(nameof(Get), new { id = successGuid }, successGuid);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Unique identifier of the product to be updated</param>
        /// <param name="product">The updated product</param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="404">Item was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(Guid id, [FromBody] Product product)
        {
            try
            {
                await _productRepository.Update(id, product.ToModel());

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete an existing product
        /// </summary>
        /// <param name="id">Unique identifier of the product to be deleted</param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _productRepository.Delete(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}