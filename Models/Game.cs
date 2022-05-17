using System;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Game
    {
        public Game()
        {
            Gamegenrerelations = new HashSet<Gamegenrerelation>();
        }

        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }
        public uint? PublisherId { get; set; }
        public uint DeveloperId { get; set; }
        public uint RatingId { get; set; }
        public uint? LibraryMediaId { get; set; }

        public virtual Developer Developer { get; set; } = null!;
        public virtual Librarymedium? LibraryMedia { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual Rating Rating { get; set; } = null!;
        public virtual ICollection<Gamegenrerelation> Gamegenrerelations { get; set; }
    }
}
