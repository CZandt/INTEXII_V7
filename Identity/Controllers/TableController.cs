using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models.Mummy;
using Identity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace Identity.Controllers
{
    public class TableController : Controller
    {
        private ebdbContext MummyContext;
        public TableController(ebdbContext temp)
        {
            MummyContext = temp;
        }

        [HttpGet]
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
                },
                Filter = false
            };

            return View(x);
        }

        [HttpPost]
        public IActionResult Summary(string field = "Area", string query = "", int pageNum = 1, string filter = null)
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
                },
                Filter = false
            };

            try
            {
                var parameter = Expression.Parameter(typeof(Burialmain), "x");
                var property = Expression.Property(parameter, field);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var queryExpression = Expression.Constant(query);
                var containsCall = Expression.Call(property, containsMethod, queryExpression);
                var nullCheck = Expression.Equal(queryExpression, Expression.Constant(null, typeof(string)));
                var orExpression = Expression.Or(containsCall, nullCheck);
                var wherePredicate = Expression.Lambda<Func<Burialmain, bool>>(orExpression, parameter);

                x = new RecordsViewModel
                {
                    Burialmains = MummyContext.Burialmains
                        .Where(wherePredicate)
                        .OrderBy(x => x.Preservation)
                        .ToList(),
                    PageInfo = new PageInfo
                    {
                        TotalNumRecords =
                            (query == null ?
                            MummyContext.Burialmains.Count() :
                            MummyContext.Burialmains.Where(wherePredicate).Count()),
                            RecordsPerPage = pageSize,
                            CurrentPage = pageNum
                    },
                    Filter = true
                };
            }
            
            catch
            {
                pageSize = 30;

                x = new RecordsViewModel
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
                    },
                    Filter = false
                };
            }


            return View(x);
        }

        public IActionResult Textiles(string filter, int pageNum = 1)
        {
            int pageSize = 30;

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

        public async Task<IActionResult> TextileDetailsAsync(long id)
        {
            var data = await MummyContext.Textiles
               .Include(x => x.ColorTextiles)
               .ThenInclude(x => x.Color)
               .Include(x => x.TextilefunctionTextiles)
               .ThenInclude(x => x.Textilefunction)
               .Include(x => x.StructureTextiles)
               .ThenInclude(x => x.Structure)
               .SingleOrDefaultAsync(m => m.Id == id);

            return View(data);
        }
        public IActionResult ArtifactDetails(long id)
        {

            var data = MummyContext.Artifactkomaushimregisters
            .Where(x => x.Id == id);

            return View(data);
        }
        public IActionResult Artifacts(string filter, int pageNum = 1)
        {
            int pageSize = 30;

            var x = new RecordsViewModel
            {
                Artifactkomaushimregisters = MummyContext.Artifactkomaushimregisters
                .Where(x => x.Material == filter | filter == null)
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo
                {
                    TotalNumRecords =
                            (filter == null ?
                            MummyContext.Artifactkomaushimregisters.Count() :
                            MummyContext.Artifactkomaushimregisters.Where(x => x.Material == filter).Count()),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                },
                Filter = false
            };

            return View(x);
        }
    }
}
