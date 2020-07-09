using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Steamv2.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Pole {0} musi zawierać przynajmniej {2} znaki.", MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Developer { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Relase Date")]
        public DateTime DataRelase { get; set; }

        [Range(0, 10000)]
        public double Price { get; set; }

        public string Image { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string YouTubeLink { get; set; }


        // relacje klucza obcego
        public int GameTypeId { get; set; }


        // wirtualne właściwości (dostęp np. do typu, kiedy mam już znalezioną grę - nie musze szukać tego typu w bazie w tabeli typ po id gry w której jestem)
        public virtual GameType GameType { get; set; }

        public virtual List<GameDLC> GameDLC { get; set; }

        public virtual List<Profile> Profiles { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Rating> Ratings { get; set; }




    }
}