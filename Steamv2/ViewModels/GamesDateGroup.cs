using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Steamv2.ViewModels
{
    public class GamesDateGroup
    {
        [DataType(DataType.Date)]
        public int EnrollmentDate { get; set; }

        public int GameCount { get; set; }
    }
}