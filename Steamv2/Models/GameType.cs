using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class GameType
    {
        public int Id { get; set; }

        public string Name { get; set; }



        public virtual List<Game> Games { get; set; }

    }
}