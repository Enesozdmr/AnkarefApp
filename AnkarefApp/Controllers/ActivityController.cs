using AnkarefApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnkarefApp.Controllers;

public class ActivityController : Controller
{
    private readonly AllDbContext _context;
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(ILogger<ActivityController> logger, AllDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Activity()
    {
        if (TempData["Message"] != "")
        {
            ViewBag.Message = TempData["Message"];
        }

        var activities = _context.Activities.ToList();

        return View(activities);
    }

    [HttpGet]
    public IActionResult ActivityAdd()
    {
        var categories = _context.ActivityCategories
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();
        ViewBag.Categories = categories;

        var users = _context.Users
            .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Email })
            .ToList();
        ViewBag.Users = users;

        return View();
    }

    [HttpPost]
public IActionResult AddActivity(string title, string description, DateTime date, Guid category, List<Guid> users)
{
    var selectedCategory = _context.ActivityCategories.FirstOrDefault(c => c.Id == category);
    if (selectedCategory == null)
    {
        TempData["Message"] = "Invalid category selected.";
        return RedirectToAction("ActivityAdd");
    }

    if (users == null || users.Count == 0)
    {
        TempData["Message"] = "Please select at least one user.";
        return RedirectToAction("ActivityAdd");
    }
    
    // Select user from database with email stored in session variable from _context
    var userId = _context.Users.FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserId"));

    if (!Guid.TryParse(userId.Id.ToString(), out var creatingUserId))
    {
        TempData["Message"] = "User ID is invalid.";
        return RedirectToAction("Activity");
    }

    var user = _context.Users.FirstOrDefault(u => u.Id == creatingUserId);

    if (user == null)
    {
        TempData["Message"] = "User does not exist.";
        return RedirectToAction("Activity");
    }
    
    var activityCategory = _context.ActivityCategories.FirstOrDefault(c => c.Id == category);

    if (activityCategory == null)
    {
        TempData["Message"] = "Selected category does not exist.";
        return RedirectToAction("Activity");
    }

    try
    {
        var activity = new ActivityTable
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Date = date,
            ActivityCategory = activityCategory,
            ActivityCategoryId = activityCategory.Id,
            CreatingUser = user,
            CreatingUserId = user.Id
        };

        _context.Activities.Add(activity);
        _context.SaveChanges();

        var activityUserMappings = users.Select(userId => new ActivityParticipant()
        {
            ActivityId = activity.Id,
            UserId = userId
        }).ToList();

        _context.ActivityParticipants.AddRange(activityUserMappings);
        _context.SaveChanges();

        TempData["Message"] = "Activity added successfully!";

        return RedirectToAction("ActivityDetail", new { id = activity.Id });
    }
    catch (Exception e)
    {
        // Handle any exceptions that occur during the saving process
        TempData["Message"] = "Error adding activity. Please try again.";
        // You can log the exception details for debugging purposes
        // Redirect back to the AddActivity page with a message
        return RedirectToAction("ActivityAdd");
    }
}

public IActionResult ActivityDetail(Guid id)
{
    var activity = _context.Activities.FirstOrDefault(a => a.Id == id);

    if (activity == null)
    {
        TempData["Message"] = "Activity not found.";
        return RedirectToAction("Activity");
    }

    var categoryName = _context.ActivityCategories.FirstOrDefault(c => c.Id == activity.ActivityCategoryId)?.Name;

    var userIds = _context.ActivityParticipants.Where(au => au.ActivityId == id).Select(au => au.UserId);
    var users = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();

    ViewBag.CategoryName = categoryName;
    ViewBag.Users = users;
    return View(activity);
}


    [HttpGet]
    public IActionResult ActivityCategoryAdd()
    {
        if (TempData["Message"] != "")
        {
            ViewBag.Message = TempData["Message"];
        }

        return View();
    }

    [HttpPost]
    public IActionResult ActivityCategoryAdd(string categoryName)
    {
        var category = new ActivityCategory
        {
            Id = Guid.NewGuid(),
            Name = categoryName
        };

        try
        {
            _context.ActivityCategories.Add(category);
            _context.SaveChanges();

            TempData["Message"] = $"{categoryName} category added successfully!";
        }
        catch (Exception e)
        {
            TempData["Message"] = $"{categoryName} can not be added!";
        }
        
        return RedirectToAction("ActivityCategoryAdd", "Activity");
    }
}