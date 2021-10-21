using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Karma.Controllers
{
    public static class ControllerExtensions
    {
        /// <summary>Returns user's id, if user is authenticated or null.</summary>
        public static string TryGetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}