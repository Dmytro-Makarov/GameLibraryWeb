using System.ComponentModel.DataAnnotations;


namespace GameLibWeb
{
    public partial class Rating
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Age rating required!")]
        [Range(0, 21)]
        public uint Age { get; set; }
        public virtual IEnumerable<Game?>? Games { get; set; }
    }
}