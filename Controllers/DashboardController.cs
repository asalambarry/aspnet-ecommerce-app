using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopZone.Data;
using ShopZone.ViewModels;

namespace ShopZone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Récupérer les données sans tri
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            // Faire le groupement et le tri en mémoire
            var topProducts = orderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new TopProductViewModel
                {
                    Name = g.First().Product.Name,
                    Category = g.First().Product.Category.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(5)
                .ToList();

            var orders = await _context.Orders.ToListAsync();
            var ordersByMonth = orders
                .GroupBy(o => o.OrderDate.ToString("MMMM"))
                .ToDictionary(g => g.Key, g => g.Count());

            var revenueByMonth = orderItems
                .GroupBy(oi => oi.Order.OrderDate.ToString("MMMM"))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(oi => oi.Price * oi.Quantity)
                );

            var viewModel = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.OrderItems.SumAsync(oi => oi.Price * oi.Quantity),
                TotalProducts = await _context.Products.CountAsync(),
                TotalCustomers = await _context.Users.CountAsync(),
                TopProducts = topProducts,
                OrdersByMonth = ordersByMonth,
                RevenueByMonth = revenueByMonth
            };

            return View(viewModel);
        }
    }
}