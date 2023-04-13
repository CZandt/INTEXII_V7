using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class ColorTextile
    {
        public long Colorid { get; set; }
        public Color Color { get; set; }

        public long Textileid { get; set; }
        public Textile Textile { get; set; }
    }
}
