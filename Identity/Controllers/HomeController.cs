using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using System.Linq;
using Identity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime;
using Identity.Models.Onnx;
using System.Collections.Generic;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;
        private ebdbContext mummyContext;
        private InferenceSession _session;
        public HomeController(UserManager<AppUser> userMgr, ebdbContext mumContext, InferenceSession session)
        {
            userManager = userMgr;
            mummyContext = mumContext;
            _session = session;
        }
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            return View();
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

                return RedirectToAction("Summary", "Table");
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

            return RedirectToAction("Summary", "Table");
        }

        public async Task<IActionResult> DetailsAsync (long id)
        {
            var data = await mummyContext.Burialmains
                .Include(x => x.BurialmainTextiles)
                .ThenInclude(x => x.Textile)
                .Include(x => x.ArtifactkomaushimregisterBurialmains)
                .ThenInclude(x => x.Artifactkomaushimregister)
                .SingleOrDefaultAsync(m => m.Id == id);

            return View(data);
        }

        [HttpGet]
        public IActionResult Supervised()
        {

            return View(new MummyData());
        }

        [HttpPost]
        public IActionResult Supervised(MummyData testData)
        {

            //RUNS THE ONNX
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", testData.AsTensor())
            });

            //EXTRACTS THE VALUE FROM THE GOOFY AHH OBJECT
            var TestFinal = result.First().AsEnumerable<string>().First();
            // STORES IT IN A PREDICTION OBJECT SO IT IS EASIER TO WORK WITH
            Prediction prediction = new Prediction();

            prediction.PredictedValue = TestFinal;


            return View("PredictedSupervised", prediction);
        }

        public IActionResult Unsupervised()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
