using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using GA.Helper;

namespace GA.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.asd = "Notifications";
        }
        private readonly AppDbContext _context;

        public NotificationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Notifications
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var notifications =  _context.notifications.OrderByDescending(p => p.Id);
            int pageSize = 7;
            return View(await PaginatedList<Notifications>.CreateAsync(notifications, pageNumber ?? 1, pageSize));
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notifications == null)
            {
                return NotFound();
            }

            return View(notifications);
        }

       

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notifications == null)
            {
                return NotFound();
            }

            return PartialView(notifications);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notifications = await _context.notifications.FindAsync(id);
            _context.notifications.Remove(notifications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationsExists(int id)
        {
            return _context.notifications.Any(e => e.Id == id);
        }
    }
}
