﻿using System;
using System.Collections.Generic;

namespace GameLibWeb
{
    public partial class Rating
    {
        public uint Id { get; set; }
        public byte? Age { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}