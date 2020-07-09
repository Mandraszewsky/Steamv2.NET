using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Steamv2.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm}", ApplyFormatInEditMode = true)]
        public DateTime ContentDate { get; set; }

        public string UserName { get; set; }

        public int? GameId { get; set; }


        public virtual Game Game { get; set; }
    }
}