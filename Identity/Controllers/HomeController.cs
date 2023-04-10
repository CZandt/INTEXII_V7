using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using System.Linq;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;
        private ebdbContext mummyContext;
        public HomeController(UserManager<AppUser> userMgr, ebdbContext mumContext)
        {
            userManager = userMgr;
            mummyContext = mumContext;   
        }

        [Authorize]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            // string message = "Hello " + user.UserName;
            return View();
        }

        public IActionResult Summary()
        {
            var mummies = mummyContext.Burialmains.Take(10).ToList();
            return View(mummies);
        }

        public IActionResult Supervised()
        {
            return View();
        }

        public IActionResult Unsupervised()
        {
            return View();
        }
    }
}
