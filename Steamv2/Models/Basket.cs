using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class Basket
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public virtual List<AddBasket> GameList { get; set; }

    }
}