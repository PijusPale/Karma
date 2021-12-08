using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Karma.Controllers
{
    public static class ControllerExtensions
    {
        /// <summary>Returns user's id, if user is authenticated or null.</summary>
        public static int? TryGetUserId(this ControllerBase controller)
        {
            int res = 0;
            if (int.TryParse(controller.User.FindFirst(ClaimTypes.Name)?.Value, out res))
                return res;
            else 
                return null;
        }
    }
}