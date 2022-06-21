using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Genre
    {
        public Genre()
        {
            Gamegenrerelations = new HashSet<Gamegenrerelation>();
        }

        public uint Id { get; set; }
        [Required(ErrorMessage = "Genre name required!")]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Gamegenrerelation> Gamegenrerelations { get; set; }
    }
}
