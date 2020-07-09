using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class OwnedGames
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? ProfileId { get; set; }

        public virtual Game Game { get; set; }
    }
}