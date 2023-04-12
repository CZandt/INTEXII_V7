using Identity.Models.Mummy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Components
{
    public class FilterViewComponent : ViewComponent
    {
        private ebdbContext mummyContext { get; set; }

        public FilterViewComponent (ebdbContext temp)
        {
            mummyContext = temp;
        }
        
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedFilter = RouteData?.Values["filter"];

            var headdirection = mummyContext.Burialmains
                .Select(x => x.Headdirection)
                .Distinct()
                .OrderBy(x => x);

            return View(headdirection);
        }

    }
}
