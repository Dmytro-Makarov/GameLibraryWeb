using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Game
    {
        public Game()
        {
            Gamegenrerelations = new HashSet<Gamegenrerelation>();
        }

        public uint Id { get; set; }
        [Display(Name = "Game Name")]
        public string Name { get; set; } = null!;
        [Display(Name = "Game Info")]
        public string? Info { get; set; }
        [Display(Name = "Publisher")]
        public uint? PublisherId { get; set; }
        [Display(Name = "Developer Name")]
        public uint? DeveloperId { get; set; }
        [Display(Name = "Rating")]
        public uint? RatingId { get; set; }
        [Display(Name = "Cover Art")]
        public string? Media { get; set; }

        public virtual Developer? Developer { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual Rating? Rating { get; set; }
        public virtual ICollection<Gamegenrerelation> Gamegenrerelations { get; set; }
    }
}
