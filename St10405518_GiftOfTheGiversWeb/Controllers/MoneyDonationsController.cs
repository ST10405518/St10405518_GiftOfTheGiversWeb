﻿using System;
using System.Collections.Generic;
using System.Data;
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

namespace St10405518_GiftOfTheGiversWeb.Controllers
{
    public class MoneyDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoneyDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoneyDonations
        public async Task<IActionResult> Index()
        {
            return _context.MoneyDonation != null ?
                        View(await _context.MoneyDonation.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.MoneyDonation'  is null.");
        }

        // GET: MoneyDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);
            if (moneyDonation == null)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        // GET: MoneyDonations/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in username
                string currentUsername = @User.Identity.Name;

                // Set the username of the donation to the current user's username
                moneyDonation.USERNAME = currentUsername;

                if (moneyDonation.DATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                    return View(moneyDonation);
                }

                // Check if the user selected "Anonymous" as the donor
                if (moneyDonation.DONOR == "Anonymous")
                {
                    moneyDonation.DONOR = "Anonymous";
                }
                else
                {
                    // Set DONOR to the current logged-in user's username
                    var currentUser = User.Identity?.Name;
                    moneyDonation.DONOR = currentUser;
                }

                // Retrieve the existing Money entity or create a new one
                var money = _context.Money.FirstOrDefault();

                if (money == null)
                {
                    money = new Money
                    {
                        TotalMoney = moneyDonation.AMOUNT,
                        RemainingMoney = moneyDonation.AMOUNT
                    };
                    _context.Add(money);
                }
                else
                {
                    // Update the existing Money entity
                    money.TotalMoney += moneyDonation.AMOUNT;
                    money.RemainingMoney += moneyDonation.AMOUNT;
                }

                // Add the money donation to the context
                _context.Add(moneyDonation);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(moneyDonation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            if (moneyDonation == null)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MONEY_DONATION_ID, USERNAME, DATE, AMOUNT, DONOR")] MoneyDonation moneyDonation)
        {
            if (id != moneyDonation.MONEY_DONATION_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Detach the existing entity
                    _context.Entry(moneyDonation).State = EntityState.Detached;

                    // Get the existing MoneyDonation
                    var existingMoneyDonation = await _context.MoneyDonation.FindAsync(id);

                    if (existingMoneyDonation == null)
                    {
                        return NotFound();
                    }

                    // Calculate the difference in donation amount for updating TotalMoney and RemainingMoney
                    decimal donationDifference = moneyDonation.AMOUNT - existingMoneyDonation.AMOUNT;

                    // Update TotalMoney and RemainingMoney accordingly
                    var money = _context.Money.First(); // Assuming you have a single record for Money

                    money.TotalMoney += donationDifference;
                    money.RemainingMoney += donationDifference;

                    // Update the existing entity with the new values
                    existingMoneyDonation.DATE = moneyDonation.DATE;
                    existingMoneyDonation.AMOUNT = moneyDonation.AMOUNT;
                    existingMoneyDonation.DONOR = moneyDonation.DONOR;

                    // Attach and update the existing entity
                    _context.Attach(existingMoneyDonation);
                    _context.Update(existingMoneyDonation);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyDonationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(moneyDonation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MoneyDonation == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            if (moneyDonation == null)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MoneyDonation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoneyDonation'  is null.");
            }

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);

            if (moneyDonation != null)
            {
                // Calculate the difference in donation amount for updating TotalMoney and RemainingMoney
                decimal donationDifference = -moneyDonation.AMOUNT;

                // Update TotalMoney and RemainingMoney accordingly
                var money = _context.Money.First(); // Assuming you have a single record for Money

                money.TotalMoney += donationDifference;
                money.RemainingMoney += donationDifference;

                _context.Update(money);
                _context.Remove(moneyDonation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationExists(int id)
        {
            return _context.MoneyDonation.Any(e => e.MONEY_DONATION_ID == id);
        }
    }
}
