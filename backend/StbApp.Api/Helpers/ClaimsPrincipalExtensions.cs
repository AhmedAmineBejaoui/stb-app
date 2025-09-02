using System.Security.Claims;

namespace StbApp.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out var userId)) return userId;
            return null;
        }
    }
}

