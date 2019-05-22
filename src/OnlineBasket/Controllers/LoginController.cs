﻿namespace OnlineBasket.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineBasket.Domain.Access;
    using OnlineBasket.Security.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<object>> PostAsync([FromServices] IAuthenticationService authenticationService)
        {
            Request.Form.TryGetValue("username", out var username);
            Request.Form.TryGetValue("password", out var password);

            var user = new User
            {
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