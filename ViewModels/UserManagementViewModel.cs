using System.ComponentModel.DataAnnotations;

namespace ShopZone.ViewModels
{
    public class UserManagementViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }  // Ajout de cette propriété
        public bool IsLocked { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public int OrderCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}