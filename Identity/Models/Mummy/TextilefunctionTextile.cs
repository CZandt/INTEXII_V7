using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class TextilefunctionTextile
    {
        public long Textilefunctionid { get; set; }
        public Textilefunction Textilefunction { get; set; }

        public long Textileid { get; set; }
        public Textile Textile {get; set; }
    }
}
