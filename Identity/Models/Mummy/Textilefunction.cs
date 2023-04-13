using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class Textilefunction
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public int? Textilefunctionid { get; set; }

        public ICollection<TextilefunctionTextile> TextilefunctionTextiles { get; set; }
    }
}
