namespace ShopZone.Services
{
    public interface IIdentityEvents
    {
        Task OnRegisteredUser(Microsoft.AspNetCore.Identity.IdentityUser user);
    }
}