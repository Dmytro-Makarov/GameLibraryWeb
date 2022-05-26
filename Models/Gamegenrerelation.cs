using System;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Gamegenrerelation
    {
        public uint Id { get; set; }
        public uint GameId { get; set; }
        public uint GenreId { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
    }
}
