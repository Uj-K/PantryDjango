﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PantryDjango.Data;
using PantryDjango.Models;
using PantryDjango.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PantryDjango.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly FoodDbContext _context;

        public FoodItemsController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: FoodItems
        public async Task<IActionResult> Index(string sortOrder)
        {
            // 정렬 상태를 ViewData에 저장
            ViewData["ExpirationDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["CurrentSort"] = sortOrder; // 현재 정렬 상태 저장

            var foodItems = from f in _context.FoodItems
                            select f;

            // 정렬 로직
            switch (sortOrder)
            {
                case "date_desc":
                    foodItems = foodItems.OrderByDescending(f => f.ExpirationDate);
                    break;
                default:
                    foodItems = foodItems.OrderBy(f => f.ExpirationDate);
                    break;
            }

            return View(await foodItems.AsNoTracking().ToListAsync());
        }

        // GET: FoodItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // GET: FoodItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ExpirationDate,Quantity,Unit,Barcode,AddedAt,UpdatedAt,Category,Location")] FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                foodItem.ExpirationDate = foodItem.ExpirationDate.Date;
                foodItem.AddedAt = DateTime.Now; // Set current time
                _context.Add(foodItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodItem);
        }

        [HttpPost]
        public async Task<IActionResult> OcrOnly(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return Content("No image");

            using var stream = imageFile.OpenReadStream();
            var ocr = new OcrService(@"C:\Program Files\Tesseract-OCR\tessdata");

            string rawText = ocr.ExtractExpiryDateFromStream(stream, "kor+eng"); // Use the correct method

            return Content(rawText); // Return the extracted text
        }

        // GET: FoodItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        // POST: FoodItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ExpirationDate,Quantity,Unit,AddedAt,UpdatedAt,Category,Location")] FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing entity from the database
                    var existingFoodItem = await _context.FoodItems.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
                    if (existingFoodItem == null)
                    {
                        return NotFound();
                    }

                    // Preserve the AddedAt value
                    foodItem.AddedAt = existingFoodItem.AddedAt;

                    // Update the UpdatedAt timestamp
                    foodItem.UpdatedAt = DateTime.Now;

                    _context.Update(foodItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodItemExists(foodItem.Id))
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
            return View(foodItem);
        }

        // GET: FoodItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // POST: FoodItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem != null)
            {
                _context.FoodItems.Remove(foodItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }

    }
}
