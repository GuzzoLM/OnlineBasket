using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineBasket.DTO;

namespace OnlineBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            [FromQuery] string name = null,
            [FromQuery] decimal? price = null,
            [FromQuery] int? stock = null)
        {
            var products = await Task.FromResult(new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shirt",
                    Price = 100,
                    Stock = 15
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shoe",
                    Price = 200,
                    Stock = 15
                }
            });

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await Task.FromResult(new Product
            {
                Id = id,
                Name = "Shirt",
                Price = 100,
                Stock = 15
            });

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] Product newProduct)
        {
            var successGuid = await Task.FromResult(Guid.NewGuid());

            return CreatedAtAction(nameof(Get), new { id = successGuid }, successGuid);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] string value)
        {
            var success = await Task.FromResult(true);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await Task.FromResult(true);

            return NoContent();
        }
    }
}