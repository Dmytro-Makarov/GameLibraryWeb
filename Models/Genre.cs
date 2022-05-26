using System;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Genre
    {
        public Genre()
        {
            Gamegenrerelations = new HashSet<Gamegenrerelation>();
        }

        public uint Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Gamegenrerelation> Gamegenrerelations { get; set; }
    }
}
