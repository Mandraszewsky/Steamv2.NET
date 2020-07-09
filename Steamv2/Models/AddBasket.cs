using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class AddBasket
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? BasketId { get; set; }

        public virtual Game Game { get; set; }

    }
}