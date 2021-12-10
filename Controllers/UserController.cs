using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Karma.Services;
using Karma.Models.Authentication;
using Karma.Repositories;
using System.Collections.Generic;
using Karma.Models;
using System;
using System.Linq;

namespace Karma.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<ListingController> _logger;

        private readonly IUserService _userService;

        private readonly Lazy<IListingRepository> _listingRepository;

        public UserController(ILogger<ListingController> logger, Lazy<IListingRepository> listingRepository, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _listingRepository = listingRepository;
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

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult NewUser(User user){
            if (user != null){
                if(_userService.GetAll().Any(u => u.Username == user.Username))
                    return StatusCode(403);
            _userService.Add(user);
            return Ok();
            }
            else return StatusCode(500);
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            var userid = this.TryGetUserId();
            if (userid == null) {
                return NoContent();
            }
            return Ok(_userService.GetUserById((int)userid));
        }

        [HttpGet("getByListingId={id}")]
        public ActionResult<IEnumerable<User>> GetRequestedUsersOfListing(int id)
        {
            return Ok(_listingRepository.Value.GetAllRequestees(id));   
        }
        [Authorize]
        [HttpGet("listings")]
        [AllowAnonymous]
        [HttpGet("listings/{username}")]
        public ActionResult<IEnumerable<Listing>> GetAllListingsOfUser(string username = null)
        {
            int? userId;
            if (username == null) {
                userId = this.TryGetUserId();
            }
            else {
                userId = _userService.GetAll().FirstOrDefault(u => u.Username == username)?.Id;
            }
            if (userId == null)
                return Unauthorized();
            
            return Ok(_userService.GetAllUserListingsByUserId((int)userId));
        }
        [Authorize]
        [HttpGet("requested_listings")]
        public ActionResult<IEnumerable<Listing>> GetAllRequestedListingsOfUser()
        {
            var userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();
            
            var user = _userService.GetUserById((int) userId);
            if (user == null)
                return NotFound();
            
            return Ok(_userService.GetAllRequestedListingsByUserId((int)userId));
        }
        
    }
}