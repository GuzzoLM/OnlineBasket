namespace OnlineBasket.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.Domain.Access;
    using OnlineBasket.Security.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<object>> PostAsync([FromServices] IAuthenticationService authenticationService)
        {
            Request.Form.TryGetValue("username", out var username);
            Request.Form.TryGetValue("password", out var password);

            // This is hurting my eyes, but I will focus on most important things first
            var user = new User
            {
                Id = Guid.Empty,
                UserName = username,
                Password = password
            };

            var authentication = await authenticationService.Authenticate(user);

            if (authentication.Authenticated)
                return Ok(authentication.Token);

            return Unauthorized();
        }
    }
}