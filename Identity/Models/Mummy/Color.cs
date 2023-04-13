using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class Color
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public int? Colorid { get; set; }

        public ICollection<ColorTextile> ColorTextiles { get; set; }
    }
}
