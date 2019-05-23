namespace OnlineBasket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.DataAccess.Services;
    using OnlineBasket.Domain.DTO;
    using OnlineBasket.Domain.DTO.TypeAdapters;
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Domain.Model;

    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;

        public BasketController(IBasketRepository basketRepository, IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Search for all baskets wich the user is owner.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <response code="200">Returns found items</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<BasketDTO>>> GetBaskets(
            [FromServices] IUserRepository userRepository,
            [FromQuery] BasketStatus? status = null)
        {
            var userName = User.Identity.Name;

            var ownerId = (await userRepository.FindUser(userName))?.Id;

            if (!ownerId.HasValue)
                return StatusCode(401);

            var baskets = await _basketRepository.GetItems(ownerId: ownerId, status: status);
            return Ok(baskets.Select(x => x.ToDTO(userName)));
        }

        /// <summary>
        /// Get a specific basket. If user is not the owner, access is denied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBasket")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BasketDTO>> GetBasket([FromServices] IUserRepository userRepository, Guid id)
        {
            var userName = User.Identity.Name;

            var ownerId = (await userRepository.FindUser(userName))?.Id;

            if (!ownerId.HasValue)
                return StatusCode(401);

            try
            {
                var basket = await _basketRepository.Get(id);

                if (basket.OwnerId != ownerId.Value)
                    return StatusCode(401);

                return Ok(basket.ToDTO(userName));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a basket for the user. If an open basket already exists gives error 400
        /// </summary>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        /// <response code="201">Item successfully created</response>
        /// <response code="400">Provided item is not valid</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Guid>> Post([FromServices] IUserRepository userRepository)
        {
            var userName = User.Identity.Name;

            var userId = (await userRepository.FindUser(userName))?.Id;

            if (!userId.HasValue)
                return StatusCode(401);

            var existentBaskets = await _basketRepository.GetItems(userId, BasketStatus.Open);

            if (existentBaskets.Count > 0)
            {
                return BadRequest();
            }

            var newBasket = new Basket(userId.Value);
            var resultId = await _basketRepository.Create(newBasket);

            return Ok(resultId);
        }

        /// <summary>
        /// Delete a specific basket. If the user is not the owner return access denied instead.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Item successfully updated</response>
        /// <response code="401">Unauthorized request. Please log in.</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete([FromServices] IUserRepository userRepository, Guid id)
        {
            var userName = User.Identity.Name;

            var userId = (await userRepository.FindUser(userName))?.Id;

            if (!userId.HasValue)
                return StatusCode(401);

            try
            {
                await _basketRepository.Delete(userId.Value, id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}