using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using System.Linq;
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

        public IActionResult Summary()
        {
            var mummies = mummyContext.Burialmains.Take(10).ToList();
            return View(mummies);
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
    }
}
