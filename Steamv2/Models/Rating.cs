﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steamv2.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int Vote { get; set; }

        public int GameId { get; set; }

        public int ProfileId { get; set; }

        public virtual Game Game { get; set; }

    }
}