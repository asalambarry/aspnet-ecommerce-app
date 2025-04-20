using Microsoft.AspNetCore.Identity;

namespace ShopZone.Services
{
    public class CustomIdentityEvents : IIdentityEvents
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CustomIdentityEvents(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnRegisteredUser(IdentityUser user)
        {
            await _userManager.AddToRoleAsync(user, "Client");
        }
    }
}