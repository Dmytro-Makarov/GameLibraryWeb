using System;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Publisher
    {
        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }
        public uint? LibraryMediaId { get; set; }

        public virtual Librarymedium? LibraryMedia { get; set; }
        public virtual Game? Game { get; set; }
    }
}
