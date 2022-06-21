using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Publisher
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Publisher name required!")]
        [MaxLength(50)]
        [Display(Name = "Publisher Name")]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        [Display(Name = "Publisher Info")]
        public string? Info { get; set; }
        [Display(Name = "Publisher Icon")]
        public string? Media { get; set; }

        public virtual IEnumerable<Game?>? Games { get; set; }
    }
}
