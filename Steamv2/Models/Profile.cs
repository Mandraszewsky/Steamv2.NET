using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class Profile
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public double ProfileFunds { get; set; }

        public int BasketId { get; set; }


        public virtual Basket Basket { get; set; }

        public virtual List<FavoriteGames> FavoriteGamesList { get; set; }

        public virtual List<OwnedGames> OwnedGamesList { get; set; }

    }
}
