using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HonoursProject.Data;
using HonoursProject.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ✅ Login Method
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Email == username);
        if (user == null || !BC.Verify(password, user.Password))
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError("", "Your account has been banned.");
            return View();
        }

        // ✅ Store UserId and UserName in Session
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        HttpContext.Session.SetString("UserName", user.UserName);

        // ✅ Add explicit "UserId" claim + NameIdentifier for consistency
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim("UserId", user.UserId.ToString()) // ✅ Added explicitly
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        _logger.LogInformation("User {Username} logged in successfully", user.UserName);

        return RedirectToAction("LoggedInHome", "Home");
    }

    // ✅ Register Method
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegister model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
        {
            ModelState.AddModelError("UserName", "Username is already taken.");
            return View(model);
        }
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email is already registered.");
            return View(model);
        }

        try
        {
            string hashedPassword = BC.HashPassword(model.Password);

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = hashedPassword,
                DiscordId = model.DiscordId,
                XboxId = model.XboxId,
                PS5Id = model.PS5Id,
                SteamId = model.SteamId,
                ProfileImageURL = "/images/default-profile.png",
                JoinDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // ✅ Add explicit "UserId" claim + NameIdentifier for consistency
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("UserId", user.UserId.ToString()) // ✅ Added explicitly
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // ✅ Store UserId and UserName in Session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.UserName);

            _logger.LogInformation("User {Username} registered and logged in successfully", user.UserName);

            return RedirectToAction("LoggedInHome", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ModelState.AddModelError("", "An error occurred while creating your account. Please try again.");
            return View(model);
        }
    }

    // ✅ Logout Method
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();

        _logger.LogInformation("User logged out successfully");

        return RedirectToAction("Index", "Home");
    }
}
