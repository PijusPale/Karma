using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Karma.Services;
using Karma.Models.Authentication;
using System.Security.Claims;
using Karma.Repositories;
using System.Collections.Generic;
using Karma.Models;
using System.Linq;
using System;

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

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            string userid = this.TryGetUserId();
            if (userid == null) {
                return NoContent();
            }
            return Ok(_userService.GetUserById(int.Parse(userid)));
        }

        [HttpGet("getByListingId={id}")]
        public IActionResult GetRequestedUsersOfListing(string id)
        {
            if(id == null)
                return NoContent();

            var listing = _listingRepository.Value.GetById(int.Parse(id));
            var listOfUsers = new List<User>();

            foreach(string requesteeId in listing.RequestedUserIDs)
            {
                listOfUsers.Add(_userService.GetUserById(int.Parse(requesteeId)));
            }

            return Ok(listOfUsers);   
        }
    }
}