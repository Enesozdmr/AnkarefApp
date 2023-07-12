using System.Diagnostics;
using AnkarefApp.Data;
using Microsoft.AspNetCore.Mvc;
using AnkarefApp.Models;

namespace AnkarefApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AllDbContext _context;

    public HomeController(ILogger<HomeController> logger, AllDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public string Job()
    {
        return "başarıyla giriş yapıldı";
    }

    // LOGIN START ---------------------------------------------------------
    // GET
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserId") != null)
        {
            return RedirectToAction("Job", "Home");
        }
        
        return View();
    }
    
    // POST
    [HttpPost]
    public IActionResult Login(string inputEmail, string inputPassword)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == inputEmail && u.Password == inputPassword);

        if (user != null)
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
    
    // LOGIN END--------------------------------------------------
    
    // REGISTER START ----------------------------------------------
    
    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("UserId") != null)
        {
            return RedirectToAction("Job", "Home");
        }
        
        return View();
    }

    [HttpPost]
    public IActionResult Register(string email, string password, string confirmPassword)
    {
        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.ErrorMessage = "Email is already registered.";
            return View();
        }

        var user = new User
        {
            Email = email,
            Password = password,
        };

        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        } catch (Exception e) {
            ViewBag.ErrorMessage = e;
            return View();
        }

        return RedirectToAction("Index", "Home");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}