using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopZone.Filters;
using ShopZone.Services;

namespace ShopZone.Filters
{
    public class AdminAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly RoleService _roleService;

        public AdminAuthorizationFilter(RoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToPageResult("/Account/Login", new { area = "Identity" });
                return;
            }

            if (!await _roleService.IsAdminAsync(user.FindFirst(ClaimTypes.NameIdentifier)?.Value))
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public class AdminOnlyAttribute : TypeFilterAttribute
    {
        public AdminOnlyAttribute() : base(typeof(AdminAuthorizationFilter))
        {
        }
    }
}

// Exemple d'utilisation sur un contrôleur admin
[AdminOnly]
public class AdminController : Controller
{
    // ... actions du contrôleur
}