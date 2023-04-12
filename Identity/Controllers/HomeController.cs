using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using System.Linq;
using Identity.Models.ViewModels;

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

        public IActionResult Summary(string filter, int pageNum = 1)
        {
            int pageSize = 20;

            var x = new RecordsViewModel
            {
                Burialmains = mummyContext.Burialmains
                .Where(x => x.Headdirection == filter | filter == null)
                .OrderBy(x => x.Preservation)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo
                {
                    TotalNumRecords = 
                        (filter == null ? 
                        mummyContext.Burialmains.Count() : 
                        mummyContext.Burialmains.Where(x => x.Headdirection == filter).Count()),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
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

        public IActionResult Details (long id)
        {
            var data = mummyContext.Burialmains.Single(x => x.Id == id);

            return View(data);
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
