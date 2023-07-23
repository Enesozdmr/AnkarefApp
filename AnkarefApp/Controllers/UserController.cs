﻿using AnkarefApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace AnkarefApp.Controllers;

public class UserController : Controller
{
    private readonly AllDbContext _context;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, AllDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserId") != null) return RedirectToAction("Activity", "Activity");

        return View();
    }

    [HttpPost]
    public IActionResult Login(string inputEmail, string inputPassword)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == inputEmail && u.Password == inputPassword);

        if (user != null)
        {
            HttpContext.Session.SetString("UserId", inputEmail);
            
            return RedirectToAction("Activity", "Activity");
        }

        ViewBag.ErrorMessage = "Invalid username or password.";

        return View("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("UserId") != null) return RedirectToAction("Activity", "Activity");

        return Index();
    }

    [HttpPost]
    public IActionResult Register(string email, string password, string confirmPassword)
    {
        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.ErrorMessage = "Email is already registered.";
            return View();
        }

        if (password != confirmPassword)
        {
            ViewBag.ErrorMessage = "Passwords do not match.";
            return View();
        }

        var user = new User
        {
            Email = email,
            Password = password
        };

        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            ViewBag.SuccessMessage = "Registration successful! Thank you for signing up.";
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = e;
            return View();
        }


        return RedirectToAction("Index", "User");
    }
}