using HonoursProject.Data;
using HonoursProject.Models;
using HonoursProject.Data.Enum;

namespace HonoursProject.Data
{
    public class DatabaseSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            if (!context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game
                    {
                        Title = "Call of Duty: Warzone",
                        Description = "Battle Royale shooter.",
                        Platforms = new List<GamePlatform>
                        {
                            GamePlatform.PC,
                            GamePlatform.Xbox,
                            GamePlatform.Playstation
                        },
                        IsCrossPlatform = true,
                        ImageURL = "https://example.com/warzone.jpg",
                        ReleaseDate = new DateTime(2020, 3, 10),
                        Publisher = "Activision",
                        Developer = "Infinity Ward",
                        ActiveStatus = true
                    },
                    new Game
                    {
                        Title = "Fortnite",
                        Description = "Online multiplayer battle royale game.",
                        Platforms = new List<GamePlatform>
                        {
                            GamePlatform.PC,
                            GamePlatform.Xbox,
                            GamePlatform.Playstation,
                            GamePlatform.Switch
                        },
                        IsCrossPlatform = true,
                        ImageURL = "https://example.com/fortnite.jpg",
                        ReleaseDate = new DateTime(2017, 7, 25),
                        Publisher = "Epic Games",
                        Developer = "Epic Games",
                        ActiveStatus = true
                    }
                };

                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}
