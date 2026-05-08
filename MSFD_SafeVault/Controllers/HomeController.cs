using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MSFD_SafeVault.Models;
using MSFD_SafeVault.Data;
using MSFD_SafeVault.Services;

namespace MSFD_SafeVault.Controllers;

public class HomeController : Controller
{
    // In a real application, you would use dependency injection to get these services. 
    // For this lab, we're just instantiating them directly for simplicity.
    private readonly ILogger<HomeController> _logger;
    private readonly SecureDatabaseExample _db;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;

        // Example connection string (not actually used in the lab because we're using an in-memory database)
        _db = new SecureDatabaseExample("Server=.;Database=SafeVault;Trusted_Connection=True;");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Form POST handler — requires authentication
    [Authorize]
    [HttpPost("/submit")]
    public IActionResult Submit(UserInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid input");
        }

        // Sanitize inputs before using them
        var cleanUsername = InputSanitizer.Sanitize(model.Username);
        var cleanEmail = InputSanitizer.Sanitize(model.Email);

        // Save to in-memory database 
        // In a real application, you would use parameterized queries to save to a real database
        // Since this is a lab, we're just simulating it with an in-memory list.
        InMemoryDatabase.Users.Add(new UserRecord
        {
            Username = cleanUsername,
            Email = cleanEmail
        });

        // Demo-only SQL call: do not fail the request if local SQL Server is unavailable.
        try
        {
            _db.GetUserByUsername(cleanUsername);
        }
        catch (SqlException ex)
        {
            _logger.LogWarning(ex, "Skipping demo database lookup because SQL Server is unavailable.");
        }

        return Ok("Input validated, sanitized, and saved.");
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    
}
