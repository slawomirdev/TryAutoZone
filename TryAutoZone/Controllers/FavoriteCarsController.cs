using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TryAutoZone.Data;
using TryAutoZone.Models;

namespace TryAutoZone.Controllers
{
    [Authorize]
    public class FavoriteCarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoriteCarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FavoriteCars
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteCars = await _context.FavoriteCar
                .Include(f => f.Car)
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return View(favoriteCars);
        }

        // POST: FavoriteCars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int carId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!_context.FavoriteCar.Any(fc => fc.CarId == carId && fc.UserId == userId))
            {
                var favoriteCar = new FavoriteCar { CarId = carId, UserId = userId };
                _context.Add(favoriteCar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: FavoriteCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var favoriteCar = await _context.FavoriteCar.FindAsync(id);
            if (favoriteCar != null)
            {
                _context.FavoriteCar.Remove(favoriteCar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteCarExists(int id)
        {
          return (_context.FavoriteCar?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
