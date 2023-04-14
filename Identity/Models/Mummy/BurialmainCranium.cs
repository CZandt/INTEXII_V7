using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class BurialmainCranium
    {
        public long Burialmainid { get; set; }
        public long Craniumid { get; set; }
        public Burialmain Burialmain { get; set; }
        public Cranium Cranium { get; set; }
    }
}
