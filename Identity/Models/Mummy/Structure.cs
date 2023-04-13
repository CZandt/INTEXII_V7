using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class Structure
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public int? Structureid { get; set; }

        public ICollection<StructureTextile> StructureTextiles { get; set; }
    }
}
