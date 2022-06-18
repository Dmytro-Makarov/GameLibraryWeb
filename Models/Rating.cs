using System;
using System.Collections.Generic;


namespace GameLibWeb
{
    public partial class Rating
    {
        public uint Id { get; set; }
        public uint Age { get; set; }

        public virtual IEnumerable<Game?> Games { get; set; }
    }
}