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
    public class StudentsListController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "StudentsList";
        }
        private readonly AppDbContext _context;

        public StudentsListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StudentsList   return View(await _context.students.OrderByDescending(p => p.Id).ToListAsync());         

        public async Task<IActionResult> Index(
     string sortOrder,
     string currentFilter,
     string searchString,
     int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["ItemSortParm"] = sortOrder == "Item" ? "item_desc" : "Item";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FullName.Contains(searchString)
                                       || s.email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.email);
                    break;
                case "name":
                    students = students.OrderBy(s => s.email);
                    break;
                case "item_desc":
                    students = students.OrderByDescending(s => s.IsBorrowed);
                    break;
                case "Item":
                    students = students.OrderBy(s => s.IsBorrowed);
                    break;


                default:
                    students = students.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize = 6;
            return View(await PaginatedList<Students>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: StudentsList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: StudentsList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentsList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,email,code,RFID,IsBorrowed,BorrowedItem,BorrowedItem1,BorrowedTime,HandedBack,IsOverTime")] Students students)
        {
            if (ModelState.IsValid)
            {
                _context.Add(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }

        // GET: StudentsList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
        }

        // POST: StudentsList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,email,code,RFID,IsBorrowed,BorrowedItem,BorrowedItem1,BorrowedTime,HandedBack,IsOverTime")] Students students)
        {
            if (id != students.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.Id))
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
            return View(students);
        }

        // GET: StudentsList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: StudentsList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _context.students.FindAsync(id);
            _context.students.Remove(students);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(int id)
        {
            return _context.students.Any(e => e.Id == id);
        }
    }
}
