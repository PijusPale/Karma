using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Karma.Services;
using Karma.Models.Authentication;
using System.Security.Claims;

namespace Karma.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<ListingController> _logger;

        private readonly IUserService _userService;

        public UserController(ILogger<ListingController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentials request)
        {
            var user = _userService.Authenticate(request);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            string userid = this.TryGetUserId();
            if (userid == null) {
                return NoContent();
            }
            return Ok(_userService.GetUserById(userid));
        }
    }
}