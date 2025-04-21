using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopZone.Data;
using ShopZone.Models;

namespace ShopZone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Liste toutes les commandes
        public async Task<IActionResult> Index(string status = "")
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .AsQueryable();

            // Filtre par statut si spécifié
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status == status);
            }

            ViewBag.CurrentStatus = status;
            ViewBag.Statuses = Enum.GetNames(typeof(OrderStatus));

            return View(await orders.OrderByDescending(o => o.OrderDate).ToListAsync());
        }

        // Détails d'une commande
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Statuses = Enum.GetNames(typeof(OrderStatus));
            return View(order);
        }

        // Mise à jour du statut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            if (Enum.TryParse<OrderStatus>(status, out OrderStatus newStatus))
            {
                order.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Order #{id} status updated to {status}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}