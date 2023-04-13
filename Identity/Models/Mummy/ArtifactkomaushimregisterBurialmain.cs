using System;
using System.Collections.Generic;

#nullable disable

namespace Identity.Models.Mummy
{
    public partial class ArtifactkomaushimregisterBurialmain
    {
        public long Artifactkomaushimregisterid { get; set; }
        public Artifactkomaushimregister Artifactkomaushimregister { get; set; }

        public long Burialmainid { get; set; }
        public Burialmain Burialmain { get; set; }
    }
}
