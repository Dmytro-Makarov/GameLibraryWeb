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
        [Required(ErrorMessage = "Game's Name required!")]
        [MaxLength(50)]
        [Display(Name = "Game Name")]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        [Display(Name = "Game Info")]
        public string? Info { get; set; }
        [Required(ErrorMessage = "Publisher required!")]
        [Display(Name = "Publisher")]
        public uint? PublisherId { get; set; }
        [Required(ErrorMessage = "Developer required!")]
        [Display(Name = "Developer Name")]
        public uint? DeveloperId { get; set; }
        [Required(ErrorMessage = "Age rating required!")]
        [Display(Name = "Rating")]
        public uint? RatingId { get; set; }
        [Display(Name = "Cover Art")]
        public string? Media { get; set; }

        public virtual Developer? Developer { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual Rating? Rating { get; set; }
        [Required(ErrorMessage = "Genres not selected!")]
        [Display(Name = "Genres")]
        public virtual ICollection<Gamegenrerelation?> Gamegenrerelations { get; set; }
    }
}
