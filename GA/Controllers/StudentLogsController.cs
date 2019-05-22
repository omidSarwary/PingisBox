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
    public class StudentLogsController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "StudentLogs";
        }
        private readonly AppDbContext _context;

        public StudentLogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StudentLogs
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

            var _studentLogs = from s in _context.StudentLogs
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                _studentLogs = _studentLogs.Where(s => s.Message.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "date_desc":
                    _studentLogs = _studentLogs.OrderByDescending(s => s.dateTime);
                    break;
                case "Date":
                    _studentLogs = _studentLogs.OrderBy(s => s.dateTime);
                    break;


                default:
                    _studentLogs = _studentLogs.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize = 6;
            return View(await PaginatedList<StudentLog>.CreateAsync(_studentLogs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: StudentLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLog = await _context.StudentLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentLog == null)
            {
                return NotFound();
            }

            return View(studentLog);
        }

        // GET: StudentLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,dateTime,Message")] StudentLog studentLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentLog);
        }

        // GET: StudentLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLog = await _context.StudentLogs.FindAsync(id);
            if (studentLog == null)
            {
                return NotFound();
            }
            return View(studentLog);
        }

        // POST: StudentLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,dateTime,Message")] StudentLog studentLog)
        {
            if (id != studentLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentLogExists(studentLog.Id))
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
            return View(studentLog);
        }

        // GET: StudentLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLog = await _context.StudentLogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentLog == null)
            {
                return NotFound();
            }

            return View(studentLog);
        }

        // POST: StudentLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentLog = await _context.StudentLogs.FindAsync(id);
            _context.StudentLogs.Remove(studentLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentLogExists(int id)
        {
            return _context.StudentLogs.Any(e => e.Id == id);
        }
    }
}
