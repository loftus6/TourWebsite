using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TourWebsite.Data;
using TourWebsite.Models;

namespace TourWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TourWebsiteContext _context;

    public HomeController(ILogger<HomeController> logger, TourWebsiteContext tourWebsiteContext)
    {
        _logger = logger;
        _context = tourWebsiteContext;
    }

    public async Task<IActionResult> IndexAsync()
    {
        return View(await _context.GetMain());
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
