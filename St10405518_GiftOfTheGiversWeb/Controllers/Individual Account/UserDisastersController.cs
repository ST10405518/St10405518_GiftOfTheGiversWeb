﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using St10405518_GiftOfTheGiversWeb;
using St10405518_GiftOfTheGiversWeb.Data;
using St10405518_GiftOfTheGiversWeb.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace St10405518_GiftOfTheGiversWeb.Controllers
{
    public class UserDisastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDisastersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Disasters/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disaster == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(m => m.DISTATER_ID == id);
            if (disaster == null)
            {
                return NotFound();
            }

            return View(disaster);
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // User is not logged in, handle this case as needed (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            // Get the current logged-in username
            string currentUsername = User.Identity.Name;

            // Query the data for the current username
            var userDisasters = await _context.Disaster
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userDisasters);
        }

        // GET: UserDisasters/Create
        [Authorize]
        public IActionResult Create()
        {
            var currentDate = DateTime.Now.Date;
            var tomorrow = currentDate.AddDays(1);

            var disaster = new Disaster
            {
                STARTDATE = currentDate,
                ENDDATE = tomorrow
            };

            return View(disaster);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] //any logged in user
        public async Task<IActionResult> Create([Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE,IsActive")] Disaster disaster)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in username
                string currentUsername = User.Identity.Name;

                // Set the username of the disaster to the current user's username
                disaster.USERNAME = currentUsername;

                // Check if the start date is before the current date
                if (disaster.STARTDATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                    return View(disaster);
                }

                // Check if the current date is in between the start and end dates
                if (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE)
                {
                    disaster.IsActive = 1; // 1 represents true in your integer-based flag
                }
                else
                {
                    disaster.IsActive = 0; // 0 represents false in your integer-based flag
                }

                if (disaster.ENDDATE < disaster.STARTDATE.Value.Date.AddDays(1))
                {
                    ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                    return View(disaster);
                }

                // Now add the disaster with MoneyAllocation to the context.
                _context.Add(disaster);

                // Save changes to the database.
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(disaster);
        }

        // GET: UserDisasters/Edit/5
        [Authorize(Roles = "Admin")] //only admin
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disaster == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster.FindAsync(id);

            if (disaster == null || disaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            if (id != disaster.DISTATER_ID)
            {
                return NotFound();
            }

            var existingDisaster = await _context.Disaster.FindAsync(id);

            if (existingDisaster == null || existingDisaster.USERNAME != @User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterExists(disaster.DISTATER_ID))
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
            return View(disaster);
        }

        // GET: UserDisasters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disaster == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(m => m.DISTATER_ID == id);

            if (disaster == null || disaster.USERNAME != User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            return View(disaster);
        }

        // POST: UserDisasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize (Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disaster   == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Disaster'  is null.");
            }

            var disaster = await _context.Disaster.FindAsync(id);

            if (disaster == null || disaster.USERNAME != User.Identity.Name)
            {
                // Either the disaster doesn't exist or it doesn't belong to the current user
                return NotFound();
            }

            _context.Disaster.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DisasterExists(int id)
        {
            return _context.Disaster.Any(e => e.DISTATER_ID == id && e.USERNAME == @User.Identity.Name);
        }
    }
}
