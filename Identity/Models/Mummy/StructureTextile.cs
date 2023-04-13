using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class StructureTextile
    {
        public long Structureid { get; set; }
        public Structure Structure { get; set; }

        public long Textileid { get; set; }
        public Textile Textile { get; set; }
    }
}
