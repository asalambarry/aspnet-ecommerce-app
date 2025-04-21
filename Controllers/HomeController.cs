using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopZone.Data;
using ShopZone.Models;

namespace ShopZone.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string category, string searchString)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Category.Name == category);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p =>
                p.Name.Contains(searchString) ||
                p.Description.Contains(searchString));
        }

        ViewBag.Categories = await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();
        ViewBag.CurrentCategory = category;
        ViewBag.SearchString = searchString;

        return View(await products.ToListAsync());
    }

    public async Task<IActionResult> Catalog(string category, string searchString)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Category.Name == category);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p =>
                p.Name.Contains(searchString) ||
                p.Description.Contains(searchString));
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.CurrentCategory = category;
        ViewBag.SearchString = searchString;

        return View(await products.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
