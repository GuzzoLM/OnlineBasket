namespace OnlineBasket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.Domain.DTO;

    [Authorize("Bearer")]
    [Route("api/{bid}/[controller]")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ProductGroupController(
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            IUserRepository userRepositor)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _userRepository = userRepositor;
        }

        /// <summary>
        /// Add, or change the number of products in the basket.
        /// </summary>
        /// <param name="bid">Unique identifier of the basket</param>
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
            var userName = User.Identity.Name;

            var userId = (await _userRepository.FindUser(userName))?.Id;

            if (!userId.HasValue)
                return StatusCode(401);

            try
            {
                var basket = await _basketRepository.Get(bid);

                if (basket.OwnerId != userId.Value)
                    return StatusCode(401);

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
        /// Remove products from the basket.
        /// </summary>
        /// <param name="bid">Unique identifier of the basket</param>
        /// <param name="id">Unique identifier of the product.</param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete("{id}", Name ="DeleteProduct")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid bid, Guid id)
        {
            var userName = User.Identity.Name;

            var userId = (await _userRepository.FindUser(userName))?.Id;

            if (!userId.HasValue)
                return StatusCode(401);

            try
            {
                var basket = await _basketRepository.Get(bid);

                if (basket.OwnerId != userId.Value)
                    return StatusCode(401);

                var product = await _productRepository.Get(id);
                var resultProduct = basket.CompletelyRemoveItem(product);
                await _productRepository.Update(id, resultProduct);
                await _basketRepository.Update(bid, basket);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove all the products from the basket.
        /// </summary>
        /// <param name="bid">Unique identifier of the basket</param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid bid)
        {
            var userName = User.Identity.Name;

            var userId = (await _userRepository.FindUser(userName))?.Id;

            if (!userId.HasValue)
                return StatusCode(401);

            try
            {
                var basket = await _basketRepository.Get(bid);

                if (basket.OwnerId != userId.Value)
                    return StatusCode(401);

                var productsReturned = basket.ClearBasket();

                foreach (var prodInfo in productsReturned)
                {
                    var product = await _productRepository.Get(prodInfo.Key);
                    product.Stock += prodInfo.Value;
                    await _productRepository.Update(product.Id, product);
                }

                await _basketRepository.Update(bid, basket);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}