using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopZone.Data;
using ShopZone.Models;

namespace ShopZone.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Checkout()
        {
            var cartId = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.CartItems = cartItems;
            ViewBag.Total = cartItems.Sum(item => item.Product.Price * item.Quantity);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            var order = new Order
            {
                UserId = user?.Id ?? "",
                CustomerName = user?.UserName ?? "",
                Email = user?.Email ?? ""
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("CustomerName,Email,Phone,Address,City,PostalCode")] Order order)
        {
            if (ModelState.IsValid)
            {
                var cartId = HttpContext.Session.GetString("CartId");
                var cartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.CartId == cartId)
                    .ToListAsync();

                order.UserId = User.Identity?.Name ?? "";
                order.OrderDate = DateTime.Now;
                order.Total = cartItems.Sum(item => item.Product.Price * item.Quantity);
                order.Status = "Pending";

                foreach (var item in cartItems)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });
                }

                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("CartId");
                TempData["Success"] = "Your order has been placed successfully!";

                return RedirectToAction(nameof(OrderConfirmation), new { id = order.Id });
            }

            // Si on arrive ici, il y a eu une erreur, on recharge les données du panier
            var currentCartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == HttpContext.Session.GetString("CartId"))
                .ToListAsync();

            ViewBag.CartItems = currentCartItems;
            ViewBag.Total = currentCartItems.Sum(item => item.Product.Price * item.Quantity);

            return View(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var cartId = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            // Create new order
            var order = new Order
            {
                UserId = User.Identity?.Name ?? "",
                OrderDate = DateTime.Now,
                Status = "Completed",
                Total = cartItems.Sum(item => item.Product.Price * item.Quantity)
            };

            // Add order items
            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            // Save order and clear cart
            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Clear cart session
            HttpContext.Session.Remove("CartId");

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction(nameof(OrderDetails), new { id = order.Id });
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .Where(o => User.Identity != null && o.UserId == User.Identity.Name)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}