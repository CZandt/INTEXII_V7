using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using Identity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    public class TableController : Controller
    {
        private ebdbContext MummyContext;
        public TableController(ebdbContext temp)
        {
            MummyContext = temp;
        }

        public IActionResult Summary(string filter, int pageNum = 1)
        {
            int pageSize = 30;

            var x = new RecordsViewModel
            {
                Burialmains = MummyContext.Burialmains
                .Where(x => x.Headdirection == filter | filter == null)
                .OrderBy(x => x.Preservation)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo
                {
                    TotalNumRecords =
                            (filter == null ?
                            MummyContext.Burialmains.Count() :
                            MummyContext.Burialmains.Where(x => x.Headdirection == filter).Count()),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }
        public IActionResult Artifacts(string filter, int pageNum = 1)
        {
            int pageSize = 20;

            var x = new RecordsViewModel
            {
                Artifactfagelgamousregisters = MummyContext.Artifactfagelgamousregisters
                .OrderBy(x => x.Registernum)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo
                {
                    TotalNumRecords = MummyContext.Artifactfagelgamousregisters.Count(),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }

        public IActionResult Textiles(string filter, int pageNum = 1)
        {
            int pageSize = 20;

            var x = new RecordsViewModel
            {
                Textiles = MummyContext.Textiles
                .Include(x => x.ColorTextiles)
                .ThenInclude(x => x.Color)
                .Include(x => x.TextilefunctionTextiles)
                .ThenInclude(x => x.Textilefunction)
                .Include(x => x.StructureTextiles)
                .ThenInclude(x => x.Structure)
                .OrderBy(x => x.Locale)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo
                {
                    TotalNumRecords = MummyContext.Textiles.Count(),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(x);
        }
    }
}
