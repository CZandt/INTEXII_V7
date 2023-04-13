using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class BurialmainTextile
    {
        public long Burialmainid { get; set; }
        public Burialmain Burialmain { get; set; }

        public long Textileid { get; set; }
        public Textile Textile { get; set; }
    }
}
