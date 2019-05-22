using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace GA.Controllers
{
    public class GraphsController : Controller
    {
        [Authorize]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "Statistics";
        }
        private readonly AppDbContext _context;
        private readonly IItemRepository _itemRepository;

        public GraphsController(AppDbContext context, IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
           _context = context;
        }

        // GET: Graphs
        public async Task<IActionResult> Index()
        {           
            return View(await _context.itemCount.ToListAsync());
        }


        public JsonResult JsonData(int? id)
        {
            int num = id.Value;
            if (num>0)
            {
                
                var item = _itemRepository.GetLastCounts(num).OrderBy(p => p.Id);
                string json = JsonConvert.SerializeObject(item);
                return Json(json);
            }
            else
            {
                var item = _itemRepository.GetAllCounts().OrderBy(p => p.Id);
                string json = JsonConvert.SerializeObject(item);
                return Json(json);
            }

        }

    }
}
