﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class GameDLC
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int GameId { get; set; }



        public virtual Game Game { get; set; }

    }
}