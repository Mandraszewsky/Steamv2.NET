using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Steamv2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Steamv2.DAL
{
    public class GameInitializer : DropCreateDatabaseIfModelChanges<GameContext>
    {
        protected override void Seed(GameContext context)
        {
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));

            var user2 = new ApplicationUser { UserName = "admin@gmial.com", Email = "admin@gmial.com" };
            string password2 = "Admin1!";

            userManager.Create(user2, password2);
            userManager.AddToRole(user2.Id, "Admin");

            Profile profile2 = new Profile { UserName = user2.Email };
            Basket basket2 = new Basket { };

            context.Baskets.Add(basket2);
            context.Profiles.Add(profile2);
            context.SaveChanges();
            

            var types = new List<GameType>
            {
                new GameType{ Name = "RPG" },
                new GameType{ Name = "RTS" },
                new GameType{ Name = "FPS" },
                new GameType{ Name = "Rouge-Like" }
            };

            types.ForEach(t => context.GameTypes.Add(t));
            context.SaveChanges();


            var games = new List<Game>
            {
                new Game{ Name = "Tibia", Description = "Trzeba iść na hunta.", Developer = "Cipsoft", DataRelase =  new DateTime(1997, 6, 1), GameType = types[3], Price = 40.00,
                Image = "tibia1.jpg", Image2 = "tibia2.png", Image3 = "tibia3.png", YouTubeLink = "https://www.youtube.com/embed/SDLyx_IqYmg"},
                new Game{ Name = "Grand Theft Auto 5", Description = "Robimy napad na bank?", Developer = "Rockstar", DataRelase =  new DateTime(2019, 8, 8), GameType = types[2], Price = 59.99,
                Image = "gta1.jpg", Image2 = "gta2.jpg", Image3 = "gta3.jpg", YouTubeLink = "https://www.youtube.com/embed/QkkoHAzjnUs"},
                new Game{ Name = "Darkest Dungeon", Description = "Ruins...", Developer = "SCC", DataRelase =  new DateTime(2019, 9, 10), GameType = types[0], Price = 11.99,
                Image = "dd1.jpg", Image2 = "dd2.jpg", Image3 = "dd3.jpg", YouTubeLink = "https://www.youtube.com/embed/AQLxdHfMPF8"},
                new Game{ Name = "The Witcher 3", Description = "Zaraza.", Developer = "CDP", DataRelase =  new DateTime(2015, 5, 19), GameType = types[0], Price = 60.00,
                Image = "witcher1.jpeg", Image2 = "witcher2.jpg", Image3 = "witcher3.jpg", YouTubeLink = "https://www.youtube.com/embed/ehjJ614QfeM"},
                new Game{ Name = "Gothic 2", Description = "Nic tu nie ma. Niczego tu nie znajde.", Developer = "Piranha Bytes", DataRelase =  new DateTime(2002, 11, 29), GameType = types[0], Price = 20.00,
                Image = "gothic1.jpg", Image2 = "gothic2.jpg", Image3 = "gothic3.jpg", YouTubeLink = "https://www.youtube.com/embed/v0R0k9djvc0"}
            };

            games.ForEach(g => context.Games.Add(g));
            context.SaveChanges();

            var dlcs = new List<GameDLC>
            {
                new GameDLC{ Name = "DLC NUMBER ONE", Game = games[0], Price = 20.00 },
                new GameDLC{ Name = "DLC NUMBER SIX WITH EXTRA DEEP", Game = games[1], Price = 40.00 },
                new GameDLC{ Name = "DLC NUMBER SEVEN", Game = games[1], Price = 60.00 }
            };

            dlcs.ForEach(d => context.GameDLCs.Add(d));
            context.SaveChanges();



        }



    }
}