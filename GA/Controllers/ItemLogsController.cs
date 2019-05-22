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
    public class ItemLogsController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "ItemLogs";
        }
        private readonly AppDbContext _context;

        public ItemLogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ItemLogs
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

            var ItemLogs = from s in _context.itemLogs
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                ItemLogs = ItemLogs.Where(s => s.Message.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "date_desc":
                    ItemLogs = ItemLogs.OrderByDescending(s => s.dateTime);
                    break;
                case "Date":
                    ItemLogs = ItemLogs.OrderBy(s => s.dateTime);
                    break;


                default:
                    ItemLogs = ItemLogs.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize = 6;
            return View(await PaginatedList<ItemLog>.CreateAsync(ItemLogs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: ItemLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemLog = await _context.itemLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemLog == null)
            {
                return NotFound();
            }

            return View(itemLog);
        }

        // GET: ItemLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,dateTime,Message")] ItemLog itemLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemLog);
        }

        // GET: ItemLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemLog = await _context.itemLogs.FindAsync(id);
            if (itemLog == null)
            {
                return NotFound();
            }
            return View(itemLog);
        }

        // POST: ItemLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,dateTime,Message")] ItemLog itemLog)
        {
            if (id != itemLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemLogExists(itemLog.Id))
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
            return View(itemLog);
        }

        // GET: ItemLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemLog = await _context.itemLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemLog == null)
            {
                return NotFound();
            }

            return View(itemLog);
        }

        // POST: ItemLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemLog = await _context.itemLogs.FindAsync(id);
            _context.itemLogs.Remove(itemLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemLogExists(int id)
        {
            return _context.itemLogs.Any(e => e.Id == id);
        }
    }
}
