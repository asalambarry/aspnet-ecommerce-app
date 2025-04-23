namespace ShopZone.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public int OrderCount { get; set; }
        public bool IsLocked { get; set; }
    }
}