using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Models;
using Karma.Repositories;
using Karma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GardenController : Controller
    {
        private readonly IGardenRepository _gardenRepository;
        private readonly IUserService _userService;
        private readonly IListingRepository _listingRepository;

        public GardenController(IGardenRepository gardenRepository, IListingRepository listingRepository, IUserService userService)
        {
            _gardenRepository = gardenRepository;
            _listingRepository = listingRepository;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("x={x}/z={z}/plant={plant}/listingId={listingId}")]
        public async Task<ActionResult> AddPlant(int x, int z, string plant, int listingId)
        {
            var userId = this.TryGetUserId();
            if (!userId.HasValue)
                return Unauthorized();
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (userId != listing.UserId)
                return Unauthorized();
            var garden = _gardenRepository.GetByUserId((int) userId);
            if (garden == null)
                return NotFound();
            garden.Plants[x][z] = $"Id={listingId}/{plant}";
            await _gardenRepository.UpdateAsync(garden);
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("{username}")]
        public ActionResult<List<List<string>>> GetGarden(string username)
        {
            var user = _userService.GetAll().FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound();
            var garden = _gardenRepository.GetByUserId(user.Id);
            if (garden == null)
                return NotFound();
            return Ok(garden.Plants);
        }
    }
}
