using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Developer
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Developer name required!")]
        [MaxLength(50)]
        [Display(Name = "Developer Name")]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        [Display(Name = "Developer Info")]
        public string? Info { get; set; }
        [Display(Name = "Developer Icon")]
        public string? Media { get; set; }

        public virtual IEnumerable<Game?>? Games { get; set; }
    }
}
