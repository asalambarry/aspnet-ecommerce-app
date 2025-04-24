namespace ShopZone.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public List<TopProductViewModel> TopProducts { get; set; } = new();
        public Dictionary<string, int> OrdersByMonth { get; set; } = new();
        public Dictionary<string, decimal> RevenueByMonth { get; set; } = new();
    }

    public class TopProductViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}