using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class BurialmainBodyanalysischart
    {
        public long Burialmainid { get; set; }
        public long Bodyanalysischartid { get; set; }

        public Burialmain Burialmain { get; set; }
        public Bodyanalysischart Bodyanalysischart { get; set; }
    }
}
