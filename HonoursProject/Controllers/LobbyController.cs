using HonoursProject.Data;
using HonoursProject.Models;
using HonoursProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using HonoursProject.Data.Enum;

namespace HonoursProject.Controllers
{
    [Authorize] // Ensure only logged-in users can access
    public class LobbyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LobbyController> _logger;

        public LobbyController(ApplicationDbContext context, ILogger<LobbyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Create Lobby Page
        public IActionResult Create()
        {
            _logger.LogInformation("Create() GET called");

            var model = new CreateLobbyViewModel
            {
                Games = _context.Games.Where(g => g.ActiveStatus).ToList(),
                Languages = new List<string>
                {
                    "English", "Spanish", "French", "German", "Italian", "Portuguese",
                    "Russian", "Chinese", "Japanese", "Korean", "Arabic", "Any"
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLobbyViewModel model)
        {
            // ✅ Log all available claims for debugging
            _logger.LogInformation("Available Claims:");
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"{claim.Type}: {claim.Value}");
            }

            // ✅ Check if user is authenticated and claim is available
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError("", "User is not authenticated.");
                // ✅ Reload dropdowns
                model.Games = await _context.Games.Where(g => g.ActiveStatus).ToListAsync();
                model.Languages = GetLanguages();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                // ✅ Reload dropdowns in case the form is invalid
                model.Games = await _context.Games.Where(g => g.ActiveStatus).ToListAsync();
                model.Languages = GetLanguages();

                // ✅ Log ModelState errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
                }

                return View(model);
            }

            // ✅ Create the lobby if everything is valid
            var lobby = new Lobby
            {
                UserId = userId,
                GameId = model.GameId,
                MaxPlayers = model.MaxPlayers,
                MicRequired = model.MicRequired,
                PreferredLanguage = model.PreferredLanguage,
                Description = model.Description,
                GamePlatform = model.GamePlatform,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Participants = new List<LobbyParticipant>
        {
            new LobbyParticipant { UserId = userId }
        }
            };

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Lobby created successfully with ID: {LobbyId}", lobby.LobbyId);

            return RedirectToAction("CreatedLobby", new { lobbyId = lobby.LobbyId });
        }

        // ✅ Helper method to get languages
        private List<string> GetLanguages()
        {
            return new List<string>
    {
        "English", "Spanish", "French", "German", "Italian", "Portuguese",
        "Russian", "Chinese", "Japanese", "Korean", "Arabic", "Any"
    };
        }




        // ✅ Created Lobby View
        public async Task<IActionResult> CreatedLobby(int lobbyId)
        {
            _logger.LogInformation("CreatedLobby() called with lobbyId: {LobbyId}", lobbyId);

            var lobby = await _context.Lobbies
                .Include(l => l.Participants)
                    .ThenInclude(lp => lp.User) // ✅ Include User from LobbyParticipant
                .Include(l => l.Game)
                .FirstOrDefaultAsync(l => l.LobbyId == lobbyId);

            if (lobby == null)
            {
                _logger.LogWarning("Lobby not found with ID: {LobbyId}", lobbyId);
                return NotFound();
            }

            var model = new CreatedLobbyViewModel
            {
                LobbyId = lobby.LobbyId,
                GameTitle = lobby.Game.Title,
                Platform = lobby.GamePlatform.ToString(),
                Participants = lobby.Participants.Select(lp => new UserInfo
                {
                    UserName = lp.User.UserName,
                    GamingId = lp.User.DiscordId ?? lp.User.XboxId ?? lp.User.PS5Id ?? lp.User.SteamId ?? "N/A", // ✅ Select available ID
                    ProfilePictureUrl = lp.User.ProfileImageURL
                }).ToList(),
                ChatMessages = "" // Placeholder for chat messages
            };

            _logger.LogInformation("Lobby loaded successfully with ID: {LobbyId}", lobby.LobbyId);

            return View(model);
        }

        // ✅ Disband Lobby
        [HttpPost]
        public async Task<IActionResult> Disband(int id)
        {
            _logger.LogInformation("Disband() called with id: {LobbyId}", id);

            var lobby = await _context.Lobbies
                .Include(l => l.Participants)
                .FirstOrDefaultAsync(l => l.LobbyId == id);

            if (lobby != null)
            {
                _context.LobbyParticipants.RemoveRange(lobby.Participants); // ✅ Remove participants first
                _context.Lobbies.Remove(lobby);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Lobby disbanded with ID: {LobbyId}", id);
            }

            return RedirectToAction("LoggedInHome", "Home");
        }

        // ✅ Delete Lobby (Optional)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete() called with id: {LobbyId}", id);

            var lobby = await _context.Lobbies
                .Include(l => l.Participants)
                .FirstOrDefaultAsync(l => l.LobbyId == id);

            if (lobby != null)
            {
                _context.LobbyParticipants.RemoveRange(lobby.Participants); // ✅ Remove participants first
                _context.Lobbies.Remove(lobby);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Lobby deleted with ID: {LobbyId}", id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string game, int? platform, string language)
        {
            var query = _context.Lobbies
                .Include(l => l.Game)
                .Where(l => l.IsActive);

            if (!string.IsNullOrEmpty(game))
                query = query.Where(l => l.Game.Title.Contains(game));

            if (platform.HasValue)
            {
                query = query.Where(l => l.Game.PlatformMappings.Any(pm => pm.Platform == (GamePlatform)platform.Value));
            }

            if (!string.IsNullOrEmpty(language))
                query = query.Where(l => l.PreferredLanguage == language);

            var lobbies = await query.OrderByDescending(l => l.CreatedDate).ToListAsync();

            var model = new LobbySearchViewModel
            {
                Lobbies = lobbies,
                Games = await _context.Games.Where(g => g.ActiveStatus).ToListAsync(),
                Platforms = Enum.GetValues(typeof(GamePlatform)).Cast<GamePlatform>().ToList(),
                Languages = GetLanguages()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int lobbyId)
        {
            _logger.LogInformation("Join() called with lobbyId: {LobbyId}", lobbyId);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogWarning("User not authenticated");
                return RedirectToAction("Login", "Account");
            }

            var lobby = await _context.Lobbies
                .Include(l => l.Participants)
                .FirstOrDefaultAsync(l => l.LobbyId == lobbyId);

            if (lobby == null || !lobby.IsActive)
            {
                _logger.LogWarning("Lobby not found or inactive");
                return NotFound();
            }

            // Check if user is already in the lobby
            if (!lobby.Participants.Any(p => p.UserId == userId))
            {
                // Add user to lobby
                var participant = new LobbyParticipant
                {
                    LobbyId = lobbyId,
                    UserId = userId
                };
                _context.LobbyParticipants.Add(participant);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserId} joined lobby {LobbyId}", userId, lobbyId);
            }

            // ✅ Redirect to CreatedLobby view
            return RedirectToAction("CreatedLobby", new { lobbyId = lobby.LobbyId });
        }

    }
}
