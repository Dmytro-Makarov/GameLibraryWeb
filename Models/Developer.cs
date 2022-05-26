using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameLibWeb
{
    public partial class Developer
    {
        public uint Id { get; set; }
        [Display(Name = "Developer Name")]
        public string Name { get; set; } = null!;
        [Display(Name = "Developer Info")]
        public string? Info { get; set; }
        [Display(Name = "Developer Icon")]
        public string? Media { get; set; }

        public virtual Game? Game { get; set; }
    }
}
