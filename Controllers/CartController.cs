using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopZone.Data;
using ShopZone.Models;

namespace ShopZone.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetCartId()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CartId")))
            {
                HttpContext.Session.SetString("CartId", Guid.NewGuid().ToString());
            }
            return HttpContext.Session.GetString("CartId")!;
        }

        public async Task<IActionResult> Index()
        {
            var cartId = GetCartId();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            ViewBag.Total = cartItems.Sum(item => item.Product.Price * item.Quantity);

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var cartId = GetCartId();
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Product added to cart successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Item removed from cart.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}