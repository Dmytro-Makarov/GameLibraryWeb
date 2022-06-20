using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Gamegenrerelation
    {
        public uint Id { get; set; }
        [Display(Name = "Game Id")]
        public uint? GameId { get; set; }
        [Display(Name = "Genre Id")]
        public uint? GenreId { get; set; }

        [Display(Name = "Game")]
        public virtual Game? Game { get; set; }
        [Display(Name = "Genre")]
        public virtual Genre? Genre { get; set; }
    }
}