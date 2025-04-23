using ShopZone.Models;

namespace ShopZone.ViewModels
{
    public class UserDetailsViewModel : UserViewModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}