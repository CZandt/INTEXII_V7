using Identity.Models.Mummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models.ViewModels
{
    public class RecordsViewModel
    {
        public List<Burialmain> Burialmains {get; set;}
        public PageInfo PageInfo { get; set; }
        public List<Textile> Textiles { get; set; }
        public List<Artifactfagelgamousregister> Artifactfagelgamousregisters { get; set; }
        public List<Artifactkomaushimregister> Artifactkomaushimregisters { get; set; }
        public bool Filter { get; set; }
    }
}
