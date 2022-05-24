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

        [Key]
        public uint Id { get; set; }
        [Required(ErrorMessage = "Please enter the name")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Display(Name = "Info")]
        public string? Info { get; set; }
        [Display(Name = "Publisher")]
        public uint? PublisherId { get; set; }
        [Required(ErrorMessage = "Please enter the developer")]
        [Display(Name = "Developer")]
        public uint DeveloperId { get; set; }
        [Required(ErrorMessage = "Please enter age restriction")]
        [Display(Name = "Rating")]
        public uint RatingId { get; set; }
        [Display(Name = "Image")]
        public uint? LibraryMediaId { get; set; }

        public virtual Developer Developer { get; set; } = null!;
        public virtual Librarymedium? LibraryMedia { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual Rating Rating { get; set; } = null!;
        public virtual ICollection<Gamegenrerelation> Gamegenrerelations { get; set; }
    }
}
