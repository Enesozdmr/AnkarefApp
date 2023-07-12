using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnkarefApp.Models;

namespace AnkarefApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    // GET
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserId") != null)
        {
            return RedirectToAction("Job", "Home");
        }
        
        return View();
    }

    public string Job()
    {
        return "başarıyla giriş yapıldı";
    }

    // POST
    [HttpPost]
    public IActionResult Login(string inputEmail, string inputPassword)
    {
        if (inputEmail == "admin@example.com" && inputPassword == "password")
        {
            HttpContext.Session.SetString("UserId", inputEmail);
            return RedirectToAction("Job", "Home");
        }

        ViewBag.ErrorMessage = "Invalid username or password.";
        
        return View("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}