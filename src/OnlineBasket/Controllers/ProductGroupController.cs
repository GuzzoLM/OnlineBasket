namespace OnlineBasket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.Domain.DTO;

    [Route("api/{bid}/[controller]")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;

        public ProductGroupController(IBasketRepository basketRepository, IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="productGroup"></param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        /// <response code="404">Item was not found</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(Guid bid, [FromBody] ProductGroupDTO productGroup)
        {
            try
            {
                var basket = await _basketRepository.Get(bid);
                var product = await _productRepository.Get(productGroup.ProductId);
                var resultProduct = basket.UpsertItem(product, productGroup.Quantity);
                await _productRepository.Update(productGroup.ProductId, resultProduct);
                await _basketRepository.Update(bid, basket);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid bid, Guid? id)
        {
            try
            {
                var basket = await _basketRepository.Get(bid);
                if (!id.HasValue)
                {
                    basket.ClearBasket();
                }
                else
                {
                    var product = await _productRepository.Get(id.Value);
                    var resultProduct = basket.CompletelyRemoveItem(product);
                    await _productRepository.Update(id.Value, resultProduct);
                    await _basketRepository.Update(bid, basket);
                }

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}