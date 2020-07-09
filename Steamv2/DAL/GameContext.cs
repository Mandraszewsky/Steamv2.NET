using Steamv2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Steamv2.DAL
{
    public class GameContext : DbContext
    {
        public GameContext() : base("DefaultConnection")
        {

        }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameType> GameTypes { get; set; }

        public DbSet<GameDLC> GameDLCs { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Rating> Ratings { get; set; }

    }
}