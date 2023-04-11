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
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            return View();
        }

        public IActionResult Summary()
        {
            var mummies = mummyContext.Burialmains
                .OrderBy(x => x.Preservation)
                .ToList();

            return View(mummies);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddData(Burialmain burialmain)
        {
            if (ModelState.IsValid)
            {
                mummyContext.Add(burialmain);
                mummyContext.SaveChanges();

                return View("AddEditConfirmation");
            }
            else
            {
                return View();
            }
            
        }

        [HttpGet]
        public IActionResult Edit (long id)
        {
            var data = mummyContext.Burialmains.Single(x => x.Id == id);

            return View("AddData", data);
        }

        [HttpPost]
        public IActionResult Edit (Burialmain burialmain)
        {
            if (ModelState.IsValid)
            {
                mummyContext.Update(burialmain);
                mummyContext.SaveChanges();

                return RedirectToAction("Summary");
            }
            else //If data entry not valid
            {
                return View(burialmain);
            }
        }

        [HttpGet]
        public IActionResult Delete (long id)
        {
            var data = mummyContext.Burialmains.Single(x => x.Id == id);

            return View(data);
        }

        [HttpPost]
        public IActionResult Delete (Burialmain burialmain)
        {
            mummyContext.Burialmains.Remove(burialmain);
            mummyContext.SaveChanges();

            return RedirectToAction("Summary");
        }

        public IActionResult Supervised(Burialmain burialmain)
        {
            return View();
        }

        public IActionResult Unsupervised()
        {
            return View();
        }
    }
}
