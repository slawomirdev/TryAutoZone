using System;
using System.Collections.Generic;
using System.Data;
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
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Car).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car.Where(c => !c.IsReserved), "Id", "Model");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("UserId,CarId,ReservationDate")] Reservation reservation)
        {
            var car = await _context.Car.FirstOrDefaultAsync(c => c.Id == reservation.CarId);
            if (car != null && !car.IsReserved)
            {
                car.IsReserved = true;
                _context.Update(car);

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                Console.WriteLine("Reservation created successfully.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("Car is already reserved or does not exist.");
                ModelState.AddModelError("", "Samochód jest już zarezerwowany lub nie istnieje.");
            }

            ViewData["CarId"] = new SelectList(_context.Car.Where(c => !c.IsReserved), "Id", "Model", reservation.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservation.UserId);
            return View(reservation);
        }
        // GET: Reservations/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CarId,ReservationDate")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            var originalReservation = await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (originalReservation == null)
            {
                return NotFound();
            }

            try
            {
                // Set IsReserved to false for the old car if the car is changed
                if (originalReservation.CarId != reservation.CarId)
                {
                    var oldCar = await _context.Car.FindAsync(originalReservation.CarId);
                    if (oldCar != null)
                    {
                        oldCar.IsReserved = false;
                        _context.Update(oldCar);
                    }

                    // Set IsReserved to true for the new car
                    var newCar = await _context.Car.FindAsync(reservation.CarId);
                    if (newCar != null)
                    {
                        newCar.IsReserved = true;
                        _context.Update(newCar);
                    }
                }

                _context.Update(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", reservation.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }




        // GET: Reservations/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations' is null.");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation != null)
            {
                // Jeśli samochód jest związany z rezerwacją, zmień jego status na niezarezerwowany
                if (reservation.Car != null)
                {
                    reservation.Car.IsReserved = false;
                    _context.Update(reservation.Car);
                }

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myReservations = await _context.Reservations
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(myReservations);
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
