using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TryAutoZone.Data;
using System.Security.Claims;
using TryAutoZone.Models;

namespace TryAutoZone.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
              return _context.Car != null ? 
                          View(await _context.Car.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Car'  is null.");
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavorite = User.Identity.IsAuthenticated &&
                             _context.FavoriteCar.Any(fc => fc.CarId == id && fc.UserId == userId);

            int? favoriteCarId = null;
            if (isFavorite)
            {
                favoriteCarId = _context.FavoriteCar
                    .Where(fc => fc.CarId == id && fc.UserId == userId)
                    .Select(fc => fc.Id)
                    .FirstOrDefault();
            }

            ViewBag.IsFavorite = isFavorite;
            ViewBag.FavoriteCarId = favoriteCarId;

            return View(car);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Make,Model,Year,HorsePower,EngineCapacity,EngineType,Gearbox,CO2Emission,FuelConsumptionString,Description,ImageUrl,VideoUrl")] Car car)
        {
            if (ModelState.IsValid)
            {
                if (double.TryParse(car.FuelConsumptionString.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double fuelConsumption))
                {
                    car.FuelConsumption = Math.Round(fuelConsumption, 1);
                    car.FuelConsumptionString = car.FuelConsumption.ToString("F1", CultureInfo.InvariantCulture);
                }
                else
                {
                    ModelState.AddModelError("FuelConsumptionString", "Nieprawidłowy format zużycia paliwa.");
                    return View(car);
                }

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            car.FuelConsumptionString = car.FuelConsumption.ToString("F1", CultureInfo.InvariantCulture);

            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,HorsePower,EngineCapacity,EngineType,Gearbox,CO2Emission,FuelConsumptionString,Description,ImageUrl,VideoUrl")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (double.TryParse(car.FuelConsumptionString.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double fuelConsumption))
                    {
                        car.FuelConsumption = Math.Round(fuelConsumption, 1);
                        car.FuelConsumptionString = car.FuelConsumption.ToString("F1", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        ModelState.AddModelError("FuelConsumptionString", "Nieprawidłowy format zużycia paliwa.");
                        return View(car);
                    }

                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            return View(car);
        }

        // GET: Cars/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Car == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Car'  is null.");
            }
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                _context.Car.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Car?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
