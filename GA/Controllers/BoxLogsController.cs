using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using GA.Helper;
using Microsoft.AspNetCore.Authorization;

namespace GA.Controllers
{
    [Authorize]
    public class BoxLogsController : Controller
    {


        private readonly AppDbContext _context;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "BoxLogs";
        }
        public BoxLogsController(AppDbContext context)
        {
            
            _context = context;
        }

        // GET: BoxLogs
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var boxLogs = from s in _context.boxLogs
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                boxLogs = boxLogs.Where(s => s.Message.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "date_desc":
                    boxLogs = boxLogs.OrderByDescending(s => s.dateTime);
                    break;
                case "Date":
                    boxLogs = boxLogs.OrderBy(s => s.dateTime);
                    break;


                default:
                    boxLogs = boxLogs.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize = 6;
            return View(await PaginatedList<BoxLog>.CreateAsync(boxLogs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
            // GET: BoxLogs/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxLog = await _context.boxLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boxLog == null)
            {
                return NotFound();
            }

            return View(boxLog);
        }

        // GET: BoxLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoxLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,dateTime,Message")] BoxLog boxLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boxLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boxLog);
        }

        // GET: BoxLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxLog = await _context.boxLogs.FindAsync(id);
            if (boxLog == null)
            {
                return NotFound();
            }
            return View(boxLog);
        }

        // POST: BoxLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,dateTime,Message")] BoxLog boxLog)
        {
            if (id != boxLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boxLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoxLogExists(boxLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(boxLog);
        }

        // GET: BoxLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxLog = await _context.boxLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boxLog == null)
            {
                return NotFound();
            }

            return View(boxLog);
        }

        // POST: BoxLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boxLog = await _context.boxLogs.FindAsync(id);
            _context.boxLogs.Remove(boxLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoxLogExists(int id)
        {
            return _context.boxLogs.Any(e => e.Id == id);
        }
    }
}
